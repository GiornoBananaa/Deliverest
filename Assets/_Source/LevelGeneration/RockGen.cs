using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RockGen : MonoBehaviour
{
    [Serializable]
    public class LevelRow
    {
        public GameObject[] row;
    }
    
    [SerializeField] private int tile_width, tile_height, tiles_in_row, rows_number, min_ledges, max_ledges, generated_height;
    [SerializeField] private GameObject[] tilesWithLedges, tilesWithOutLedges;
    [SerializeField] private GameObject edgeTile, avalanche_prefab, avalanche_sighn_prefab, stone_prefab, stone_sighn_prefab;
    [SerializeField] private Transform playerBody;
    [SerializeField] private List<LevelRow> _startRows;
    
    private List<GameObject[]> rows = new List<GameObject[]>();
    private bool _isMoving;
    private bool isStorm;
    private bool stoneIsWaitingStormEnd;
    private bool avalancheIsWaitingStormEnd;
    private float timeForNextStone, timeForNextAvalanche;
    private Level level;
    private Camera _camera;

    public Action OnObstacle;
    
    void Start()
    {
        _camera = Camera.main;
        foreach (var row in _startRows)
        {
            rows.Add(row.row);
        }
        isStorm = false;
        level = GameManager.instance.currentLevel;
        timeForNextStone = level.stone_max_delay;
        timeForNextAvalanche = level.avalanche_max_delay;
    }


    void Update()
    {
        if (GameManager.instance.isPaused)
            return;
        timeForNextStone -= Time.deltaTime;
        timeForNextAvalanche -= Time.deltaTime;

        if (GameManager.instance.isSnowStorm != isStorm)
        {
            if (isStorm)
            {
                if (timeForNextStone < 4)
                    timeForNextStone = 4;
                if (timeForNextAvalanche < 5)
                    timeForNextStone = 5;
            }
            isStorm = GameManager.instance.isSnowStorm;
        }

        if (timeForNextStone <= 0)
        {
            if (!GameManager.instance.isSnowStorm)
            {
                if (stoneIsWaitingStormEnd)
                {
                    stoneIsWaitingStormEnd = false;
                    timeForNextStone = 5;
                }
                else
                    StartCoroutine(StoneEvent());
            }
            else
                stoneIsWaitingStormEnd = true;
        }
        if (timeForNextAvalanche <= 0)
        {
            if (!GameManager.instance.isSnowStorm)
            {
                if (avalancheIsWaitingStormEnd)
                {
                    avalancheIsWaitingStormEnd = false;
                    timeForNextAvalanche = 7;
                }
                else
                    StartCoroutine(AvalancheEvent());
            }
            else
                avalancheIsWaitingStormEnd = true;
        }

        if (_camera.transform.position.y >= generated_height - tile_height * 2)
        {
            Debug.Log(generated_height);
            GenRow();
        }
    }

    #region Obstacles
    private IEnumerator StoneEvent()
    {
        OnObstacle?.Invoke();
        timeForNextStone = Random.Range(level.stone_min_delay, level.avalanche_max_delay) + level.stone_sign_time;
        Vector3 pos = new Vector3(playerBody.position.x + Random.Range(-tile_width, tile_width), tile_height);
        GameObject sign = Instantiate(stone_sighn_prefab, pos, Quaternion.identity);
        yield return new WaitForSeconds(level.stone_sign_time);
        pos.y = 10;
        DropStone(pos);
        yield return new WaitForSeconds(1);
        Destroy(sign);
    }

    private void DropStone(Vector3 pos)
    {
        timeForNextStone = Random.Range(level.stone_min_delay, level.avalanche_max_delay);
        GameObject stone = Instantiate(stone_prefab, pos, Quaternion.identity);
    }

    private IEnumerator AvalancheEvent()
    {
        OnObstacle?.Invoke();
        timeForNextAvalanche = Random.Range(level.avalanche_min_delay, level.avalanche_max_delay) + level.avalanche_sign_time;
        Vector3 pos = new Vector3(transform.position.x + tile_width * 2f * Random.Range(-1, 2), tile_height);
        GameObject sign = Instantiate(avalanche_sighn_prefab, pos, Quaternion.identity);
        yield return new WaitForSeconds(level.avalanche_sign_time);
        pos.y = 20;
        DropAvalanche(pos);
        yield return new WaitForSeconds(1);
        Destroy(sign);
    }

    private void DropAvalanche(Vector3 pos)
    {
        GameObject avalanche = Instantiate(avalanche_prefab, pos, Quaternion.identity);
    }
    
    #endregion
    
    private void DelLastRaw()
    {
        GameObject[] rawToDel = rows[0];
        for (int i = 0; i < rawToDel.Length; i++)
        {
            Destroy(rawToDel[i]);
        }
        rows.Remove(rawToDel);
    }
    
    private void GenRow()
    {
        generated_height += tile_height;
        int ledges_ammount = Random.Range(min_ledges, max_ledges + 1);
        GameObject[] rowOfPrefabs = new GameObject[tiles_in_row];
        for (int i = 0; i < tiles_in_row; i++)
        {
            if (i < ledges_ammount)
                rowOfPrefabs[i] = tilesWithLedges[Random.Range(0, tilesWithLedges.Length)];
            else
                rowOfPrefabs[i] = tilesWithOutLedges[Random.Range(0, tilesWithOutLedges.Length)];
        }
        for (int i = rowOfPrefabs.Length - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (rowOfPrefabs[j], rowOfPrefabs[i]) = (rowOfPrefabs[i], rowOfPrefabs[j]);
        }
        GameObject[] rowOfTiles = new GameObject[tiles_in_row + 2];
        rowOfTiles[0] = Instantiate(edgeTile, transform);
        rowOfTiles[0].transform.position = new Vector3(tile_width / 2 - tile_width * (tiles_in_row + 1) / 2,
            generated_height);
        rowOfTiles[rowOfTiles.Length - 1] = Instantiate(edgeTile, transform);
        rowOfTiles[rowOfTiles.Length - 1].transform.position = new Vector3(- tile_width / 2 + tile_width * (tiles_in_row + 1) / 2,
            generated_height);
        rowOfTiles[rowOfTiles.Length - 1].transform.localScale = new Vector3(-1, 1, 1);
        for (int i = 0; i < tiles_in_row; i++)
        {
            rowOfTiles[i + 1] = Instantiate(rowOfPrefabs[i], transform);
            rowOfTiles[i + 1].transform.position = new Vector3(tile_width * i + tile_width / 2 - tile_width * tiles_in_row / 2,
                generated_height);
        }
        rows.Add(rowOfTiles);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    [SerializeField] private int tile_width, tile_height, tiles_in_row, rows_number, min_ledges, max_ledges;
    [SerializeField] private GameObject[] tilesWithLedges, tilesWithOutLedges;
    [SerializeField] private GameObject edgeTile, avalanche_prefab, avalanche_sighn_prefab, stone_prefab, stone_sighn_prefab;
    [SerializeField] private Transform player, playerBody;

    private List<GameObject[]> rows = new List<GameObject[]>();
    private bool _isMoving;
    private bool isStorm;
    private bool stoneIsWaitingStormEnd;
    private bool avalancheIsWaitingStormEnd;
    private float timeForNextStone, timeForNextAvalanche;
    private Level level;
    private PlayerManager playerManager;

    void Start()
    {
        isStorm = false;
        playerManager = player.GetComponent<PlayerManager>();
        level = GameManager.instance.currentLevel;
        timeForNextStone = level.stone_max_delay;
        timeForNextAvalanche = level.avalanche_max_delay;
        for (int i = 0; i < rows_number; i++)
        {
            FastScroll();
            GenRow();
        }
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

        if (playerBody.position.y > transform.position.y && playerManager.IsOnTwoHands)
        {
            StartCoroutine(SmoothScroll());
        }
    }

    private IEnumerator StoneEvent()
    {
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
        StartCoroutine(SoundLerp(stone));
    }

    private IEnumerator AvalancheEvent()
    {
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
        StartCoroutine(SoundLerp(avalanche));
    }
    private void FastScroll()
    {
        foreach (GameObject[] row in rows)
        {
            foreach (GameObject tile in row)
            {
                tile.transform.position += Vector3.down * tile_height;
            }
        }

    }
    private IEnumerator SmoothScroll()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            Vector3 startPosition = transform.position;
            for (float i = 0f; i <= 1.0f; i += 0.02f)
            {
                transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.down * tile_height, i);
                yield return new WaitForSeconds(0.01f);
            }
            DelLastRaw();
            transform.position = startPosition;
            GameManager.instance.height += tile_height;
            player.position += Vector3.down * tile_height;
            FastScroll();
            GenRow();
            _isMoving = false;
        }

    }
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
            (rows_number - 2) * tile_height);
        rowOfTiles[rowOfTiles.Length - 1] = Instantiate(edgeTile, transform);
        rowOfTiles[rowOfTiles.Length - 1].transform.position = new Vector3(- tile_width / 2 + tile_width * (tiles_in_row + 1) / 2,
            (rows_number - 2) * tile_height);
        rowOfTiles[rowOfTiles.Length - 1].transform.localScale = new Vector3(-1, 1, 1);
        for (int i = 0; i < tiles_in_row; i++)
        {
            rowOfTiles[i + 1] = Instantiate(rowOfPrefabs[i], transform);
            rowOfTiles[i + 1].transform.position = new Vector3(tile_width * i + tile_width / 2 - tile_width * tiles_in_row / 2,
                (rows_number - 2) * tile_height);
        }
        rows.Add(rowOfTiles);
    }

    private IEnumerator SoundLerp(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        float deafultVolume = audio.volume;
        audio.volume = 0;
        while (audio.volume < deafultVolume-0.05f)
        {
            audio.volume = Mathf.Lerp(audio.volume, deafultVolume, 0.025f);
            yield return new WaitForFixedUpdate();
        }
    }
}

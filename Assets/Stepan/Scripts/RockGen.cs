using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    [SerializeField] private int tile_width, tile_height, tiles_in_row, rows_number, min_ledges, max_ledges;
    [SerializeField] private GameObject[] tilesWithLedges, tilesWithOutLedges;
    [SerializeField] private Transform player, playerBody;
    private List<GameObject[]> rows = new List<GameObject[]>();
    private bool _isMoving;
    void Start()
    {
        for (int i = 0; i < rows_number; i++)
        {
            FastScroll();
            GenRow();
        }
    }


    void Update()
    {
        if (playerBody.position.y > transform.position.y)
        {
            StartCoroutine( SmoothScroll());
            
        }
    }
    private void FastScroll()
    {
        foreach (GameObject[] row in rows)
        {
            foreach(GameObject tile in row)
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
        int ledges_ammount = Random.Range(min_ledges, max_ledges+1);
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
        GameObject[] rowOfTiles = new GameObject[tiles_in_row];
        for (int i = 0; i < tiles_in_row; i++)
        {
            rowOfTiles[i] = Instantiate(rowOfPrefabs[i], transform);
            rowOfTiles[i].transform.position = new Vector3(tile_width * i + tile_width / 2 - tile_width * tiles_in_row / 2,
                (rows_number - 2) * tile_height);
        }
        rows.Add(rowOfTiles);
    }
}

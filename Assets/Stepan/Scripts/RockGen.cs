using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGen : MonoBehaviour
{
    [SerializeField] private int tile_width, tile_height, tiles_in_row, rows_number, min_ledges;
    [SerializeField] private GameObject[] tilesWithLedges, tilesWithOutLedges;
    private List<GameObject[]> rows = new List<GameObject[]>();
    void Start()
    {
        for (int i = 0; i < rows_number; i++)
        {
            Scroll();
            GenRow();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DelLastRaw();
            Scroll();
            GenRow();
        }
    }
    private void Scroll()
    {
        foreach (GameObject[] row in rows)
        {
            foreach(GameObject tile in row)
            {
                tile.transform.position += Vector3.down * tile_height;
            }
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
        int ledges_ammount = Random.Range(min_ledges, tiles_in_row - 1);
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
            Vector3 position = new Vector3(tile_width * i + tile_width / 2 - tile_width * tiles_in_row / 2,
                rows_number * tile_height );
            rowOfTiles[i] = Instantiate(rowOfPrefabs[i], position, Quaternion.identity);
        }
        rows.Add(rowOfTiles);
    }
}

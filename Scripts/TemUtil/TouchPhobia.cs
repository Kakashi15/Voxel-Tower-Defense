using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TouchPhobia : MonoBehaviour
{
    public List<TileBase> tileList = new List<TileBase>();
    Vector3Int grid;
    Vector3Int[] adjacentOffsets;
    void Start()
    {
        grid = BuildingSystem.BuildingSystemInstance.tileMap.WorldToCell(transform.position);
        tileList = new List<TileBase>(4);
        TileBase temp = null;
        for (int i = 0; i < 4; i++)
        {
            tileList.Add(temp);
        }
        adjacentOffsets = new Vector3Int[]
        {
        new Vector3Int(-1, 0, 0), // Left
        new Vector3Int(1, 0, 0),  // Right
        new Vector3Int(0, -1, 0), // Down
        new Vector3Int(0, 1, 0),   // Up
        };


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            TileBase tile = BuildingSystem.BuildingSystemInstance.tileMap.GetTile(grid + adjacentOffsets[i]);
            tileList[i] = tile;
        }

        if (!canDrawBorder())
            Destroy(gameObject);
    }

    bool canDrawBorder()
    {
        for (int i = 0; i < tileList.Count; i++)
        {
            int j = i + 1;
            if (j >= tileList.Count)
                j = 0;

            if (tileList[i] != tileList[j])
                return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.layer == 8)
        //{
        //    Destroy(collision.gameObject);
        //    Destroy(gameObject);
        //}
    }

}

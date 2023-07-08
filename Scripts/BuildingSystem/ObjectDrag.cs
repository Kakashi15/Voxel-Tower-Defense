using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectDrag : MonoBehaviour
{
    public Vector3 offset;
    Vector3 pos;
    public bool isPlacing = true;
    public bool ifEnemy = false;
    ObjectSelector objectSelector;
    private void Start()
    {
        objectSelector = FindObjectOfType<ObjectSelector>();
    }

    private void Update()
    {
        if (isPlacing)
        {
            pos = BuildingSystem.BuildingSystemInstance.GetMouseWorldPosition() + offset;
            transform.position = BuildingSystem.BuildingSystemInstance.SnapCoordinateToGrid(pos);
        }

        if (Input.GetMouseButtonDown(1))
        {

            if (!CheckIfPlacementAllowed(pos) || !isPlacing)
                return;

            Vector3Int grid = BuildingSystem.BuildingSystemInstance.gridLayout.WorldToCell(pos);

            Vector3Int[] adjacentOffsets = new Vector3Int[]
            {
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(-1, 1, 0), // Up Left
            new Vector3Int(-1, -1, 0), // Down Left
            new Vector3Int(1, 1, 0), // Up Right
            new Vector3Int(1, 0, 0),  // Right
            new Vector3Int(1, -1, 0), // Down Right
            new Vector3Int(0, -1, 0), // Down
            new Vector3Int(0, 1, 0),   // Up
            new Vector3Int(0,0,0)
            };

            foreach (Vector3Int offset in adjacentOffsets)
            {
                Vector3Int adjacentCellPosition = grid + offset;
                if (!ifEnemy)
                    BuildingSystem.BuildingSystemInstance.tileMap.SetTile(adjacentCellPosition, BuildingSystem.BuildingSystemInstance.playerTileBase);
                else
                    BuildingSystem.BuildingSystemInstance.tileMap.SetTile(adjacentCellPosition, BuildingSystem.BuildingSystemInstance.enemyTileBase);

            }

            BuildingSystem.BuildingSystemInstance.CreateHollowBox(BuildingSystem.BuildingSystemInstance.grid.GetCellCenterWorld(grid));
            objectSelector.selectableObjects.Add(gameObject.GetComponent<UnitController>());
            isPlacing = false;
        }
    }

    private bool CheckIfPlacementAllowed(Vector3 pos)
    {
        Vector3Int grid = BuildingSystem.BuildingSystemInstance.gridLayout.WorldToCell(pos);

        Vector3Int[] adjacentOffsets = new Vector3Int[]
            {
            new Vector3Int(-2, 0, 0), // Left
            new Vector3Int(-2, 2, 0), // Up Left
            new Vector3Int(-2, -2, 0), // Down Left
            new Vector3Int(2, 2, 0), // Up Right
            new Vector3Int(2, 0, 0),  // Right
            new Vector3Int(2, -2, 0), // Down Right
            new Vector3Int(0, -2, 0), // Down
            new Vector3Int(0, 2, 0),   // Up
            new Vector3Int(0,0,0)
            };

        foreach (Vector3Int offset in adjacentOffsets)
        {
            Vector3Int adjacentCellPosition = grid + offset;
            TileBase tile = BuildingSystem.BuildingSystemInstance.tileMap.GetTile<TileBase>(adjacentCellPosition);
            if (!ifEnemy)
            {
                if (tile == BuildingSystem.BuildingSystemInstance.playerTileBase)
                    return true;
            }
            else
            {
                if (tile == BuildingSystem.BuildingSystemInstance.enemyTileBase)
                    return true;
            }
        }
        return false;
    }

    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.BuildingSystemInstance.GetMouseWorldPosition();
    }

    void GetAdjacentCells(Vector3Int cellPosition)
    {

    }
}

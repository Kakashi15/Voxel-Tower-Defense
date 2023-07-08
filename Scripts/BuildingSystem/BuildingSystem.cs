using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem BuildingSystemInstance;

    public GridLayout gridLayout;
    public Tilemap tileMap;
    public TileBase playerTileBase;
    public TileBase enemyTileBase;
    public TileBase tileGround;
    public Grid grid;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlacedObject objectToPlace;


    public Vector3 centerPosition;

    public Vector3Int tilePosition;

    public float cubeWidth = 4f;
    public float cubeOffsetPos = 3f;

    void Awake()
    {
        BuildingSystemInstance = this;
        grid = gridLayout.GetComponent<Grid>();

        tileMap.BoxFill(Vector3Int.one, tileGround, 2, 2, 4, 4);

        //CreateHollowBox();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InitializeObject(prefab1);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            InitializeObject(prefab2);
        }
    }



    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.one;
        }
    }


    public Vector3 SnapCoordinateToGrid(Vector3 pos)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        pos = grid.GetCellCenterWorld(cellPos);
        return pos;
    }


    public void InitializeObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlacedObject>();
        //obj.AddComponent<ObjectDrag>();
    }


    public void CreateHollowBox(Vector3 pos)
    {
        Vector3 tileWorldPosition = tileMap.CellToWorld(tilePosition);
        Vector3 boxCenter = tileWorldPosition + tileMap.cellSize;

        Vector3 boxSize = new Vector3(1, 1, 1);

        Vector3Int minGridPosition = tilePosition - new Vector3Int(1, 1, 0);
        Vector3Int maxGridPosition = tilePosition + new Vector3Int(1, 1, 0);

        Vector3Int[] adjacentOffsets = new Vector3Int[]
        {
         new Vector3Int(-(int)(tileMap.cellSize.x + cubeOffsetPos), 0, 0), // Left
         new Vector3Int((int)(tileMap.cellSize.x + cubeOffsetPos), 0, 0),  // Right
         new Vector3Int(0, -(int)(tileMap.cellSize.y + cubeOffsetPos), 0), // Down
         new Vector3Int(0, (int)(tileMap.cellSize.y + cubeOffsetPos), 0),   // Up
        };

        Vector3Int[] scaleSet = new Vector3Int[]
        {
         new Vector3Int(-(int)(cubeWidth), 0, 0), // Left
         new Vector3Int((int)(cubeWidth), 0, 0),  // Right
         new Vector3Int(0, -(int)(cubeWidth), 0), // Down
         new Vector3Int(0, (int)(cubeWidth), 0),   // Up
        };

        GameObject border = GameObject.CreatePrimitive(PrimitiveType.Plane);
        border.transform.position = pos;

        //foreach (Vector3Int offset in adjacentOffsets)
        //{
        //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    cube.transform.position = pos + offset;
        //    cube.transform.localScale = boxSize + offset;
        //    cube.GetComponent<Renderer>().material.color = Color.white;
        //    cube.transform.SetParent(border.transform);
        //}

        for (int i = 0; i < adjacentOffsets.Length; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = pos + adjacentOffsets[i];
            cube.transform.gameObject.AddComponent<BoxCollider>();
            cube.gameObject.AddComponent<TouchPhobia>();
            cube.layer = 8;
            //Rigidbody rb = cube.gameObject.AddComponent<Rigidbody>();
            //rb.freezeRotation = true;
            //rb.constraints = RigidbodyConstraints.FreezeRotation;
            //rb.constraints = RigidbodyConstraints.FreezePosition;
            //rb.useGravity = false;
            cube.GetComponent<Renderer>().material.color = Color.blue;
            cube.transform.SetParent(border.transform);
        }

        border.transform.eulerAngles = new Vector3(90, 0, 0);

        for (int i = scaleSet.Length - 1, j = 0; i >= 0 && j < scaleSet.Length; i--, j++)
        {
            border.transform.GetChild(j).transform.localScale = boxSize + scaleSet[i];
        }

        /*for (int i = 0; i < 4; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = pos + new Vector3(0f, 0f, 0.5f) * cubeWidth;
            cube.transform.localScale = boxSize;
            cube.GetComponent<Renderer>().material.color = Color.white;
        }*/
    }
}

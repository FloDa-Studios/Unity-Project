using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloDa.Utils;
using UnityEngine.Tilemaps;

public class ShipGrid
{
    private int width, height;
    private float cellSize;
    private Vector3 originPosition;
    public int[,] gridArray;
    private TextMesh[,] debugTextArray;
    private string idx;

    // protected ShipGrid() {}

    public ShipGrid()
    {
        Tilemap tilemap = GameObject.Find("Tilemap_Ship").GetComponent<Tilemap>();

        Debug.Log("ShipGrid initialisert!");
        Debug.Log("ShipGrid W" + width + " H" + height + " CellSize" + cellSize);

        this.width = tilemap.cellBounds.size.x;
        this.height = tilemap.cellBounds.size.y;
        this.cellSize = GameObject.Find("Grid").GetComponent<Grid>().transform.localScale.x;
        this.originPosition = new Vector3(tilemap.cellBounds.xMin * cellSize, tilemap.cellBounds.yMin * cellSize, 0);

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        Debug.Log("grid: " + width + "w " + height + "h");

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = -1;
                debugTextArray[x, y] = Utils.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, -1);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public int GetValue(int x, int y)
    {
        return gridArray[x,y];
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x,y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void SetValue(int x, int y, int value)
    {
        gridArray[x,y] = value;
        debugTextArray[x,y].text = gridArray[x,y].ToString();
        if (value > 0)
        {
            debugTextArray[x,y].color = Color.green;
        }
        else
        {
            debugTextArray[x,y].color = Color.white;
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x,y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }



    public void SetMap()
    {
        Tilemap tilemap = GameObject.Find("Tilemap_Ship").GetComponent<Tilemap>();

        for (int x = 0; x < tilemap.cellBounds.size.x; x++)
        {
            for (int y = 0; y < tilemap.cellBounds.size.y; y++)
            {
                AdvancedRuleTile tile = (AdvancedRuleTile)tilemap.GetTile(new Vector3Int(x + tilemap.cellBounds.xMin, y + tilemap.cellBounds.yMin, 0));
                if (tile == null)
                {
                    SetValue(x, y, -1);
                }
                else if (tile.name == "ConnectorAdv")
                {
                    SetValue(x, y, 0);
                }
                else if (tile.name == "CockpitAdv")
                {
                    SetValue(x, y, 1);
                }
                else if (tile.name == "HardpointAdv")
                {
                    SetValue(x, y, 2);
                }
                else if (tile.name == "ReactorAdv")
                {
                    SetValue(x, y, 3);
                }
                else if (tile.name == "EngineroomAdv")
                {
                    SetValue(x, y, 4);
                }
                else if (tile.name == "EngineAdv")
                {
                    SetValue(x, y, 5);
                }
                else if (tile.name == "StorageAdv")
                {
                    SetValue(x, y, 6);
                }
                else if (tile.name == "BedroomAdv")
                {
                    SetValue(x, y, 7);
                }
            }
        }



        /*
        if(mapIndex == 0)
        {
            Debug.Log("Noch kein Schiff ausgewählt!");
        }

        if (mapIndex == 1)
        {
            GameObject gameObject = GameObject.Find("Tilemap_Ship");
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.enabled = true;
            gameObject = GameObject.Find("Tilemap_Crew");
            renderer = gameObject.GetComponent<Renderer>();
            renderer.enabled = true;

            //Engines 5
            SetValue(3,2,5);
            SetValue(3,7,5);

            //Engineroom 4
            SetValue(4,2,4);
            SetValue(4,7,4);

            //Reactor 3
            SetValue(7,4,3);
            SetValue(7,5,3);
            SetValue(8,4,3);
            SetValue(8,5,3);

            //Hardpoints 2
            SetValue(10,3,2);
            SetValue(10,6,2);
            SetValue(11,3,2);
            SetValue(11,6,2);

            //Cockpit 1
            SetValue(15,4,1);
            SetValue(15,5,1);

            //Space 0
            SetValue(5,2,0);
            SetValue(5,7,0);
            SetValue(6,2,0);
            SetValue(6,3,0);
            SetValue(6,6,0);
            SetValue(6,7,0);
            SetValue(7,2,0);
            SetValue(7,3,0);
            SetValue(7,6,0);
            SetValue(7,7,0);
            SetValue(8,2,0);
            SetValue(8,3,0);
            SetValue(8,6,0);
            SetValue(8,7,0);
            SetValue(9,3,0);
            SetValue(9,4,0);
            SetValue(9,5,0);
            SetValue(9,6,0);
            SetValue(10,4,0);
            SetValue(10,5,0);
            SetValue(11,4,0);
            SetValue(11,5,0);
            SetValue(12,4,0);
            SetValue(12,5,0);
            SetValue(13,4,0);
            SetValue(13,5,0);
            SetValue(14,4,0);
            SetValue(14,5,0);

            return 1;
        }
        else if (mapIndex == 2)
        {
            GameObject gameObject = GameObject.Find("Tilemap_Ship_1");
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.enabled = false;
            gameObject = GameObject.Find("Tilemap_Crew_1");
            renderer = gameObject.GetComponent<Renderer>();
            renderer.enabled = false;

            gameObject = GameObject.Find("Tilemap_Ship_2");
             renderer = gameObject.GetComponent<Renderer>();
            renderer.enabled = true;
            gameObject = GameObject.Find("Tilemap_Crew_2");
            renderer = gameObject.GetComponent<Renderer>();
            renderer.enabled = true;

            //Engines 5
            SetValue(3,2,5);
            SetValue(3,3,5);
            SetValue(3,6,5);
            SetValue(3,7,5);

            //Engineroom 4
            SetValue(4,2,4);
            SetValue(4,3,4);
            SetValue(4,6,4);
            SetValue(4,7,4);

            //Reactor 3
            SetValue(5,4,3);
            SetValue(5,5,3);
            SetValue(6,4,3);
            SetValue(6,5,3);
            SetValue(7,4,3);
            SetValue(7,5,3);

            //Hardpoints 2
            SetValue(8,1,2);
            SetValue(8,8,2);
            SetValue(7,1,2);
            SetValue(7,8,2);

            //Cockpit 1
            SetValue(15,4,1);
            SetValue(15,5,1);

            //Space 0
            SetValue(5,2,0);
            SetValue(5,3,0);
            SetValue(5,6,0);
            SetValue(5,7,0);
            SetValue(6,2,0);
            SetValue(6,3,0);
            SetValue(6,6,0);
            SetValue(6,7,0);
            SetValue(7,2,0);
            SetValue(7,3,0);
            SetValue(7,6,0);
            SetValue(7,7,0);
            SetValue(8,2,0);
            SetValue(8,3,0);
            SetValue(8,4,0);
            SetValue(8,5,0);
            SetValue(8,6,0);
            SetValue(8,7,0);
            SetValue(9,2,0);
            SetValue(9,3,0);
            SetValue(9,4,0);
            SetValue(9,5,0);
            SetValue(9,6,0);
            SetValue(9,7,0);
            SetValue(10,3,0);
            SetValue(10,4,0);
            SetValue(10,5,0);
            SetValue(10,6,0);
            SetValue(11,3,0);
            SetValue(11,4,0);
            SetValue(11,5,0);
            SetValue(11,6,0);
            SetValue(12,3,0);
            SetValue(12,4,0);
            SetValue(12,5,0);
            SetValue(12,6,0);
            SetValue(13,3,0);
            SetValue(13,4,0);
            SetValue(13,5,0);
            SetValue(13,6,0);
            SetValue(14,3,0);
            SetValue(14,4,0);
            SetValue(14,5,0);
            SetValue(14,6,0);

            return 1;
        }
        else
        {
            return -1;
        }
        */
    }
}

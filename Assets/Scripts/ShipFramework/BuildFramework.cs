using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using FloDa.Utils;
using UnityEngine.Tilemaps;

public class BuildFramework : ClassInstance<BuildFramework>
{

    public Tilemap tilemapGetTiles;
    public Tilemap tilemap, wallmap;
    public AdvancedRuleTile currentTile, connectorTile, cockpitTile, hardpointTile, reactorTile, engineroomTile, engineTile, storageTile, bedroomTile, spacewallTile;
    Vector3 worldPosition, originPosition;
    ShipDataControl control;
    float cellSize;

    public void MouseInput()
    {
        worldPosition = Utils.GetMouseWorldPosition();

        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);

        SetTile(Utils.GridArrayToWorldPosition(new Vector3Int(x, y, 0)),currentTile);
    }

    public void Initialize()
    {
        tilemap = GameObject.Find("Tilemap_Ship").GetComponent<Tilemap>();
        wallmap = GameObject.Find("Tilemap_SpaceWall").GetComponent<Tilemap>();
        tilemapGetTiles = GameObject.Find("Tilemap_GetTiles").GetComponent<Tilemap>();
        connectorTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(0,-1,0));
        cockpitTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(2, -1, 0));
        hardpointTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(4, -1, 0));
        reactorTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(6, -1, 0));
        engineroomTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(8, -1, 0));
        engineTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(10, -1, 0));
        storageTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(12, -1, 0));
        bedroomTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(14, -1, 0));
        spacewallTile = tilemapGetTiles.GetTile<AdvancedRuleTile>(new Vector3Int(16, -1, 0));
        control = GameObject.Find("ShipData").GetComponent<ShipDataControl>();
        originPosition = control.gridOffset;
        cellSize = control.gridCellSize;

    }

    public void SetTile(Vector3Int pos, AdvancedRuleTile tile)
    {
        tilemap.SetTile(pos, tile);
        if (tile == null)
        {
            wallmap.SetTile(pos, null);
        }
        else
        {
            wallmap.SetTile(pos, spacewallTile);
        }     
    }

    public void ClearAllTiles()
    {
        for (int x = 0; x < tilemap.cellBounds.size.x; x++)
        {
            for (int y = 0; y < tilemap.cellBounds.size.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x + tilemap.cellBounds.xMin, y + tilemap.cellBounds.yMin, 0), null);
                wallmap.SetTile(new Vector3Int(x + tilemap.cellBounds.xMin, y + tilemap.cellBounds.yMin, 0), null);
            }
        }
    }

    public void SelectTile(string buttonName)
    {
        if (buttonName == "ButtonConnector")
        {
            currentTile = connectorTile;
        }
        else if (buttonName == "ButtonCockpit")
        {
            currentTile = cockpitTile;
        }
        else if (buttonName == "ButtonHardpoint")
        {
            currentTile = hardpointTile;
        }
        else if (buttonName == "ButtonReactor")
        {
            currentTile = reactorTile;
        }
        else if (buttonName == "ButtonEngineroom")
        {
            currentTile = engineroomTile;
        }
        else if (buttonName == "ButtonEngine")
        {
            currentTile = engineTile;
        }
        else if (buttonName == "ButtonStorage")
        {
            currentTile = storageTile;
        }
        else if (buttonName == "ButtonBedroom")
        {
            currentTile = bedroomTile;
        }
        else if (buttonName == "ButtonDeleteTile")
        {
            currentTile = null;
        }

        Debug.Log("currentTile: " + currentTile);
    }
}

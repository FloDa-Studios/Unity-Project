using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using FloDa.Utils;
using UnityEngine.Tilemaps;

public class BuildFramework : ClassInstance<BuildFramework>
{
    private BuildFramework buildFramework = null;

    private ShipDataControl  shipDataControl;

    private Tilemap tilemap, wallmap, getTiles;

    private Vector3 worldPosition, originPosition;
    private float cellSize;

    public AdvancedRuleTile currentTile, connectorTile, cockpitTile, hardpointTile, reactorTile, engineroomTile, engineTile, storageTile, bedroomTile, spacewallTile;

    protected BuildFramework(GameObject tilemapGetTiles, GameObject tilemapShip, GameObject tilemapSpaceWall, float gridCellSize, Vector3 gridOffset) {

        this.tilemap = tilemapShip.GetComponent<Tilemap>();
        this.wallmap = tilemapSpaceWall.GetComponent<Tilemap>();
        this.getTiles = tilemapGetTiles.GetComponent<Tilemap>();

        this.connectorTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(0, -1, 0));
        this.cockpitTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(2, -1, 0));
        this.hardpointTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(4, -1, 0));
        this.reactorTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(6, -1, 0));
        this.engineroomTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(8, -1, 0));
        this.engineTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(10, -1, 0));
        this.storageTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(12, -1, 0));
        this.bedroomTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(14, -1, 0));
        this.spacewallTile = getTiles.GetTile<AdvancedRuleTile>(new Vector3Int(16, -1, 0));

        this.originPosition = gridOffset;
        this.cellSize = gridCellSize;

    }

    public void Initialize(GameObject tilemapGetTiles, GameObject tilemapShip, GameObject tilemapSpaceWall, float gridCellSize, Vector3 gridOffset) {
        if (buildFramework == null) {
            this.buildFramework = new BuildFramework(tilemapGetTiles, tilemapShip, tilemapSpaceWall, gridCellSize, gridOffset);
        }
    }

    public void MouseInput() {
        worldPosition = Utils.GetMouseWorldPosition();

        int x = Mathf.FloorToInt((worldPosition - this.originPosition).x / this.cellSize);
        int y = Mathf.FloorToInt((worldPosition - this.originPosition).y / this.cellSize);

        SetTile(Utils.GridArrayToWorldPosition(new Vector3Int(x, y, 0)), currentTile);
    }

    public void SetTile(Vector3Int pos, AdvancedRuleTile tile) {
        this.tilemap.SetTile(pos, tile);
        if (tile == null)
        {
            this.wallmap.SetTile(pos, null);
        }
        else
        {
            this.wallmap.SetTile(pos, this.spacewallTile);
        }     
    }

    public void ClearAllTiles() {
        for (int x = 0; x < this.tilemap.cellBounds.size.x; x++)
        {
            for (int y = 0; y < this.tilemap.cellBounds.size.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x + this.tilemap.cellBounds.xMin, y + this.tilemap.cellBounds.yMin, 0), null);
                wallmap.SetTile(new Vector3Int(x + this.tilemap.cellBounds.xMin, y + this.tilemap.cellBounds.yMin, 0), null);
            }
        }
    }

    public void SelectTile(string buttonName) {

        if (buttonName == "ButtonConnector") {
            currentTile = connectorTile;
        }
        else if (buttonName == "ButtonCockpit") {
            currentTile = cockpitTile;
        }
        else if (buttonName == "ButtonHardpoint") {
            currentTile = hardpointTile;
        }
        else if (buttonName == "ButtonReactor") {
            currentTile = reactorTile;
        }
        else if (buttonName == "ButtonEngineroom") {
            currentTile = engineroomTile;
        }
        else if (buttonName == "ButtonEngine") {
            currentTile = engineTile;
        }
        else if (buttonName == "ButtonStorage") {
            currentTile = storageTile;
        }
        else if (buttonName == "ButtonBedroom") {
            currentTile = bedroomTile;
        }
        else if (buttonName == "ButtonDeleteTile") {
            currentTile = null;
        }

        Debug.Log("currentTile: " + currentTile);
    }
}

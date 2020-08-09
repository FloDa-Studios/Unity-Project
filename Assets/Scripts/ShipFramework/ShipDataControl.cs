using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System;
using FloDa.Utils;
using TMPro;
using UnityEngine.Tilemaps;
using System.Xml.Linq;
using System.Text;
using UnityEngine.UI;


public class ShipDataControl : MonoBehaviour
{
    public static ShipDataControl persistentData = null;

    public int gridWidth, gridHeight;
    public float gridCellSize;
    public Vector3 gridOffset;
    int shipIndex;
    public Type type;
    public GameObject tilemap;
    public ShipGrid grid;

    public int garageSlotAmount;

    public GameObject GarageSlotPanel, stockPanel;

    public GameObject CrewUI, ShipUI, BuildUI;

    int returnValue;

    public static bool ranOnce;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {

        //TODO: Modulanzahl festlegen und lesen/schreiben, CrewMember LoadLevel resistent machen
        //TODO: UI Rework vom Inspect Mode (Crew besser festlegen) evtl erst wenn rekrutierung fertig
        //FEATURE: AUSWERTUNG des Schiffs für andere Module einsehbar machen
        //FEATURE: Panzerung anbringen, Schildgenerator, funktionalität für Modulanzahl
        //PLANNED: Türen einbauen, Hinergrund mit Markierung für Baubereich (evtl vergrößerbar)

        GarageSlotPanel.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, garageSlotAmount*240);

        for (int i = 0; i < garageSlotAmount; i++)
        {
            GameObject panel = Instantiate(stockPanel);
            panel.name = "Ship " + i;
            panel.transform.SetParent(GarageSlotPanel.transform);
            panel.transform.localPosition = new Vector3(122 + 240*i, -100, 0);
            panel.GetComponentInChildren<TextMeshProUGUI>().text = "Ship " + i;
        }
        
        ShipFramework.Instance.Initialize();

        BuildFramework.Instance.Initialize();

        shipIndex = ShipFramework.Instance.shipIndex;

        grid = new ShipGrid();
        grid.SetMap();

        returnValue = ShipModuleManager.Instance.Initialize();

        ShipModuleManager.Instance.shipModules = new List<ShipModule>();

        if (returnValue == 1)
        {
            Debug.Log("ShipModuleManager initialisert!");
        }
        else if (returnValue == 0)
        {
            Debug.Log("ShipModuleManager existiert bereits!");
        }

        CrewManagement.Instance.Initialize(15, 14, grid);
        UIHandler.Instance.Initialize();
    }

    public void SetAllModules()
    {
        ShipModuleManager.Instance.SetAllModules(grid);
    }

    public void SavePresets()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/shipPresets.dat");
        ShipPresets data = new ShipPresets();
        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Ship presets saved: " + Application.persistentDataPath);
    }


    public void LoadPresets()
    {
        if (File.Exists(Application.persistentDataPath + "/shipPresets.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/shipPresets.dat", FileMode.Open);
            ShipPresets data = (ShipPresets)bf.Deserialize(file);
            file.Close();

            TMP_Dropdown field = GameObject.Find("DropdownShips").GetComponent<TMP_Dropdown>();
            field.ClearOptions();
            field.AddOptions(data.options);
        }
    }

    public void SaveCrewPreset()
    {
        grid.SetMap();
        SetAllModules();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/crewPresetShip_" + ShipFramework.Instance.shipIndex + ".dat");

        CrewDataSave data = new CrewDataSave();

        bf.Serialize(file, data);
        file.Close();

        BinaryFormatter bf2 = new BinaryFormatter();
        FileStream file2 = File.Create(Application.persistentDataPath + "/shipTileData_" + ShipFramework.Instance.shipIndex + ".dat");

        TileMapSave data2 = new TileMapSave();
        bf2.Serialize(file2, data2);
        file2.Close();

        Debug.Log("Ship saved: " + Application.persistentDataPath);
    }

    public void LoadCrewPreset()
    {
        if (File.Exists(Application.persistentDataPath + "/shipTileData_" + ShipFramework.Instance.shipIndex + ".dat"))
        {

            BinaryFormatter bf2 = new BinaryFormatter();
            FileStream file2 = File.Open(Application.persistentDataPath + "/shipTileData_" + ShipFramework.Instance.shipIndex + ".dat", FileMode.Open);
            TileMapSave data2 = (TileMapSave)bf2.Deserialize(file2);
            file2.Close();
            AdvancedRuleTile tile;

            BuildFramework.Instance.ClearAllTiles();

            for (int x = 0; x < data2.sizex; x++)
            {
                for (int y = 0; y < data2.sizey; y++)
                {
                    int value = data2.array[x][y];

                    if (value == 1)
                    {
                        tile = BuildFramework.Instance.connectorTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 2)
                    {
                        tile = BuildFramework.Instance.cockpitTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 3)
                    {
                        tile = BuildFramework.Instance.hardpointTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 4)
                    {
                        tile = BuildFramework.Instance.reactorTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 5)
                    {
                        tile = BuildFramework.Instance.engineroomTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 6)
                    {
                        tile = BuildFramework.Instance.engineTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 7)
                    {
                        tile = BuildFramework.Instance.storageTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                    if (value == 8)
                    {
                        tile = BuildFramework.Instance.bedroomTile;
                        BuildFramework.Instance.SetTile(new Vector3Int(x + data2.minx, y + data2.miny, 0), tile);
                    }
                }
            }
        }

        UIHandler.Instance.UpdateHP();
        grid.SetMap();
        SetAllModules();

        if (File.Exists(Application.persistentDataPath + "/crewPresetShip_" + ShipFramework.Instance.shipIndex + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/crewPresetShip_" + ShipFramework.Instance.shipIndex + ".dat", FileMode.Open);
            CrewDataSave data = (CrewDataSave)bf.Deserialize(file);
            file.Close();

            CrewManagement.Instance.crewCockpit = data.crewCockpit;
            CrewManagement.Instance.crewHardpoints = data.crewHardpoints;
            CrewManagement.Instance.crewReactor = data.crewReactor;
            CrewManagement.Instance.crewEngineroom = data.crewEngineroom;
            CrewManagement.Instance.crewUsed = data.crewUsed;

            for (int i = 1; i < ShipModuleManager.Instance.shipModuleTypes.Count; i++)
            {
                foreach (ShipModule mod in ShipModuleManager.Instance.shipModules)
                {
                    if (data.shipModuleCrewWorking[mod.moduleID][mod.modulePartIndex])
                    {
                        ShipModuleManager.Instance.GetModuleTypeList(mod.moduleID).Find(x => x.modulePartIndex == mod.modulePartIndex).SetManned();
                    } 
                }
            }

            CrewManagement.Instance.UpdateCrew(grid);
            //UIHandler.Instance.Initialize();
            //UIHandler.Instance.UpdateUI();
            //UIHandler.Instance.UpdateText();

        }  
    }
}

[Serializable]
class ShipPresets
{
    public List<string> options = new List<string>();

    public ShipPresets()
    {
        for (int i = 0; i < GameObject.Find("DropdownShips").GetComponent<TMP_Dropdown>().options.Count; i++)
        {
            options.Add(GameObject.Find("DropdownShips").GetComponent<TMP_Dropdown>().options[i].text);
        }
    }
}

[Serializable]
class TileMapSave
{
    public int[][] array;
    public int minx, maxx;
    public int miny, maxy;
    public int sizex, sizey;

    public TileMapSave()
    {
        BoundsInt bounds;
        GameObject TilemapObj = GameObject.Find("Tilemap_Ship");
        bounds = TilemapObj.GetComponent<Tilemap>().cellBounds;
        minx = bounds.xMin;
        maxx = bounds.xMax;
        miny = bounds.yMin;
        maxy = bounds.yMax;
        sizex = bounds.size.x;
        sizey = bounds.size.y;
        
        Tilemap tilemap = TilemapObj.GetComponent<Tilemap>();
        
        array = new int[bounds.size.x][];
        for (int i = 0; i < bounds.size.x; i++)
        {
            array[i] = new int[bounds.size.y];
        }
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                AdvancedRuleTile tile = tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x+bounds.xMin,y + bounds.yMin, 0));
                if (tile == null)
                {
                    array[x][y] = 0;
                }
                else if (tile.name == "ConnectorAdv")
                {
                    array[x][y] = 1;
                }
                else if (tile.name == "CockpitAdv")
                {
                    array[x][y] = 2;
                }
                else if (tile.name == "HardpointAdv")
                {
                    array[x][y] = 3;
                }
                else if (tile.name == "ReactorAdv")
                {
                    array[x][y] = 4;
                }
                else if (tile.name == "EngineroomAdv")
                {
                    array[x][y] = 5;
                }
                else if (tile.name == "EngineAdv")
                {
                    array[x][y] = 6;
                }
                else if (tile.name == "StorageAdv")
                {
                    array[x][y] = 7;
                }
                else if (tile.name == "BedroomAdv")
                {
                    array[x][y] = 8;
                }
            }
        }

    }
}

[Serializable]
class CrewDataSave
{
    public int crewCockpit = CrewManagement.Instance.crewCockpit;
    public int crewHardpoints = CrewManagement.Instance.crewHardpoints;
    public int crewReactor = CrewManagement.Instance.crewReactor;
    public int crewEngineroom = CrewManagement.Instance.crewEngineroom;
    public int crewUsed = CrewManagement.Instance.crewUsed;
    public bool[][] shipModuleCrewWorking = new bool[ShipModuleManager.Instance.shipModuleTypes.Count+1][];

    public CrewDataSave()
    {
        for (int i = 1; i < ShipModuleManager.Instance.shipModuleTypes.Count; i++)
        {
            foreach (ShipModule mod in ShipModuleManager.Instance.shipModules)
            {
                shipModuleCrewWorking[mod.moduleID] = new bool[ShipModuleManager.Instance.GetModuleTypeList(mod.moduleID).Count];
                bool isManned = ShipModuleManager.Instance.GetModuleTypeList(mod.moduleID).Find(x => x.modulePartIndex == mod.modulePartIndex).isManned;
                shipModuleCrewWorking[mod.moduleID][mod.modulePartIndex] = isManned;
            }
        }
    }
}
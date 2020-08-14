using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using FloDa.Utils;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UIHandler : MonoBehaviour
{
    public GameObject txtCockpit;
    public GameObject txtHardpoints;
    public GameObject txtReactor;
    public GameObject txtEngineroom;
    public GameObject CrewUI, ShipUI, BuildUI;
    public GameObject crewIndicator;
    bool wereCreated = false;
    public TextMeshProUGUI tC, tH, tR, tE;

    public UIHandler() { }

    private void Start()
    {
        
    }

    public void Initialize()
    {
        Debug.Log("ButtonHandler Start()");
    }

    public void UpdateHP()
    {
        if (wereCreated)
        {        
            Destroy(GameObject.Find("Hp"));
        }
        
        Ship ship = ShipFramework.Instance.ships.Find(x => x.shipName == "Ship " + ShipFramework.Instance.shipIndex);

        GameObject hp = new GameObject("Hp");
        hp.transform.parent = GameObject.Find("ShipHPUI").transform;
        hp.transform.localPosition = new Vector3(0, 0);
        Image img = hp.AddComponent<Image>();
        RectTransform rt = hp.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(340, 60);
        img.color = Color.green;

        /*
        for (int i = 0; i < (int)width; i++)
        {
            GameObject hp = new GameObject("Hp" + i);
            hp.transform.parent = GameObject.Find("ShipHPUI").transform;
            hp.transform.localPosition = new Vector3(-160 + i * (width/2 + 3), 0);
            Image img = hp.AddComponent<Image>();
            RectTransform rt = hp.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(width/2, 60);
            img.color = Color.green;
        }
        */

        wereCreated = true;

    }

    public void UpdateUI()
    {
        
    }

    public void UpdateUI(int moduleID, bool isPlus)
    {

        /*
        if (isPlus)
        {
            int length = ShipModuleManager.Instance.GetModuleTypeList(moduleID).Count;

            Vector3[] positions = new Vector3[length];

            for (int i = 0; i < length; i++)
            {
                positions[i] = ShipModuleManager.Instance.GetModuleTypeList(moduleID).Find(x => x.modulePartIndex == i).modulePosition;      // noch unnötig oft abgefragt
            }

            int counter = 0;
            for (int i = 0; i < moduleID - 1; i++)
            {
                counter += ShipModuleManager.Instance.GetModuleTypeList(i + 1).Count;
            }

            Tilemap crewTileMap = GameObject.Find("Tilemap_Crew_" + ShipFramework.Instance.shipIndex).GetComponent<Tilemap>(); // noch unnötig oft abgefragt

            for (int i = 0; i < ShipModuleManager.Instance.shipModuleTypes.Find(x => x.moduleID == moduleID).crewWorking; i++)
            {
                int index = counter + i;
                TileBase crewTile = CrewManagement.Instance.tiles[index];

                Vector3Int vektor3int = new Vector3Int((int)positions[i].x, (int)positions[i].y, (int)positions[i].z);
                Vector2 vektor2 = Utils.GridArrayToWorldPosition(new Vector2(vektor3int.x, vektor3int.y));
                vektor3int.x = (int)vektor2.x;
                vektor3int.y = (int)vektor2.y;

                crewTileMap.SetTile(vektor3int, crewTile);
            }

        }
        else if (!isPlus)
        {
            int length = ShipModuleManager.Instance.GetModuleTypeList(moduleID).Count;

            Vector3[] positions = new Vector3[length];

            for (int i = 0; i < length; i++)
            {
                positions[i] = ShipModuleManager.Instance.GetModuleTypeList(moduleID).Find(x => x.modulePartIndex == i).modulePosition;      // noch unnötig oft abgefragt
            }

            int counter = 0;
            for (int i = 0; i < moduleID - 1; i++)
            {
                counter += ShipModuleManager.Instance.GetModuleTypeList(i + 1).Count;
            }

            Tilemap crewTileMap = GameObject.Find("Tilemap_Crew_" + ShipFramework.Instance.shipIndex).GetComponent<Tilemap>(); // noch unnötig oft abgefragt
            int t = 0;
            for (int i = CrewManagement.Instance.maxCrewWorking[moduleID]; i > ShipModuleManager.Instance.shipModuleTypes.Find(x => x.moduleID == moduleID).crewWorking; i--)
            {
                int index = counter + i;

                Vector3Int vektor3int = new Vector3Int((int)positions[i - 1].x, (int)positions[i - 1].y, 0);
                Vector2 vektor2 = Utils.GridArrayToWorldPosition(new Vector2(vektor3int.x, vektor3int.y));
                vektor3int.x = (int)vektor2.x;
                vektor3int.y = (int)vektor2.y;

                crewTileMap.SetTile(vektor3int, null);
                t += 1;
            }
        }
        */
    }
}


/*
        for (int i = 1; i < ShipModuleManager.Instance.shipModuleTypes.Count; i++)
        {
            for (int j = 0; j < ShipModuleManager.Instance.GetModuleTypeList(i).Count; j++)
            {
                ShipModule mod = ShipModuleManager.Instance.GetModuleTypeList(i).Find(x => x.modulePartIndex == j);

                Vector3 pos = Utils.GridArrayToWorldPosition(mod.modulePosition);

                Tilemap crewTileMap = GameObject.Find("Tilemap_Ship").GetComponent<Tilemap>();

                crewTileMap.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), null);
            }
        }

        for (int moduleID = 1; moduleID < ShipModuleManager.Instance.shipModuleTypes.Count; moduleID++)
        {

            int length = ShipModuleManager.Instance.GetModuleTypeList(moduleID).Count;

            Vector3[] positions = new Vector3[length];

            for (int i = 0; i < length; i++)
            {
                positions[i] = ShipModuleManager.Instance.GetModuleTypeList(moduleID).Find(x => x.modulePartIndex == i).modulePosition;      // noch unnötig oft abgefragt
            }

            int counter = 0;
            for (int i = 0; i < moduleID - 1; i++)
            {
                counter += ShipModuleManager.Instance.GetModuleTypeList(i + 1).Count;
            }

            Tilemap crewTileMap = GameObject.Find("Tilemap_Ship").GetComponent<Tilemap>();              // noch unnötig oft abgefragt

            for (int i = 0; i < ShipModuleManager.Instance.shipModuleTypes.Find(x => x.moduleID == moduleID).crewWorking; i++)
            {
                int index = counter + i;
                TileBase crewTile = CrewManagement.Instance.tiles[index];

                Vector3Int vektor3int = new Vector3Int((int)positions[i].x, (int)positions[i].y, (int)positions[i].z);
                Vector2 vektor2 = Utils.GridArrayToWorldPosition(new Vector2(vektor3int.x, vektor3int.y));
                vektor3int.x = (int)vektor2.x;
                vektor3int.y = (int)vektor2.y;

                crewTileMap.SetTile(vektor3int, crewTile);
            }
        }  

*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class ButtonHandler : MonoBehaviour
{
    public GameObject infoPanel;
    bool buildActive = false;
    bool infoActive = false;
    List<string> strList = new List<string>();
    
    private void Start()
    {
        infoPanel.SetActive(false);
    }

    public void GaragePanelClick()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        string resultString = Regex.Match(name, @"\d+").Value;
        int number = Int32.Parse(resultString);
        ShipFramework.Instance.shipIndex = number;
        GameObject.Find("ShipNameUI").GetComponentInChildren<TextMeshProUGUI>().text = EventSystem.current.currentSelectedGameObject.name;
        GameObject.Find("ShipData").GetComponent<ShipDataControl>().LoadCrewPreset();
    }

    public void ButtonInfoPanelClick()
    {
        if (infoActive)
        {
            infoPanel.SetActive(false);
            infoActive = false;
        }
        else
        {
            infoPanel.SetActive(true);
            infoActive = true;
        }
    }

    public void ButtonBuildClick()
    {
        if (buildActive)
        {
            GameObject.Find("BuildMode").GetComponent<Canvas>().enabled = false;
            GameObject.Find("InspectMode").GetComponent<Canvas>().enabled = true;
            buildActive = false;
        }
        else
        {
            GameObject.Find("BuildMode").GetComponent<Canvas>().enabled = true;
            GameObject.Find("InspectMode").GetComponent<Canvas>().enabled = false;
            buildActive = true;
        }
    }

    public void BuildFieldClick()
    {
        BuildFramework.Instance.SelectTile(EventSystem.current.currentSelectedGameObject.name);
    }

    public void BuildFrameClick()
    {
        BuildFramework.Instance.MouseInput();
    }

    public void CockpitPlusClick()
    {
        if ((CrewManagement.Instance.crewUsed < CrewManagement.Instance.crewMax) && (CrewManagement.Instance.crewCockpit < CrewManagement.Instance.maxCockpit) && (CrewManagement.Instance.crewMen.FindAll(x=>x.crewManModule == null).Count > 0))
        {
            CrewManagement.Instance.crewCockpit += 1;
            CrewManagement.Instance.crewUsed += 1;
            UIHandler.Instance.tC.text = CrewManagement.Instance.crewCockpit + "/" + CrewManagement.Instance.maxCockpit;
            ShipModule mod = ShipModuleManager.Instance.GetModuleTypeList(1).Find(x => x.isManned == false);
            CrewManagement.Instance.crewMen.Find(x => x.crewManModule == null).AssignModule(mod);
            mod.isManned = true;

            //ShipModuleManager.Instance.shipModuleTypes.Find(x => x.moduleID == 1).SetCrewAmount(CrewManagement.Instance.crewCockpit);
            UIHandler.Instance.UpdateUI();
            
        }
        else
        {
            Debug.Log("Limit erreicht!");
        }
    }

    public void CockpitMinusClick()
    {
        if (CrewManagement.Instance.crewCockpit > 0)
        {
            CrewManagement.Instance.crewCockpit -= 1;
            CrewManagement.Instance.crewUsed -= 1;
            UIHandler.Instance.tC.text = CrewManagement.Instance.crewCockpit + "/" + CrewManagement.Instance.maxCockpit;
            ShipModule mod = ShipModuleManager.Instance.GetModuleTypeList(1).Find(x => x.isManned == true);
            mod.isManned = false;
            CrewManagement.Instance.crewMen.Find(x => x.crewManModule == mod).AssignModule(null);
            //ShipModuleManager.Instance.shipModuleTypes.Find(x => x.moduleID == 1).SetCrewAmount(CrewManagement.Instance.crewCockpit);
            UIHandler.Instance.UpdateUI(1, false);
        }
        else
        {
            Debug.Log("Kein weiterer verteilt!");
        }
    }

    public void CrewSave()
    {
        ShipDataControl obj = GameObject.Find("ShipData").GetComponent<ShipDataControl>();
        obj.SaveCrewPreset();
    }

    public void CrewLoad()
    {
        ShipDataControl obj = GameObject.Find("ShipData").GetComponent<ShipDataControl>();
        obj.LoadCrewPreset();
    }

    public void AddCrewManClick()
    {
        System.Random random = new System.Random();
        int index1 = random.Next(CrewManagement.Instance.firstNames.Count);
        int index2 = random.Next(CrewManagement.Instance.surNames.Count);
        float skill = UnityEngine.Random.Range(0.00f, 0.80f);
        skill = (float)System.Math.Round(skill, 2);
        CrewMan man = new CrewMan(CrewManagement.Instance.firstNames[index1] + " " + CrewManagement.Instance.surNames[index2], CrewManagement.Instance.crewMen.Count, skill, UnityEngine.Random.Range(0,3));
        CrewManagement.Instance.AddCrewMan(man);
    }
}

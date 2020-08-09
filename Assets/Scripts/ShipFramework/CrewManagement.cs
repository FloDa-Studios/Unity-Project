using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using FloDa.Utils;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CrewMan
{
    public string crewManName;
    public int crewManID;
    public float crewManSkill;
    public ShipModule crewManModule;
    public string crewManJob;
    public string[] Jobs = { "Pilot", "Weapons Officer", "Engineer" };

    public CrewMan(string crewManName, int crewManID, float crewManSkill, int job)
    {
        this.crewManName = crewManName;
        this.crewManID = crewManID;
        if (crewManSkill < 1.0f && crewManSkill > 0.0f)
        {
            this.crewManSkill = crewManSkill;
        }
        else
        {
            this.crewManSkill = 0.0f;
        }
        this.crewManJob = Jobs[job];
    }

    public CrewMan(string crewManName, int crewManID, float crewManSkill, int job, ShipModule crewManModule)
    {
        this.crewManName = crewManName;
        this.crewManID = crewManID;
        if (crewManSkill < 1.0f && crewManSkill > 0.0f)
        {
            this.crewManSkill = crewManSkill;
        }
        else
        {
            this.crewManSkill = 0.0f;
        }
        this.crewManModule = crewManModule;
        this.crewManJob = Jobs[job];
    }

    public void AssignModule(ShipModule crewManModule)
    {
        this.crewManModule = crewManModule;
    }

    public override string ToString()
    {
        return "CrewMan ID: " + this.crewManID + "  |  Name: " + this.crewManName + "  |  Hash: " + GetHashCode();
    }

    public override int GetHashCode()
    {
        return this.crewManID;
    }

}


public class CrewManagement : ClassInstance<CrewManagement>
{
    bool ranOnce;

    public int crewMax;
    public int crewCount;
    public int crewCockpit, crewHardpoints, crewReactor, crewEngineroom, bedroomAmount, storageAmount;
    public int maxCockpit, maxHardpoints, maxReactor, maxEngineroom;
    public int crewUsed;

    public List<string> firstNames = new List<string> { "Oliver", "Jack", "Harry", "Jacob", "Charlie", "Thomas", "George", "Oscar", "James", "William" };
    public List<string> surNames = new List<string> { "Smith", "Jones", "Williams", "Taylor", "Davies", "Evans", "Thomas", "Johnson", "Roberts" };

    public float crewMoraleSkillPenalty;

    public int[] maxCrewWorking = new int[5];

    public List<CrewMan> crewMen;
    public List<TileBase> tiles;

    protected CrewManagement() { }

    public void Initialize()
    {
        Debug.Log("CrewManagement initialisert!");
    }

    public void Initialize(int crewMax, int crewCount, ShipGrid grid)
    {
        crewMen = new List<CrewMan>();
        tiles = new List<TileBase>();

        if (!ranOnce)
        {
            Debug.Log("CrewManagement initialisert!");
            UpdateCrew(grid);
            ranOnce = true;
        }


        LoadCrewMen();
    }

    public void UpdateCrew(ShipGrid grid)
    {
        SetModuleAmounts(grid);
        crewCount = crewMen.Count;
        crewMax = maxCockpit + maxHardpoints + maxReactor + maxEngineroom;

        if (crewMax > bedroomAmount * 4)
        {
            crewMoraleSkillPenalty = 0.1f;
        }

        maxCrewWorking[1] = maxCockpit;
        maxCrewWorking[2] = maxHardpoints;
        maxCrewWorking[3] = maxReactor;
        maxCrewWorking[4] = maxEngineroom;
    }

    public void AddCrewMan(CrewMan crewMan)
    {
        crewMen.Add(crewMan);
    }

    public void AddCrewMan(string name, int id, float skill, int job)
    {
        CrewMan genericCrewMan = new CrewMan(name, id, skill, job);
        crewMen.Add(genericCrewMan);
    }

    public void SetModuleAmounts(ShipGrid grid)
    {
        maxCockpit = Utils.FindAllIndexOf(grid.gridArray, 1).GetLength(0);
        maxHardpoints = Utils.FindAllIndexOf(grid.gridArray, 2).GetLength(0);
        maxReactor = Utils.FindAllIndexOf(grid.gridArray, 3).GetLength(0);
        maxEngineroom = Utils.FindAllIndexOf(grid.gridArray, 4).GetLength(0);
        storageAmount = Utils.FindAllIndexOf(grid.gridArray, 6).GetLength(0);
        bedroomAmount = Utils.FindAllIndexOf(grid.gridArray, 7).GetLength(0);
    }

    public void SaveCrewMan(CrewMan man)
    {
        SaveCrew crew = new SaveCrew();
        crew.SaveCrewMan(man);
    }

    public void LoadCrewMen()
    {
        if (File.Exists(Application.persistentDataPath + "/crewMen.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/crewMen.dat", FileMode.Open);
            SaveCrew data = (SaveCrew)bf.Deserialize(file);
            file.Close();

            int job;

            for (int i = 0; i < data.nameList.Count; i++)
            {
                if (data.jobList[i] == "Pilot")
                {
                    job = 0;
                }
                else if (data.jobList[i] == "Weapons Officer")
                {
                    job = 1;
                }
                else
                {
                    job = 2;
                }
                AddCrewMan(data.nameList[i], i, data.skillList[i], job);
            }

            Debug.Log("Crew loaded, Count: " + crewMen.Count);
        }
    }
}



[Serializable]
class SaveCrew
{
    public List<string> nameList;
    public List<float> skillList;
    public List<int> moduleIDList;
    public List<int> moduleIndexList;
    public List<string> jobList;

    public SaveCrew()
    {
        nameList = new List<string>();
        skillList = new List<float>();
        moduleIDList = new List<int>();
        moduleIndexList = new List<int>();
        jobList = new List<string>();
    }

    public void LoadLists()
    {
        if (File.Exists(Application.persistentDataPath + "/crewMen.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/crewMen.dat", FileMode.Open);
            SaveCrew data = (SaveCrew)bf.Deserialize(file);
            file.Close();

            nameList = data.nameList;
            skillList = data.skillList;
            moduleIDList = data.moduleIDList;
            moduleIndexList = data.moduleIndexList;
            jobList = data.jobList;
        }
    }
    public void SaveCrewMan(CrewMan man)
    {
        LoadLists();

        nameList.Add(man.crewManName);
        skillList.Add(man.crewManSkill);
        if (man.crewManModule == null)
        {
            moduleIDList.Add(0);
            moduleIndexList.Add(0);
        }
        else
        {
            moduleIDList.Add(man.crewManModule.moduleID);
            moduleIndexList.Add(man.crewManModule.modulePartIndex);
        }
        jobList.Add(man.crewManJob);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/crewMen.dat");

        SaveCrew data = this;

        bf.Serialize(file, data);
        file.Close();

    }
}

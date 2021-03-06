﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloDa.Utils;

public class ShipModule
{
    public string moduleName;
    public int moduleID;
    public int modulePartIndex;
    public Vector2 modulePosition;
    public bool isManned;

    public ShipModule(string moduleName, int moduleID, int modulePartIndex)
    {
        this.moduleName = moduleName;
        this.moduleID = moduleID;
        this.modulePartIndex = modulePartIndex;
        this.modulePosition.x = 0f;
        this.modulePosition.y = 0f;
        this.isManned = false;
    }

    public ShipModule(string moduleName, int moduleID, int modulePartIndex, Vector2 modulePosition)
    {
        this.moduleName = moduleName;
        this.moduleID = moduleID;
        this.modulePosition = modulePosition;
        this.modulePartIndex = modulePartIndex;
        this.isManned = false;
    }

    public void SetManned()
    {
        this.isManned = true;
    }

    public void SetUnmanned()
    {
        this.isManned = false;
    }

    public override string ToString()
    {
        return "ShipModule ID: " + this.moduleID + "  |  Name: " + this.moduleName + "  |  Hash: " + GetHashCode();
    }

    public override int GetHashCode()
    {
        return this.moduleID + this.modulePartIndex*10;
    }
}

public class ShipModuleManager : ClassInstance<ShipModuleManager>
{
    private ShipModuleManager shipModuleManager = null;

    public ShipModule genericModule;

    public List<ShipModule> shipModuleTypes = new List<ShipModule>();
    public List<ShipModule> shipModules = new List<ShipModule>();

    protected ShipModuleManager() {
        this.shipModuleTypes.Add(new ShipModule("Cockpit", 1, 0));
        this.shipModuleTypes.Add(new ShipModule("Hardpoint", 2, 0));
        this.shipModuleTypes.Add(new ShipModule("Reactor", 3, 0));
        this.shipModuleTypes.Add(new ShipModule("Engineroom", 4, 0));
        this.shipModuleTypes.Add(new ShipModule("Engine", 5, 0));
        this.shipModuleTypes.Add(new ShipModule("Storage", 6, 0));
        this.shipModuleTypes.Add(new ShipModule("Bedroom", 7, 0));
    }

    public void Initialize() {
        if (shipModuleManager == null) {
            this.shipModuleManager = new ShipModuleManager();
        }
    }

    public void SetAllModules(ShipGrid grid) {
        this.shipModules.Clear();
        foreach (ShipModule mod in shipModuleTypes)
        {
            int[,] positions = Utils.FindAllIndexOf(grid.gridArray, mod.moduleID);
            for (int i = 0; i < positions.GetLength(0); i++)
            {
                int x = positions[i, 0];
                int y = positions[i, 1];
                AddShipModule(mod.moduleName, mod.moduleID, i, new Vector2(x, y));
            }
        }
    }

    public void AddShipModule(ShipModule mod) {
        this.shipModules.Add(mod);
    }

    public void AddShipModule(string moduleName, int moduleID, int modulePartIndex) {
        this.genericModule = new ShipModule(moduleName, moduleID, modulePartIndex);
        this.shipModules.Add(genericModule);
    }

    public void AddShipModule(string moduleName, int moduleID, int modulePartIndex, Vector2 modulePosition) {
        this.genericModule = new ShipModule(moduleName, moduleID, modulePartIndex, modulePosition);
        this.shipModules.Add(genericModule);
    }

    public void SetCoordinates(int moduleID, Vector2 modulePositon) {
        this.genericModule = shipModules.Find(x => x.moduleID == moduleID);
        this.genericModule.modulePosition = modulePositon;
    }


    public List<ShipModule> GetModuleTypeList(int moduleID) {
        List<ShipModule> test = shipModules.FindAll(x => x.moduleID == moduleID);
        return test;
    }
}

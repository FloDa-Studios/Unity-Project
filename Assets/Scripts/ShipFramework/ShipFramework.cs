using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloDa.Utils;

public class Ship
{
    public string shipName;
    public int shipID;
    public int maxHP;
    public int HP;

    public Ship(string shipName, int shipID, int maxHP)
    {
        this.shipName = shipName;
        this.shipID = shipID;
        this.maxHP = maxHP;
    }

    public void SetHP(int hp)
    {
        this.HP = hp;
    }

    public void Repair()
    {
        this.HP = this.maxHP;
    }

    public void Damage(int value)
    {
        this.HP -= value;
    }

}

public class ShipFramework
{
    public int shipIndex;

    public List<Ship> ships = new List<Ship>();

    public static bool ranOnce;

    public ShipFramework()
    {
        if (!ranOnce)
        {
            shipIndex = 1;
            ships.Add(new Ship("Ship 0", 2, 100));
            ships.Add(new Ship("Ship 1", 1, 120));
            ships.Add(new Ship("Ship 2", 2, 150));
            ships.Add(new Ship("Ship 3", 2, 500));
            ranOnce = true;
            return;
        }
    }
    
}

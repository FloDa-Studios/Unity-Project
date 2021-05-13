using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FloDa.Utils;

public class Ship {

    private string shipName;
    private int shipID;
    private int maxHP;
    private int currentHP;

    public Ship(string shipName, int shipID, int maxHP) {
        this.shipName = shipName;
        this.shipID = shipID;
        this.maxHP = maxHP;
    }

    public void SetHP(int hp) {
        this.currentHP = hp;
    }

    public void Repair() {
        this.currentHP = this.maxHP;
    }

    public void Damage(int value) {
        this.currentHP -= value;
    }

    public String getShipName() {
        return this.shipName;
    }

    public int getShipID() {
        return this.shipID;
    }

    public int getMaxHP() {
        return this.maxHP;
    }

    public int getHP() {
        return this.currentHP;
    }

}

public class ShipFramework : ClassInstance<ShipFramework>
{
    private ShipFramework shipFramework = null;

    ///<summary>
    ///IMPORTANT The Index of the currently displayed and selected ship.
    ///</summary>
    public int shipIndex;

    public List<Ship> ships = new List<Ship>();

    protected ShipFramework() {

        this.shipIndex = 0;
        this.ships.Add(new Ship("Ship 0", 0, 100));
        this.ships.Add(new Ship("Ship 1", 1, 120));
        this.ships.Add(new Ship("Ship 2", 2, 150));
        this.ships.Add(new Ship("Ship 3", 3, 500));

    }

    public void Initialize() {
        if (shipFramework == null)
        {
            this.shipFramework = new ShipFramework();
        }
    }

    
}

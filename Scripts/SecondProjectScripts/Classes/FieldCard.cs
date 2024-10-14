using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public abstract class FieldCard : Card
{
    public FieldCard(int id, string name, Owner owner, string faction, Types? type, Sprite image, Sprite backImage, int power, List<Position> positions, OnActivation activation,  int effect, bool isLeader):
        base(id, name, owner, faction, type, image, backImage, power, positions, activation,  effect, isLeader)
        {
            for (int i = 0; i < 4; i++) powers[i] = power;
        }
    /*
    It is necessary to save the values of different power layers 
    powers[0]: holds de basepower value
    powers[1]: holds extra modifications resulting power (user-created effects)
    powers[2]: holds the boostaffected power
    powers[3]: holds the climate affected power
    */

    public int[] powers = new int[4];
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

//[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Object/UnitCard", order = 1)]

public abstract class Card : ScriptableObject
{
    public int id;
    public string name;
    public string effect;
    public string faction;
    public Sprite spriteImage;
    public bool IsActivated;
    public bool isLider;
    public Sprite cardBack;
    public bool isSpecial;
    public bool isUnit;
    public enum Owner 
    {
        Player, Opponent
    }
    public Owner owner;
    public bool IsSelected {get; set;} = false;
    public bool HasBeenMulligan  {get; set;} = false;

    public abstract string[] GetKind();
    public abstract int GetPower();
}

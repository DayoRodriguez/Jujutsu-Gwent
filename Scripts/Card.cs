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
    public bool IsActivated = false;
    public bool isLider;
    public Sprite cardBack;
    public bool isSpecial;
    public bool isUnit;

    public abstract string[] GetKind();
}

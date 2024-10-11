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
    public int effectN;
    public Types? type;
    public string faction;
    public Sprite spriteImage;
    public bool isActivated;
    public bool isLider;
    public Sprite cardBack;
    public bool isSpecial;
    public bool isUnit;
    public List<Position> positions;
    public Position position;
    public CardEffect cardEffects;
    public Owner? owner;
    public OnActivation activation;
    public bool IsSelected {get; set;} = false;
    public bool HasBeenMulligan  {get; set;} = false;


    public enum Owner
    {
        Player, Opponent
    }
    public enum CardEffect
    {
        ChangeAttack, SpecialSummon, Active, Destroy, Draw, TakeControl, Add
    }
    public enum Types
    {
        Golden, Silver, Weather, Increase, Leader, Dump  
    }

    public enum Position
    {
        Melee, Ranged, Siege, All, Weather, Increase, Dump
    }

    public Card(){}
    public Card(int id, string name, Owner owner, string faction, Types? type, Sprite image, Sprite backImage, int power, List<Position> positions, OnActivation activation,  int effect, bool isLeader)
        //: base(id, name, owner, faction, type, image, backImage, power, positions, activation,  effect, isLeader)
        {
        
        }


    public abstract string[] GetKind();
    public abstract int GetPower();

    public void SetAttack(int n)
    {
        if(isUnit && this is UnitCard)
        {
            UnitCard unitCard = (UnitCard)this;
            unitCard.power = n;
        }
    }
}

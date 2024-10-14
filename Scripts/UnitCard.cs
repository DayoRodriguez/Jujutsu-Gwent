using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit Card", menuName = "Unit Card", order = 1)]

public class UnitCard : Card
{
    public int power;
    
    public string kindAttack1;
    public string kindAttack2;
    public string kindAttack3;

    public static int Power{set; get;}

    public UnitCard()
    {

    }

    public UnitCard(int id, string name, Owner? owner, string faction, Types? type, Sprite spriteImage, Sprite backImage, int power, List<Position> positions, OnActivation activation, int effect, bool isLeader)
    {
        
        this.id = id; 
        this.name = name;
        this.owner = owner;
        this.faction = faction;
        this.type = type;
        this.spriteImage = spriteImage;
        cardBack = backImage;
        this.power = power;
        this.positions = positions;
        if(positions.Count >= 0 && positions[0] != null) kindAttack1 = positions[0].ToString();
        if(positions.Count >= 1 && positions[1] != null) kindAttack2 = positions[1].ToString();
        if(positions.Count == 2 && positions[2] != null) kindAttack3 = positions[2].ToString();
        this.activation = activation;
        this.effectN = effect;
        if(type == Types.Golden) isSpecial = true;
        this.isLider = isLeader;
    }

    public UnitCard(int id,string name, string effect, string faction, bool isSpecial, Sprite spriteImage, Sprite cardBack, int power, string kindAttack1, string kindAttack2, string kindAttack3, bool isLider)
    {
        this.name = name;
        this.effect = effect;
        this.faction = faction;
        this.isSpecial = isSpecial;
        this.spriteImage = spriteImage;
        this.power = power;
        this.kindAttack1 = kindAttack1;
        this.kindAttack2 = kindAttack2;
        this.kindAttack3 = kindAttack3;
        this.isLider = isLider;
        this.cardBack = cardBack;
        this.isUnit = true;
    }

    public override string[] GetKind()
    {
        string[] types = {kindAttack1, kindAttack2, kindAttack3};
        return types;
    }

    public override int GetPower()
    {
        return this.power;
    }

    public override void Initialize(int id, string name, Owner? owner, string faction, Types? type, Sprite image, Sprite backImage, int power, List<Position> positions, OnActivation activation, int effect, bool isLeader)
    {
        this.id = id; 
        this.name = name;
        this.owner = owner;
        this.faction = faction;
        this.type = type;
        this.spriteImage = spriteImage;
        cardBack = backImage;
        this.power = power;
        this.positions = positions;
        // if(positions.Count >= 0 && positions[0] != null) kindAttack1 = positions[0].ToString();
        // if(positions.Count >= 1 && positions[1] != null) kindAttack2 = positions[1].ToString();
        // if(positions.Count == 2 && positions[2] != null) kindAttack3 = positions[2].ToString();
        this.activation = activation;
        this.effectN = effect;
        if(type == Types.Golden) isSpecial = true;
        this.isLider = isLeader;   
    }
}

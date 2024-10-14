using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Especial Card",menuName = "Especial Card", order = 2)]

public class EspecialCard : Card
{
    public string types; 

    public EspecialCard()
    {

    }

    public EspecialCard(int id, string name, Owner? owner, Types? type, string faction,Sprite spriteImage, Sprite backImage, List<Position> positions, OnActivation activation, int effect)
    {
        this.id = id;
        this.name = name;
        types = type.ToString();
        this.faction = faction;
        this.type = type;
        this.owner = owner;
        this.positions = positions;
        this.activation = activation;
        effectN = effect;
        this.spriteImage = spriteImage;
        cardBack = backImage;
    }
    public EspecialCard(int id, string name, string effect, string type, Sprite spriteImage, Sprite cardBack, bool isSpecial)
    {
        this.id = id;
        this.name = name;
        this.effect = effect;
        this.types = type;
        this.spriteImage = spriteImage;
        this.cardBack = cardBack;
        this.isSpecial = isSpecial;
        this.faction = "Neutral";
        this.isLider = false;
        this.isUnit = false; 
    }


    public override string[] GetKind()
    {
        string[] kind = {types};
        return kind;
    }

    public override int GetPower()
    {
        return 0;
    }

    public override void Initialize(int id, string name, Owner? owner, string faction, Types? type, Sprite image, Sprite backImage, int power, List<Position> positions, OnActivation activation, int effect, bool isLeader)
    {
        this.id = id;
        this.name = name;
        types = type.ToString();
        this.faction = faction;
        this.type = type;
        this.owner = owner;
        this.positions = positions;
        this.activation = activation;
        effectN = effect;
        this.spriteImage = spriteImage;
        cardBack = backImage;
    }
}

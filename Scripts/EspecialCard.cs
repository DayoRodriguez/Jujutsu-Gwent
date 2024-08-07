using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Especial Card",menuName = "Especial Card", order = 2)]

public class EspecialCard : Card
{
    public string type; 

    public EspecialCard()
    {

    }

    public EspecialCard(int id, string name, string effect, string type, Sprite spriteImage, Sprite cardBack, bool isSpecial)
    {
        this.id = id;
        this.name = name;
        this.effect = effect;
        this.type = type;
        this.spriteImage = spriteImage;
        this.cardBack = cardBack;
        this.isSpecial = isSpecial;
        this.faction = "Neutral";
        this.isLider = false;
        this.isUnit = false; 
    }

    public override string[] GetKind()
    {
        string[] kind = {type};
        return kind;
    }

    public override int GetPower()
    {
        return 0;
    }
}

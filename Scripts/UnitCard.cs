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

    public UnitCard()
    {

    }

    public UnitCard(int id,string name, string effect, string faction, bool isSpecial, Sprite spriteImage, int power, string kindAttack1, string kindAttack2, string kindAttack3, bool isLider, Sprite cardBack)
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

    /*public UnitCard(int id, string name, string effect, string faction, string type, int power, string kindAttack1, string kindAttack2, string kindAttack3)
    {
        this.id = id;
        this.name = name;
        this.effect = effect;
        this.faction = faction;
        this.type = type;
        this.power = power;
        this.kindAttack1 = kindAttack1;
        this.kindAttack2 = kindAttack2;
        this.kindAttack3 = kindAttack3;
    }*/
}

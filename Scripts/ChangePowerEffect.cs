using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChangePowerEffect : MonoBehaviour , ICardEffect
{
    private GameObject activeCard;
    private BoardManager board;
    private Transform selectedRow;
    private GameObject selectedCard;

    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        Initialize();
        //cardEffect = FindObjectOfType<CardEffects>();

        //activingCard = cardEffect.activingCard;

        StartCoroutine(HandleEffectExecution(activingC, activingCard));
    }

    private IEnumerator HandleEffectExecution(DisplayCard activingC, GameObject activingCard)
    {
        bool needRowSelection = cardEffect.NeedsRowSelection();
        bool needCardSelection = cardEffect.NeedsCardSelection();

        if(needRowSelection)
        {
            bool rowSelected = false;
            board.SelectionRowIfNeeded(row =>
            {
                selectedRow = row;
                rowSelected = true;
            });
            while(!rowSelected)
            {
                yield return null;
            }
        }

        if(needCardSelection)
        {
            bool cardSelected = false;
            board.SelectionCardIfNeeded(card => 
            {
                selectedCard = card;
                cardSelected = true;
            });
            while(!cardSelected)
            {
                yield return null;
            }
        }

        switch(activingC.card.effect)
        {
            case "ReduceByFive" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ModifyRowByPower(selectedRow, activingCard, (c, actPower) => c.card.SetAttack(c.card.GetPower() - 5), 
                "La Seleccionada no es valida, Vuelva a intentarlo");
                break;
            case "ChangeToLower" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ApplyToRow(selectedRow, ChangeToLower);
                break;
            case "Absorption" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                Absorption(activingCard);
                break;        
            case "ReducerCourt" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ReducerCourt(selectedCard);
                break;
            case "ChangeToAverage" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ApplyToAllRows(ChangeToAverage);
                break;
            case "SoulMutation" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                SoulMutation(selectedCard, UnityEngine.Random.Range(0,21));
                break;            
            case "BloodManipulation" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                BloodManipulation(activingCard);
                break;
            case "Vudu" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                Vudu(activingCard, selectedCard);
                break;
            case "IncreaseAttackByTwo" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ModifyRowByPower(selectedRow, activingCard, (c, actPower) => c.card.SetAttack(c.card.GetPower() + 2), 
                "La fila seleccionada no es valida, Vuelva a intentarlo");
                break;            
            case "TempleEvil" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                TempleEvil(activingCard);
                break;    
            default :
                selectedRow = null; 
                selectedCard = null;
                break;
        }
    }

    public void Initialize()
    {
       board = FindObjectOfType<BoardManager>();
    }

    public void ShowMessagePanel(string sms)
    {
        //Implementacion para mostrar el panel
        Debug.Log(sms);
    }

    public bool CanActive()
    {
        //logica para evaluar si se puede activar el efecto 
        return true; //Placeholder
    }

    public void EndEffect(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(!activingC.card.isUnit && (activingC.card.GetKind()[0] == "Increase"))
        {
            Transform grav = activingC.card.owner == Card.Owner.Player ? board.transformGraveyard : board.opponentTransformGraveyard;
            board.CleanRow(activingCard.transform.parent, grav);
        }
    }

    public bool IsValidRow(Transform row)
    {
        return activeCard.GetComponent<DisplayCard>().card.owner == Card.Owner.Opponent
        ? row != board.transformMeleeRow && row != board.transformRangedRow && row != board.transformSeigeRow
        : row != board.opponentTransformMeleeRow && row != board.opponentTransformRangedRow && row != board.opponentTransformSeigeRow;
    }

    //Jogo's effect
    public void ReduceByFive(GameObject activingCard, Transform row)
    {
        ModifyRowByPower(row, activingCard, (c, actPower) => c.card.SetAttack(c.card.GetPower() - 5), "La seleccion no es valida vuelva a intentarlo");
    }

    //AutoEncarnacion de la Perfeccion's effect
    public void ChangeToLower(DisplayCard[] cards)
    {
        int lessAttack = int.MaxValue;
        foreach( DisplayCard c in cards)
        {
            lessAttack = Math.Min(lessAttack, c.card.GetPower());
        }
        foreach(DisplayCard c in cards)
        {
            c.card.SetAttack(lessAttack);
        }
    }

    //Hanami's Effect
    public IEnumerator Absorption(GameObject activingCard)
    {
        yield return board.WaitForSelection<GameObject>
        (
            seletedCard => {

                DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
                DisplayCard targedC = selectedCard.GetComponent<DisplayCard>();

                if(targedC != null && activingC != null && targedC.card.isUnit && activingC.card.isUnit)
                {
                    int absorpbedAttack = targedC.card.GetPower();
                    targedC.card.SetAttack(0);
                    activingC.card.SetAttack(activingC.card.GetPower() + absorpbedAttack);
                }
                else
                {
                    ShowMessagePanel("La carta seleccionada no es valida");
                } 
                
            },
            () => board.effectSelectedCard == null
        );
    }

    //Nanami's Effect
    public void ReducerCourt(GameObject targedCard)
    {
        int reduction = UnityEngine.Random.Range(1,8);
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        targedC.card.SetAttack(targedC.card.GetPower() - reduction);
        if(targedC.card.GetPower() <= 5)
        {
            if(targedC.card.owner == Card.Owner.Player)
            {
                targedCard.transform.SetParent(board.transformGraveyard);
                targedCard.transform.localPosition = Vector3.zero;
                targedCard.transform.localRotation = Quaternion.identity;
                targedCard.transform.localScale = Vector3.one;
            }
            else
            {
                targedCard.transform.SetParent(board.opponentTransformGraveyard);
                targedCard.transform.localPosition = Vector3.zero;
                targedCard.transform.localRotation = Quaternion.identity;
                targedCard.transform.localScale = Vector3.one;

            }
        }
    }

    //Mecamaru's effect
    public void ChangeToAverage(DisplayCard[] cards)
    {
        int totalPower = 0;
        foreach (var c in cards)
        {
            totalPower += c.card.GetPower();
        }

        int averagePower = cards.Length > 0 ? totalPower / cards.Length : 0;

        foreach (var c in cards)
        {
            c.card.SetAttack(averagePower);
        }
    }

    //Mahito's Effect
    public void SoulMutation(GameObject targedCard, int attackChange)
    {
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        if(targedC != null && 0 <= attackChange && attackChange <= 20)
        {
            targedC.card.SetAttack(attackChange);
        }
    }

    //Manipulacion de Sangre's Effect
    public void BloodManipulation(GameObject activingCard)
    {
        DisplayCard[] cardsInMelee;
        DisplayCard[] cardsInRanged;
        DisplayCard[] cardsInSeige;
        int averageMelee = 0;
        int averageRanged = 0;
        int averageSeige = 0;
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        if(activingC.card.owner == Card.Owner.Player)
        {
            cardsInMelee = board.transformMeleeRow.GetComponentsInChildren<DisplayCard>();
            cardsInRanged = board.transformRangedRow.GetComponentsInChildren<DisplayCard>();
            cardsInSeige = board.transformSeigeRow.GetComponentsInChildren<DisplayCard>();
            averageMelee = CalculateAverage(cardsInMelee);
            averageRanged = CalculateAverage(cardsInRanged);
            averageMelee = CalculateAverage(cardsInSeige);
            if(averageMelee <= averageRanged && averageMelee <= averageSeige)
            {
                foreach(DisplayCard c in cardsInMelee)
                {
                    c.card.SetAttack(c.card.GetPower() + 2);
                }
            }
            else if(averageRanged <= averageMelee && averageRanged <= averageSeige)
            {
                foreach(DisplayCard c in cardsInRanged)
                {
                    c.card.SetAttack(c.card.GetPower() + 2);
                }
            }
            else 
            {
                foreach(DisplayCard c in cardsInSeige)
                {
                    c.card.SetAttack(c.card.GetPower() + 2);
                }
            }
        }
        else
        {
            cardsInMelee = board.opponentTransformMeleeRow.GetComponentsInChildren<DisplayCard>();
            cardsInRanged = board.opponentTransformRangedRow.GetComponentsInChildren<DisplayCard>();
            cardsInSeige = board.opponentTransformSeigeRow.GetComponentsInChildren<DisplayCard>();
            averageMelee = CalculateAverage(cardsInMelee);
            averageRanged = CalculateAverage(cardsInRanged);
            averageMelee = CalculateAverage(cardsInSeige);
            if(averageMelee <= averageRanged && averageMelee <= averageSeige)
            {
                foreach(DisplayCard c in cardsInMelee)
                {
                    c.card.SetAttack(c.card.GetPower() + 2);
                }
            }
            else if(averageRanged <= averageMelee && averageRanged <= averageSeige)
            {
                foreach(DisplayCard c in cardsInRanged)
                {
                    c.card.SetAttack(c.card.GetPower() + 2);
                }
            }
            else 
            {
                foreach(DisplayCard c in cardsInSeige)
                {
                    c.card.SetAttack(c.card.GetPower() + 2);
                }
            }
        }
    }

    //Nobara's effect
    public void Vudu(GameObject activingCard, GameObject targedCard)
    {
        if((activingCard.transform.parent == board.transformRangedRow && targedCard.transform.parent == board.opponentTransformRangedRow)
            || (activingCard.transform.parent == board.transformSeigeRow && targedCard.transform.parent == board.opponentTransformSeigeRow))
            {
                int howManyActivingCard = HowManyThereAre(board.transformRangedRow, activingCard) + HowManyThereAre(board.transformSeigeRow, activingCard);
                int howManyTargedCard = HowManyThereAre(board.opponentTransformRangedRow, targedCard) + HowManyThereAre(board.opponentTransformSeigeRow, targedCard);
                DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
                targedC.card.SetAttack(targedC.card.GetPower()/(howManyActivingCard + howManyTargedCard));
            }
        else if((activingCard.transform.parent == board.opponentTransformRangedRow && targedCard.transform.parent == board.transformRangedRow)
            || (activingCard.transform.parent == board.opponentTransformSeigeRow && targedCard.transform.parent == board.transformSeigeRow))
            {
                int howManyActivingCard = HowManyThereAre(board.opponentTransformRangedRow, activingCard) + HowManyThereAre(board.opponentTransformSeigeRow, activingCard);
                int howManyTargedCard = HowManyThereAre(board.transformRangedRow, targedCard) + HowManyThereAre(board.transformSeigeRow, targedCard);
                DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
                targedC.card.SetAttack(targedC.card.GetPower()/(howManyActivingCard + howManyTargedCard));
            }    
    }

    //Ryomen Sukuna's effect
    public void DivideAllEnemysAttacks(GameObject ActivingCard)
    {
        DisplayCard activingC = ActivingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            DivideAttack(board.opponentTransformMeleeRow, 2);
            DivideAttack(board.opponentTransformRangedRow, 2);
            DivideAttack(board.opponentTransformSeigeRow, 2);
        }
        else
        {
            DivideAttack(board.transformMeleeRow, 2);
            DivideAttack(board.transformRangedRow, 2);
            DivideAttack(board.transformSeigeRow, 2);   
        }
    }

    //Shoko Ieiri's effect
    public void IncreaseAttackByTwo(Transform Row)
    {
        DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in cards)
        {
            c.card.SetAttack(c.card.GetPower() + 2);
        }
    }

    //Temple of Evil's effect
    public void TempleEvil(GameObject ActivingCard)
    {
        DisplayCard liderCard;
        DisplayCard activingC = ActivingCard.GetComponent<DisplayCard>();
        if(activingC.card.owner == Card.Owner.Player)
        {
            liderCard = board.transformLeaderCardSlot.GetChild(0).GetComponent<DisplayCard>();
        }  
        else
        {
            liderCard = board.opponentTransformLeaderCardSlot.GetChild(0).GetComponent<DisplayCard>();
        }

        if(liderCard.card.name != "Ryomen Sukuna")
        {
            int meleeSum = CalculateSum(board.transformMeleeRow);
            int rangedSum = CalculateSum(board.transformRangedRow);
            int seigeSum = CalculateSum(board.transformSeigeRow);

            int oppMeleeSum = CalculateSum(board.opponentTransformMeleeRow);
            int oppRangedSum = CalculateSum(board.opponentTransformRangedRow);
            int oppSeigeSum = CalculateSum(board.opponentTransformSeigeRow);

            DisplayCard[] greatSumRow;
            DisplayCard[] greatOppSumRow;

            Transform greatRow;
            Transform greatOppRow;

            if(meleeSum >= rangedSum && meleeSum >= seigeSum)
            {
                greatSumRow = board.transformMeleeRow.GetComponents<DisplayCard>();
                greatRow = board.transformMeleeRow;
            }  
            else if(rangedSum >= meleeSum && rangedSum >= seigeSum)
            {
                greatSumRow = board.transformRangedRow.GetComponents<DisplayCard>();
                greatRow = board.transformRangedRow;
            }
            else
            {
                greatSumRow = board.transformSeigeRow.GetComponents<DisplayCard>();
                greatRow = board.transformSeigeRow;
            }
            //-------------
            if(oppMeleeSum >= oppRangedSum && oppMeleeSum >= oppSeigeSum)
            {
                greatOppSumRow = board.opponentTransformMeleeRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformMeleeRow;
            }  
            else if(oppRangedSum >= oppMeleeSum && oppRangedSum >= oppSeigeSum)
            {
                greatOppSumRow = board.opponentTransformRangedRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformRangedRow;
            }
            else
            {
                greatOppSumRow = board.opponentTransformSeigeRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformSeigeRow;
            }
            //-------
            if(greatSumRow.Length != greatOppSumRow.Length)
            {
                DivideAttack(greatRow,Math.Abs(greatSumRow.Length - greatOppSumRow.Length));
                DivideAttack(greatOppRow,Math.Abs(greatSumRow.Length - greatOppSumRow.Length));
            }
            else
            {
                DivideAttack(greatRow, 1);
                DivideAttack(greatOppRow, 1);
            }
        }
        else
        {
            
            int meleeSum = CalculateSum(board.transformMeleeRow);
            int rangedSum = CalculateSum(board.transformRangedRow);
            int seigeSum = CalculateSum(board.transformSeigeRow);

            int oppMeleeSum = CalculateSum(board.opponentTransformMeleeRow);
            int oppRangedSum = CalculateSum(board.opponentTransformRangedRow);
            int oppSeigeSum = CalculateSum(board.opponentTransformSeigeRow);

            DisplayCard[] greatSumRow;
            DisplayCard[] greatOppSumRow;

            Transform greatRow;
            Transform greatOppRow;

            if(meleeSum >= rangedSum && meleeSum >= seigeSum)
            {
                greatSumRow = board.transformMeleeRow.GetComponents<DisplayCard>();
                greatRow = board.transformMeleeRow;
            }  
            else if(rangedSum >= meleeSum && rangedSum >= seigeSum)
            {
                greatSumRow = board.transformRangedRow.GetComponents<DisplayCard>();
                greatRow = board.transformRangedRow;
            }
            else
            {
                greatSumRow = board.transformSeigeRow.GetComponents<DisplayCard>();
                greatRow = board.transformSeigeRow;
            }
            //-------------
            if(oppMeleeSum >= oppRangedSum && oppMeleeSum >= oppSeigeSum)
            {
                greatOppSumRow = board.opponentTransformMeleeRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformMeleeRow;
            }  
            else if(oppRangedSum >= oppMeleeSum && oppRangedSum >= oppSeigeSum)
            {
                greatOppSumRow = board.opponentTransformRangedRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformRangedRow;
            }
            else
            {
                greatOppSumRow = board.opponentTransformSeigeRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformSeigeRow;
            }
            //------------    
            if(activingC.card.owner == Card.Owner.Player)
            {
                if(greatSumRow.Length != greatOppSumRow.Length)
                {
                    DivideAttack(greatOppRow,Math.Abs(greatSumRow.Length - greatOppSumRow.Length));
                }
                else
                {
                    DivideAttack(greatOppRow, 1);
                }
            }
            else
            {
                if(greatSumRow.Length != greatOppSumRow.Length)
                {
                    DivideAttack(greatRow,Math.Abs(greatSumRow.Length - greatOppSumRow.Length));
                }
                else
                {
                    DivideAttack(greatRow, 1);
                }
            }
        }
    }

    //======Metodos Basicos para utilizar en los effectos=============================
    private void DivideAttack(Transform Row, int count)
    {
        DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in cards)
        {
            c.card.SetAttack(c.card.GetPower()/count);
        }
    }
    private int HowManyThereAre(Transform Row, GameObject Card)
    {
        DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();
        DisplayCard cardToCount = Card.GetComponent<DisplayCard>();
        int howManyCard = 0;

        foreach(DisplayCard c in cards)
        {
            if(c.card.name.Equals(cardToCount.card.name))
            {
                howManyCard +=1;
            }
        }
        return howManyCard;
    }
    private int CalculateAverage(DisplayCard[] cards)
    {
        int average = 0;
        foreach(DisplayCard c in cards)
        {
            average += c.card.GetPower();
        }
        return average/cards.Length;
    }

    private int CalculateSum(Transform row)
    {
        if(row == board.transformMeleeRow || row == board.transformRangedRow || row == board.transformSeigeRow
            || row == board.opponentTransformMeleeRow || row == board.opponentTransformRangedRow || row == board.opponentTransformSeigeRow)
            {
                int sum = 0;
                DisplayCard[] cards = row.GetComponentsInChildren<DisplayCard>();
                foreach(DisplayCard c in cards)
                {
                    sum += c.card.GetPower();
                }
                return sum;  
            }
        else return -1;
    }

    private void ApplyToRow(Transform row, Action<DisplayCard[]> effectAction)
    {
        DisplayCard[] cardsInRow = row.GetComponentsInChildren<DisplayCard>();
        effectAction.Invoke(cardsInRow);
    }

    private void ApplyToAllRows(Action<DisplayCard[]> effectAction)
    {
        DisplayCard activingC = activeCard.GetComponent<DisplayCard>();

        Transform[] allRows = activingC.card.owner == Card.Owner.Opponent 
        ? new[]{board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow} 
        : new[]{board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow};

        foreach(var row in allRows)
        {
            ApplyToRow(row, effectAction);
        }
    }

    private void ModifyRowByPower(Transform row, GameObject activingCard, Action<DisplayCard, int> effect, string errorSms)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        int actPower = activingC.card.GetPower();

        if(IsValidRow(row))
        {
            DisplayCard[] cards = row.GetComponentsInChildren<DisplayCard>();
            foreach(DisplayCard c in cards)
            {
                effect.Invoke(c, actPower);
            }
        }
        else 
        {
            ShowMessagePanel(errorSms);
        }
    }
}

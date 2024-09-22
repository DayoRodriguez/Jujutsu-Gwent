using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChangePowerEffect : MonoBehaviour , ICardEffect
{
    public GameObject cardSelectionPanel;  // Panel que contendrá las cartas
    public GameObject cardButtonPrefab;    // Prefab de un botón para cada carta
    public Transform cardListContainer;
    public GameObject cardPrefabs;

    private GameObject activeCard;
    private BoardManager board;
    public Transform selectedRow;
    //private GameObject selectedCard;

    //private CardEffects cardEffect;

    void Start()
    {
        board = FindObjectOfType<BoardManager>();
       
    } 
    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        switch(activingC.card.effect)
        {
            case "ReduceByFive" :
                ShowRowsToSelectd(activingC);
                break;
            case "ChangeToLower" :
                ShowRowsToSelectd(activingC);
                break;
            case "Absorption" :
                List<DisplayCard> cardsToAbs = GetCardsTo(activingC);
                if(cardsToAbs.Count != 0) ShowCardsToChange(cardsToAbs, activingC);
                break;        
            case "ReducerCourt" :
                List<DisplayCard> cardsToCurt = GetCardsTo(activingC);
                if(cardsToCurt.Count != 0) ShowCardsToChange(cardsToCurt, activingC);
                break;
            case "ChangeToAverage" :
                List<DisplayCard> cardsToAverage = new List<DisplayCard>();
                Transform[] field = {board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow
                                    ,board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow};
                foreach(Transform t in field)
                {
                    DisplayCard[] auxCards = t.GetComponentsInChildren<DisplayCard>();
                    if(auxCards.Length != 0)
                    {
                        for(int i = 0; i < auxCards.Length; i++)
                        {
                            cardsToAverage.Add(auxCards[i]);
                        }
                    }
                }
                if(cardsToAverage.Count != 0) ShowCardsToChange(cardsToAverage, activingC);
                break;
            case "SoulMutation" :
                List<DisplayCard> cardsToMute = GetCardsTo(activingC);
                if(cardsToMute.Count != 0) ShowCardsToChange(cardsToMute, activingC);
                break;            
            case "BloodManipulation" :
                BloodManipulation(activingCard);
                EndEffect(activingCard);
                break;
            case "Vudu" : //Comprobar
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                List<DisplayCard> cardsToVudu = GetCardsTo(activingC);
                if(cardsToVudu.Count != 0) ShowCardsToChange(cardsToVudu, activingC);
                break;
            case "IncreaseAttackByTwo" :
                ShowRowsToSelectd(activingC);
                IncreaseAttackByTwo(selectedRow);
                break;            
            case "TempleEvil" :
                TempleEvil(activingCard);
                break;    
            case "DivideAllEnemysAttacks" :
                DivideAllEnemysAttacks(activingCard);
                break;     
            default :
                selectedRow = null; 
                break;
        }
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

    //Jogo's effect
    public void ReduceByFive(GameObject activingCard, Transform row)
    {
        DisplayCard[] cardsToReduce = row.GetComponentsInChildren<DisplayCard>();
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(cardsToReduce.Length != 0)
        {
            foreach(DisplayCard c in cardsToReduce)
            {
                if(c.card.GetPower() <= activingC.card.GetPower())
                {
                    c.card.SetAttack(c.card.GetPower() - 5);
                    if(c.card.GetPower() <= 0)
                    {
                        Transform aux = c.card.owner == Card.Owner.Player ? board.transformGraveyard : board.opponentTransformGraveyard;
                        c.gameObject.transform.SetParent(aux);
                        c.gameObject.transform.localScale = Vector3.one;
                        c.gameObject.transform.localPosition = Vector3.zero;
                        c.gameObject.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }
    }

    //AutoEncarnacion de la Perfeccion's effect
    public void ChangeToLower(Transform t)
    {
        DisplayCard[] cards = t.GetComponentsInChildren<DisplayCard>();
        if(cards.Length != 0)
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
    }

    //Hanami's Effect
    public void Absorption(GameObject activingCard, GameObject selectedCard)
    {
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
           Debug.Log("La carta seleccionada no es valida");
        } 
    }

    //Nanami's Effect
    public void ReducerCourt(GameObject targedCard)
    {
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        int reduction = UnityEngine.Random.Range(1,8);
        int newAttack = targedC.card.GetPower() - reduction;
        targedC.card.SetAttack(newAttack);

        if(newAttack <= 5)
        {
            if(targedC.card.owner == Card.Owner.Player)
            {
                targedCard.transform.SetParent(board.transformGraveyard);
            }
            else
            {
                targedCard.transform.SetParent(board.opponentTransformGraveyard);
            }
            targedCard.transform.localPosition = Vector3.zero;
            targedCard.transform.localRotation = Quaternion.identity;
            targedCard.transform.localScale = Vector3.one;
        }
    }

    //Mecamaru's effect
    public void ChangeToAverage(DisplayCard activinC, DisplayCard targedC)
    {
        if(activinC.card.owner == Card.Owner.Player)
        {
            Transform[] rivalRows = GetRows(false);
            int average = 0;
            int count = 0;
            foreach(Transform t in rivalRows)
            {
                foreach(DisplayCard c in GetCards(t, true))
                {
                    average += c.card.GetPower();
                    count++;
                }
            }
            if(count != 0) targedC.card.SetAttack(average/count);
        }
        else 
        {
            Transform[] rivalRows = GetRows(true);
            int average = 0;
            int count = 0;
            foreach(Transform t in rivalRows)
            {
                foreach(DisplayCard c in GetCards(t, true))
                {
                    average += c.card.GetPower();
                    count++;
                }
            }
            if(count != 0) targedC.card.SetAttack(average/count);
        }
    }

    //Mahito's Effect
    public void SoulMutation(GameObject targedCard)
    {
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();

        int newAttack = UnityEngine.Random.Range(0, targedC.card.GetPower());
        if(targedC != null)
        {
            targedC.card.SetAttack(newAttack);
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
        int howManyActivingCard = HowManyThereAre(board.transformMeleeRow, activingCard) + HowManyThereAre(board.transformRangedRow, activingCard) + HowManyThereAre(board.transformSeigeRow, activingCard)
                                + HowManyThereAre(board.opponentTransformMeleeRow, activingCard) + HowManyThereAre(board.opponentTransformRangedRow, activingCard) + HowManyThereAre(board.opponentTransformSeigeRow, activingCard);

        int howManyTargedCard = HowManyThereAre(board.opponentTransformMeleeRow, targedCard) + HowManyThereAre(board.opponentTransformRangedRow, targedCard) + HowManyThereAre(board.opponentTransformSeigeRow, targedCard)
                            + HowManyThereAre(board.transformMeleeRow, targedCard) + HowManyThereAre(board.transformRangedRow, targedCard) + HowManyThereAre(board.transformSeigeRow, targedCard);

        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        targedC.card.SetAttack(targedC.card.GetPower()/(howManyActivingCard + howManyTargedCard)); 
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
    public void IncreaseAttackByTwo(Transform row)
    {
        DisplayCard[] cards = row.GetComponentsInChildren<DisplayCard>();
        if(cards.Length != 0)
        {
            foreach(DisplayCard c in cards)
            {
                int attackToIncrease = c.card.GetPower();
                c.card.SetAttack(attackToIncrease + 2);
            }
        }
        SeeAttack(row);
    }

    //Temple of Evil's effect
    public void TempleEvil(GameObject ActivingCard)
    {
        DisplayCard liderCard;
        DisplayCard activingC = ActivingCard.GetComponent<DisplayCard>();
        if(activingC.card.owner == Card.Owner.Player)
        {
            liderCard = board.transformLeaderCardSlot.GetComponentInChildren<DisplayCard>();
        }  
        else
        {
            liderCard = board.opponentTransformLeaderCardSlot.GetComponentInChildren<DisplayCard>();
        }

        if(liderCard.card.name != "Ryomen Sukuna")
        {
            int meleeSum = CalculateSum(board.transformMeleeRow);
            int rangedSum = CalculateSum(board.transformRangedRow);
            int seigeSum = CalculateSum(board.transformSeigeRow);

            int oppMeleeSum = CalculateSum(board.opponentTransformMeleeRow);
            int oppRangedSum = CalculateSum(board.opponentTransformRangedRow);
            int oppSeigeSum = CalculateSum(board.opponentTransformSeigeRow);

            int maxRowPlayer = Math.Max(meleeSum, Math.Max(rangedSum, seigeSum));
            int maxRowOpp = Math.Max(oppMeleeSum, Math.Max(oppRangedSum, oppSeigeSum));

            DisplayCard[] greatSumRow;
            DisplayCard[] greatOppSumRow;

            Transform greatRow;
            Transform greatOppRow;

            if(meleeSum == maxRowPlayer)
            {
                greatSumRow = board.transformMeleeRow.GetComponentsInChildren<DisplayCard>();
                greatRow = board.transformMeleeRow;
            }  
            else if(rangedSum == maxRowPlayer)
            {
                greatSumRow = board.transformRangedRow.GetComponentsInChildren<DisplayCard>();
                greatRow = board.transformRangedRow;
            }
            else
            {
                greatSumRow = board.transformSeigeRow.GetComponentsInChildren<DisplayCard>();
                greatRow = board.transformSeigeRow;
            }
            //-------------
            if(oppMeleeSum == maxRowOpp)
            {
                greatOppSumRow = board.opponentTransformMeleeRow.GetComponentsInChildren<DisplayCard>();
                greatOppRow = board.opponentTransformMeleeRow;
            }  
            else if(oppRangedSum == maxRowOpp)
            {
                greatOppSumRow = board.opponentTransformRangedRow.GetComponentsInChildren<DisplayCard>();
                greatOppRow = board.opponentTransformRangedRow;
            }
            else
            {
                greatOppSumRow = board.opponentTransformSeigeRow.GetComponentsInChildren<DisplayCard>();
                greatOppRow = board.opponentTransformSeigeRow;
            }
            //-------
            if(greatSumRow.Length != greatOppSumRow.Length)
            {
                DivideAttack(greatRow,Math.Max(greatSumRow.Length, greatOppSumRow.Length));
                DivideAttack(greatOppRow,Math.Max(greatSumRow.Length, greatOppSumRow.Length));
            }
            else
            {
                if(greatSumRow.Length != 0)
                {
                    DivideAttack(greatRow, greatSumRow.Length);
                    DivideAttack(greatOppRow, greatSumRow.Length);
                } 
                else 
                {
                    DivideAttack(greatRow, 1);
                    DivideAttack(greatOppRow, 1);
                }    
            }
        }
        else
        {
            
            int meleeSum = CalculateSum(board.transformMeleeRow);
            Debug.Log(meleeSum);
            int rangedSum = CalculateSum(board.transformRangedRow);
            int seigeSum = CalculateSum(board.transformSeigeRow);

            int oppMeleeSum = CalculateSum(board.opponentTransformMeleeRow);
            int oppRangedSum = CalculateSum(board.opponentTransformRangedRow);
            int oppSeigeSum = CalculateSum(board.opponentTransformSeigeRow);

            int maxRowPlayer = Math.Max(meleeSum, Math.Max(rangedSum, seigeSum));
            int maxRowOpp = Math.Max(oppMeleeSum, Math.Max(oppRangedSum, oppSeigeSum));

            DisplayCard[] greatSumRow;
            DisplayCard[] greatOppSumRow;

            Transform greatRow;
            Transform greatOppRow;

            if(meleeSum == maxRowPlayer)
            {
                greatSumRow = board.transformMeleeRow.GetComponentsInChildren<DisplayCard>();
                greatRow = board.transformMeleeRow;
            }  
            else if(rangedSum == maxRowPlayer)
            {
                greatSumRow = board.transformRangedRow.GetComponentsInChildren<DisplayCard>();
                greatRow = board.transformRangedRow;
            }
            else
            {
                greatSumRow = board.transformSeigeRow.GetComponentsInChildren<DisplayCard>();
                greatRow = board.transformSeigeRow;
            }
            //-------------
            if(oppMeleeSum == maxRowOpp)
            {
                greatOppSumRow = board.opponentTransformMeleeRow.GetComponentsInChildren<DisplayCard>();
                greatOppRow = board.opponentTransformMeleeRow;
            }  
            else if(oppRangedSum == maxRowOpp)
            {
                greatOppSumRow = board.opponentTransformRangedRow.GetComponentsInChildren<DisplayCard>();
                greatOppRow = board.opponentTransformRangedRow;
            }
            else
            {
                greatOppSumRow = board.opponentTransformSeigeRow.GetComponentsInChildren<DisplayCard>();
                greatOppRow = board.opponentTransformSeigeRow;
            }
            //------------    
            if(activingC.card.owner == Card.Owner.Player)
            {
                if(greatSumRow.Length != greatOppSumRow.Length)
                {
                    DivideAttack(greatOppRow,Math.Max(greatSumRow.Length, greatOppSumRow.Length));
                }
                else
                {
                    if(greatSumRow.Length != 0) DivideAttack(greatOppRow, greatSumRow.Length);
                    else DivideAttack(greatOppRow, 1);
                }
            }
            else
            {
                if(greatSumRow.Length != greatOppSumRow.Length)
                {
                    DivideAttack(greatRow,Math.Max(greatSumRow.Length, greatOppSumRow.Length));
                }
                else
                {
                    if(greatSumRow.Length != 0) DivideAttack(greatRow, greatSumRow.Length);
                    else DivideAttack(greatRow, 1);
                }
            }
        }
    }

    //======Metodos Basicos para utilizar en los effectos=============================
    private List<DisplayCard> GetCards(Transform t, bool b)
    {
        List<DisplayCard> cardsToChangePower = new List<DisplayCard>();
        DisplayCard[] cards = t.GetComponentsInChildren<DisplayCard>();

        foreach(DisplayCard c in cards)
        {
            if(b && c.card.isUnit) cardsToChangePower.Add(c);
            else if(!c.card.isUnit) cardsToChangePower.Add(c);
        }
        
        return cardsToChangePower;
    }

    private Transform[] GetRows(bool b)
    {
        if(b) return new Transform[] {board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow};
        else return new Transform[] {board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow};
    }

    private void ShowCardsToChange(List<DisplayCard> cards, DisplayCard activingC)
    {
        foreach(Transform t in cardListContainer)
        {
            Destroy(t.gameObject);
        }

        foreach(DisplayCard c in cards)
        {
            GameObject buttonCard =Instantiate(cardButtonPrefab, cardListContainer);
            buttonCard.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(c.card.name);
            buttonCard.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                ActiveEffect(activingC, c.gameObject);
                cardSelectionPanel.SetActive(false);        
            });
        }
        cardSelectionPanel.SetActive(true);
    }

    private void ActiveEffect(DisplayCard c, GameObject g)
    {
        switch(c.card.name)
        {
            case "Kento Nanami":
                ReducerCourt(g);
                break;
            case "Mahito":
                SoulMutation(g);
                break;
            case "Hanami":
                Absorption(c.gameObject,g);
                break;
            case "Nobara Kugisaki":
                Vudu(c.gameObject, g);
                break;
            case "Kokichi Muta":
                ChangeToAverage(c, g.GetComponent<DisplayCard>());
                break;    
            default :
                break;                
        }
        cardSelectionPanel.SetActive(false);
    }

    private void ShowRowsToSelectd(DisplayCard activingC)
    {
        foreach(Transform t in cardListContainer)
        {
            Destroy(t.gameObject);
        }
        string[] rowsNames = {"Melee", "Ranged", "Seige", "OppMelee", "OppRanged", "OppSeige"};

        for(int i = 0; i < rowsNames.Length; i++)
        {
            GameObject buttonRow = Instantiate(cardButtonPrefab, cardListContainer);
            string rowName = rowsNames[i];
            buttonRow.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>(rowName);
            buttonRow.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GiveValueToSelectedRow(rowName, activingC));
        }
        cardSelectionPanel.SetActive(true);
    }

    private void GiveValueToSelectedRow(string name, DisplayCard activingC)
    {
        switch(name)
        {
            case "Melee" :
                selectedRow = board.transformMeleeRow;
                Debug.Log(selectedRow);
                break;
            case "Ranged" :
                selectedRow = board.transformRangedRow;
                Debug.Log(selectedRow);
                break;
            case "Seige" :
                selectedRow = board.transformSeigeRow;
                Debug.Log(selectedRow);
                break;
            case "OppMelee" :
                selectedRow = board.opponentTransformMeleeRow;
                Debug.Log(selectedRow);
                break;
            case "OppRanged" :
                selectedRow = board.opponentTransformRangedRow;
                Debug.Log(selectedRow);
                break;
            case "OppSeige" :
                selectedRow = board.opponentTransformSeigeRow;
                Debug.Log(selectedRow);
                break;
            default :
                selectedRow = null;
                break;  
        }
        Debug.Log(selectedRow);
        switch(activingC.card.name)
        {
            case "Shoko Ieiri" :
                IncreaseAttackByTwo(selectedRow);
                break;
            case "Jogo":
                ReduceByFive(activingC.gameObject, selectedRow);
                break;
            case "Autoencarnacion de la perfeccion" :
                ChangeToLower(selectedRow);
                break;
            default :
                break;    
        }

        cardSelectionPanel.SetActive(false);
    }

    private List<DisplayCard> GetCardsTo(DisplayCard activingC)
    {
        List<DisplayCard> cardsTo = new List<DisplayCard>();
        Transform[] rows = new Transform[3];
        if(activingC.card.owner == Card.Owner.Player) rows = GetRows(false);
        else rows = GetRows(true);
        if(rows.Length != 0)
        {
            foreach(Transform t in rows)
            {
                foreach(DisplayCard c in GetCards(t, true)) cardsTo.Add(c);
            }
        }
        return cardsTo;
    }
    
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
        if(cards.Length != 0)
        {
            int average = 0;
            foreach(DisplayCard c in cards)
            {
                average += c.card.GetPower();
            }
            return average/cards.Length;
        }
        else return 0;
    }

    private int CalculateSum(Transform row)
    {
        int sum = 0;
        DisplayCard[] cards = row.GetComponentsInChildren<DisplayCard>();
        foreach(DisplayCard c in cards)
        {
            sum += c.card.GetPower();
        }
        return sum;  
    } 

    //Borrar este metodo 
    private void SeeAttack(Transform row)
    {
        DisplayCard[] auxCards = row.GetComponentsInChildren<DisplayCard>();
        foreach(DisplayCard c in auxCards)
        {
            Debug.Log(c.card.GetPower());
        }
    }
}

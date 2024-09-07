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
    private Transform selectedRow;
    private GameObject selectedCard;

    private CardEffects cardEffect;

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
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ShowRowsToSelectd(activingC);
                break;
            case "ChangeToLower" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ShowRowsToSelectd(activingC);
                break;
            case "Absorption" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                List<DisplayCard> cardsToAbs = GetCardsTo(activingC);
                if(cardsToAbs.Count != 0) ShowCardsToChange(cardsToAbs, activingC);
                Absorption(activingCard, selectedCard);
                break;        
            case "ReducerCourt" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                List<DisplayCard> cardsToCurt = GetCardsTo(activingC);
                if(cardsToCurt.Count != 0) ShowCardsToChange(cardsToCurt, activingC);
                //ReducerCourt(selectedCard);
                break;
            case "ChangeToAverage" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
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
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                List<DisplayCard> cardsToMute = GetCardsTo(activingC);
                if(cardsToMute.Count != 0) ShowCardsToChange(cardsToMute, activingC);
                //SoulMutation(selectedCard, UnityEngine.Random.Range(0,21));
                break;            
            case "BloodManipulation" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                BloodManipulation(activingCard);
                break;
            case "Vudu" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                List<DisplayCard> cardsToVudu = GetCardsTo(activingC);
                if(cardsToVudu.Count != 0) ShowCardsToChange(cardsToVudu, activingC);
                //Vudu(activingCard, selectedCard);
                break;
            case "IncreaseAttackByTwo" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                ShowRowsToSelectd(activingC);
                IncreaseAttackByTwo(selectedRow);
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
        
        //ModifyRowByPower(row, activingCard, (c, actPower) => c.card.SetAttack(c.card.GetPower() - 5), "La seleccion no es valida vuelva a intentarlo");
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
            Debug.Log("La carta seleccionada tiene" + targedC.card.GetPower());
            activingC.card.SetAttack(activingC.card.GetPower() + absorpbedAttack);
            Debug.Log(activingC.card.GetPower());
        }
        else
        {
            ShowMessagePanel("La carta seleccionada no es valida");
        } 
    }

    //Nanami's Effect
    public void ReducerCourt(GameObject targedCard)
    {
        int reduction = UnityEngine.Random.Range(1,8);
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        targedC.card.SetAttack(targedC.card.GetPower() - reduction);
        Debug.Log(targedC.card.GetPower());
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
    public void SoulMutation(GameObject targedCard, int attackChange)
    {
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        if(targedC != null && 0 <= attackChange && attackChange <= 20)
        {
            targedC.card.SetAttack(attackChange);
            Debug.Log(targedC.card.GetPower());
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
        if(cards.Length != 0)
        {
            foreach(DisplayCard c in cards)
            {
                c.card.SetAttack(c.card.GetPower() + 2);
            }
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

            int maxRowPlayer = Math.Max(meleeSum, Math.Max(rangedSum, seigeSum));
            int maxRowOpp = Math.Max(oppMeleeSum, Math.Max(oppRangedSum, oppSeigeSum));

            DisplayCard[] greatSumRow;
            DisplayCard[] greatOppSumRow;

            Transform greatRow;
            Transform greatOppRow;

            if(meleeSum == maxRowPlayer)
            {
                greatSumRow = board.transformMeleeRow.GetComponents<DisplayCard>();
                greatRow = board.transformMeleeRow;
            }  
            else if(rangedSum == maxRowPlayer)
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
            if(oppMeleeSum == maxRowOpp)
            {
                greatOppSumRow = board.opponentTransformMeleeRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformMeleeRow;
            }  
            else if(oppRangedSum == maxRowOpp)
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

            int maxRowPlayer = Math.Max(meleeSum, Math.Max(rangedSum, seigeSum));
            int maxRowOpp = Math.Max(oppMeleeSum, Math.Max(oppRangedSum, oppSeigeSum));

            DisplayCard[] greatSumRow;
            DisplayCard[] greatOppSumRow;

            Transform greatRow;
            Transform greatOppRow;

            if(meleeSum == maxRowPlayer)
            {
                greatSumRow = board.transformMeleeRow.GetComponents<DisplayCard>();
                greatRow = board.transformMeleeRow;
            }  
            else if(rangedSum == maxRowPlayer)
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
            if(oppMeleeSum == maxRowOpp)
            {
                greatOppSumRow = board.opponentTransformMeleeRow.GetComponents<DisplayCard>();
                greatOppRow = board.opponentTransformMeleeRow;
            }  
            else if(oppRangedSum == maxRowOpp)
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
                //selectedCard = c.gameObject;
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
                SoulMutation(g, UnityEngine.Random.Range(0, 21));
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
            buttonRow.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => GiveValueToSelectedRow(rowsNames[i], activingC));
        }
        cardSelectionPanel.SetActive(true);
    }

    private void GiveValueToSelectedRow(string name, DisplayCard activingC)
    {
        switch(name)
        {
            case "Melee" :
                selectedRow = board.transformMeleeRow;
                break;
            case "Ranged" :
                selectedRow = board.transformRangedRow;
                break;
            case "Seige" :
                selectedRow = board.transformSeigeRow;
                break;
            case "OppMelee" :
                selectedRow = board.opponentTransformMeleeRow;
                break;
            case "OppRanged" :
                selectedRow = board.opponentTransformRangedRow;
                break;
            case "OppSeige" :
                selectedRow = board.opponentTransformSeigeRow;
                break;
            default :
                selectedRow = null;
                break;  
        }
        switch(activingC.card.name)
        {
            case "Shoko Ieiri" :
                IncreaseAttackByTwo(selectedRow);
                break;
            case "Jogo":
                ReduceByFive(activeCard, selectedRow);
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
}

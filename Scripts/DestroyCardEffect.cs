using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestroyCardEffect : MonoBehaviour , ICardEffect
{
    public GameObject auxCard;
    public BoardManager board;
    private CardEffects cardEffect;

    public void Execute(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        Initialize();
        //cardEffect = FindObjectOfType<CardEffects>();

        //auxCard = cardEffect.activingCard;

        switch(activingC.card.effect)
        {
            case "DespliegueDominio" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                DespliegueDominio(activingCard);
                break;
            case "DestroyMaxAttack" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                DestroyMaxAttack(activingCard);
                break;    
            case "DestroyLessAttack" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                DestroyLessAttack(activingCard);
                break;  
            case "MaxPowerOut" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                MaxPowerOut();
                break;
            case "GreatVoid" :
                Debug.Log("Activando el efecto de la carta " + activingC.card.name);
                GreatVoid(activingCard);
                break;        
            default :
                break;    
        }
    }

    public void Initialize()
    {
        board = FindObjectOfType<BoardManager>();
    }

    public void ShowMessagePanel(string sms)
    {
        Debug.Log(sms);
    }

    public bool CanActive()
    {
        //Revisar la Implementaciob de este metodo
        return true;
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

    //Despliegue de Dominio's effect
    public IEnumerator DespliegueDominio(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        //if(activingC.card.owner == Card.Owner.Player)
        //{
            if(ThereIs(board.transformWeatherMeleeSlot) || ThereIs(board.transformWeatherRangedSlot) || ThereIs(board.transformWeatherSeigeSlot)
            || ThereIs(board.opponentTransformWeatherMeleeSlot) || ThereIs(board.opponentTransformWeatherRangedSlot) || ThereIs(board.opponentTransformWeatherSeigeSlot))
            {
                yield return board.WaitForSelection<GameObject>
                (
                    selectedCard => 
                    {
                        auxCard = selectedCard;
                        if(auxCard != null)
                        {
                            DestroyCard(auxCard);
                        }
                    },
                    () => board.effectSelectedCard == null
                );
            }
        //}
        // else
        // {
        //     if(ThereIs(board.opponentTransformWeatherMeleeSlot) || ThereIs(board.opponentTransformWeatherRangedSlot) || ThereIs(board.opponentTransformWeatherSeigeSlot))
        //     {
        //         SelectedCard();
        //         DestroyCard();
        //     }
        // }

        EndEffect(activingCard);
    }

    //Destello Oscuro's effect
    public void DestroyMaxAttack(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        
        if(activingC.card.owner == Card.Owner.Player)
        {
            DestroyMaxAttackOnRow(board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow);
        }
        else 
        {
            DestroyMaxAttackOnRow(board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow);    
        }
    }

    //Esou's effect
    public void DestroyLessAttack(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            DestroyLessAttackOnRow(board.opponentTransformMeleeRow, board.opponentTransformRangedRow, board.opponentTransformSeigeRow);
        }
        else 
        {
            DestroyLessAttackOnRow(board.transformMeleeRow, board.transformRangedRow, board.transformSeigeRow);
        }
    }

    //MaxPowerOut's effect
    public void MaxPowerOut()
    {
        DisplayCard maxCardMelee = MaxAttackCard(board.transformMeleeRow);
        DisplayCard maxCardRanged = MaxAttackCard(board.transformRangedRow);
        DisplayCard maxCardSeige = MaxAttackCard(board.transformSeigeRow);

        DisplayCard maxCardOppMelee = MaxAttackCard(board.opponentTransformMeleeRow);
        DisplayCard maxCardOppRanged = MaxAttackCard(board.opponentTransformRangedRow);
        DisplayCard maxCardOppSeige = MaxAttackCard(board.opponentTransformSeigeRow);

        DisplayCard maxCard = GetMaxCard(maxCardMelee, GetMaxCard(maxCardRanged, maxCardSeige));
        DisplayCard maxOppCard = GetMaxCard(maxCardOppMelee, GetMaxCard(maxCardOppRanged, maxCardOppSeige));

        if(maxCard.card.GetPower() > maxOppCard.card.GetPower())
        {
            DestroyCard(maxCard.gameObject);
        }
        else if(maxOppCard.card.GetPower() > maxCard.card.GetPower())
        {
            DestroyCard(maxOppCard.gameObject);
        }
        else
        {
            DestroyCard(maxCard.gameObject);
            DestroyCard(maxOppCard.gameObject);
        }
    }

    //Satoru Gojo's effect
    public void Violet(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();
        List<DisplayCard> cardsInOppField = new List<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            AddCardToList(board.transformMeleeRow, cardsInOppField);
            AddCardToList(board.transformRangedRow, cardsInOppField);
            AddCardToList(board.transformSeigeRow, cardsInOppField);

            cardsInOppField.Sort((card1, card2) => (card2.card.GetPower().CompareTo(card1.card.GetPower())));

            for(int i = 0; i < Math.Min(3, cardsInOppField.Count); i++)
            {
                DestroyCard(cardsInOppField[i].gameObject);
            }
        }
        else
        {
            AddCardToList(board.opponentTransformMeleeRow, cardsInOppField);
            AddCardToList(board.opponentTransformRangedRow, cardsInOppField);
            AddCardToList(board.opponentTransformSeigeRow, cardsInOppField);

            cardsInOppField.Sort((card1, card2) => (card2.card.GetPower().CompareTo(card1.card.GetPower())));

            for(int i = 0; i < Math.Min(3, cardsInOppField.Count); i++)
            {
                DestroyCard(cardsInOppField[i].gameObject);
            }
        }
    }

    //Vacio Inconmensurable's effect
    public void GreatVoid(GameObject activingCard)
    {
        DisplayCard activingC = activingCard.GetComponent<DisplayCard>();

        if(activingC.card.owner == Card.Owner.Player)
        {
            DisplayCard liderC = board.transformLeaderCardSlot.GetComponent<DisplayCard>();

            if(!liderC.card.name.Equals("Satoru Gojo"))
            {
                board.CleanRow(MaxRow(board.transformMeleeRow, MaxRow(board.transformRangedRow, board.transformSeigeRow)), board.transformGraveyard);
                board.CleanRow(MaxRow(board.opponentTransformMeleeRow, MaxRow(board.opponentTransformRangedRow, board.opponentTransformSeigeRow)), board.opponentTransformGraveyard);
            }
            else
            {
                board.CleanRow(MaxRow(board.opponentTransformMeleeRow, MaxRow(board.opponentTransformRangedRow, board.opponentTransformSeigeRow)), board.opponentTransformGraveyard);
            }
        }
        else
        {
            DisplayCard liderC = board.opponentTransformLeaderCardSlot.GetComponent<DisplayCard>();

            if(!liderC.card.name.Equals("Satoru Gojo"))
            {
                board.CleanRow(MaxRow(board.transformMeleeRow, MaxRow(board.transformRangedRow, board.transformSeigeRow)), board.transformGraveyard);
                board.CleanRow(MaxRow(board.opponentTransformMeleeRow, MaxRow(board.opponentTransformRangedRow, board.opponentTransformSeigeRow)), board.opponentTransformGraveyard);
            }
            else
            {
                board.CleanRow(MaxRow(board.transformMeleeRow, MaxRow(board.transformRangedRow, board.transformSeigeRow)), board.transformGraveyard);
            }
        } 
    }

    //--------Metodos Basicos para utilizar en los efectos------------------------------------------------------------
    private bool ThereIs(Transform Row)
    {
        DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();
        
        if(cards.Length != 0) return true;
        else return false;
    }

    private DisplayCard MaxAttackCard(Transform Row)
    {
        if(Row == board.transformMeleeRow || Row == board.transformRangedRow || Row == board.transformSeigeRow
          || Row == board.opponentTransformMeleeRow || Row == board.opponentTransformRangedRow || Row == board.opponentTransformSeigeRow)
        {
            DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();
            DisplayCard strongestCard;

            if(cards.Length != 0)
            {
                strongestCard = cards[0];
                for(int i = 1; i < cards.Length; i++)
                {
                    if(strongestCard.card.GetPower() < cards[i].card.GetPower())
                    {
                        strongestCard = cards[i];
                    }
                }
                return strongestCard;
            }
            else return null;
        }
        else return null;    
    }

    private DisplayCard LessAttackCard(Transform Row)
    {
        if(Row == board.transformMeleeRow || Row == board.transformRangedRow || Row == board.transformSeigeRow
          || Row == board.opponentTransformMeleeRow || Row == board.opponentTransformRangedRow || Row == board.opponentTransformSeigeRow)
        {
            DisplayCard[] cards = Row.GetComponentsInChildren<DisplayCard>();
            DisplayCard weakestCard;

            if(cards.Length != 0)
            {
                weakestCard = cards[0];
                for(int i = 1; i < cards.Length; i++)
                {
                    if(weakestCard.card.GetPower() < cards[i].card.GetPower())
                    {
                        weakestCard = cards[i];
                    }
                }
                return weakestCard;
            }
            else return null;
        }
        else return null;    
    }

    private DisplayCard GetMaxCard(DisplayCard card1, DisplayCard card2)
    {
        if(card2 != null && (card1 == null || card2.card.GetPower() > card1.card.GetPower())) return card2;
        return card1;
    }

    private DisplayCard GetLessCard(DisplayCard card1, DisplayCard card2)
    {
        if(card2 != null && (card1 == null || card2.card.GetPower() < card1.card.GetPower())) return card2;
        return card1;
    }

    private void DestroyCard(GameObject targedCard)
    {
        DisplayCard targedC = targedCard.GetComponent<DisplayCard>();
        
        if(targedC.card.owner == Card.Owner.Player)
        {
            targedCard.transform.SetParent(board.transformGraveyard);
            targedCard.transform.localPosition = Vector3.zero;
            targedCard.transform.localScale = Vector3.one;
            targedCard.transform.localRotation = Quaternion.identity;
        }
        else
        {
            targedCard.transform.SetParent(board.opponentTransformGraveyard);
            targedCard.transform.localPosition = Vector3.zero;
            targedCard.transform.localScale = Vector3.one;
            targedCard.transform.localRotation = Quaternion.identity;
        }
    }

    private int Sum(Transform Row)
    {
        int sum = 0;
        DisplayCard[] cards = Row.GetComponents<DisplayCard>();

        if(cards.Length != 0)
        {
            foreach(DisplayCard c in cards)
            {
                sum += c.card.GetPower();
            }
            return sum;
        }
        else return -1;
    }

    private Transform MaxRow(Transform row1, Transform row2)
    {
        int sumRow1 = Sum(row1);
        int sumRow2 = Sum(row2);

        if(sumRow1 == -1 && sumRow2 != -1) return row2;
        else if(sumRow1 != -1 && sumRow2 == -1) return row1;
        else 
        {
            if(sumRow1 >= sumRow2) return row1;
            else return row2;
        }
    }

    private void DestroyMaxAttackOnRow(Transform Melee, Transform Ranged, Transform Seige)
    {
        DisplayCard maxCardMelee = MaxAttackCard(Melee);
        DisplayCard maxCardRanged = MaxAttackCard(Ranged);
        DisplayCard maxCardSeige = MaxAttackCard(Seige);

        DisplayCard cardToDestroy = GetMaxCard(maxCardMelee, GetMaxCard(maxCardRanged, maxCardSeige));

        if(cardToDestroy != null) DestroyCard(cardToDestroy.gameObject);
        else Debug.Log("No hay Carta para destruir"); 
    }

    private void DestroyLessAttackOnRow(Transform Melee, Transform Ranged, Transform Seige)
    {
        DisplayCard maxCardMelee = LessAttackCard(Melee);
        DisplayCard maxCardRanged = LessAttackCard(Ranged);
        DisplayCard maxCardSeige = LessAttackCard(Seige);

        DisplayCard cardToDestroy = GetLessCard(maxCardMelee, GetLessCard(maxCardRanged, maxCardSeige));

        if(cardToDestroy != null) DestroyCard(cardToDestroy.gameObject);
        else Debug.Log("No hay Carta para destruir"); 
    }

    private void AddCardToList(Transform row, List<DisplayCard> cards)
    {
        DisplayCard[] cardsInRow = row.GetComponents<DisplayCard>();
        foreach(DisplayCard c in cardsInRow)
        {
            cards.Add(c);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    CardManager cardManager;


    public Vector2 StartPosition;
    public List<GameObject> Cards;
    public List<GameObject> TableCards;
    public List<GameObject> DoubleTableCards;
    public List<GameObject> DiscardedCards;
    public List<GameObject> BurnedCards;
    public int HandPosition;
    public Vector2 DiscardPosition;
    private int block;
    public int Hp;
    public int HiglightPosition;
    public int CardsOnTheTableCounter = 0;
    public GameObject ManaPopUp;
    public int Strength;
    public int StartDrawCards = 5;

    public int Block
    {
        get => block;
        set
        {
            block = Mathf.Max(0, value);
        }
    }

    public void DrawCard()
    {
        DrawCard(StartDrawCards);
    }

    public void DrawCard(int numberOfCards)
    {
        int i = TableCards.Count;
        while (TableCards.Count < numberOfCards + i)
        {
            if (Cards.Count == 0)
            {
                ShufflingDrawDeck();
            }
            GameObject card = Cards[0];
            TableCards.Add(card);
            Cards.RemoveAt(0);
            cardManager.CallAnimation("draw", card);
        }
    }

    public void DiscardCards()
    {
        DiscardCards(TableCards.Count);
    }

    public void DiscardCards(int numberOfCards)
    {
        int cardsToDiscard = Mathf.Min(numberOfCards, TableCards.Count);

        for (int i = 0; i < cardsToDiscard; i++)
        {
            GameObject card = TableCards[TableCards.Count - 1];
            TableCards.RemoveAt(TableCards.Count - 1);
            DiscardedCards.Add(card);
            cardManager.CallAnimation("discard", card);
        }
    }

    public void DiscardCard(GameObject card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        DiscardedCards.Add(card);
        cardManager.CallAnimation("discard", card);
    }

    public void ShufflingDrawDeck()
    {
        while (DiscardedCards.Count > 0)
        {

            GameObject card = DiscardedCards[0];
            Cards.Add(card);
            DiscardedCards.RemoveAt(0);
        }
        for (int i = 0; i < Cards.Count; i++)
        {
            GameObject temp = Cards[i];
            int randomIndex = Random.Range(i, Cards.Count);
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }
        //cardManager.CallAnimation("shuffle");
    }

    public int DealDamage(int damage)
    {
        if (Block >= damage)
        {
            Block -= damage;
        }
        else
        {
            Hp -= (damage - Block);
            Block = 0;
        }

        return Hp;
    }

    void Awake()
    {
        //CardManager = ;
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        // Debug.Log("Awake in SIde happens");
    }

}

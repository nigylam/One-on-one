using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
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

    public Side(Vector2 startPosition, List<GameObject> cards, List<GameObject> tableCards, List<GameObject> doubleTableCards, List<GameObject> discardedCards, List<GameObject> burnedCards, int handPosition, Vector2 discardPosition, int block, int hp, int higlightPosition, GameObject manaPopUp, int strength)
    {
        StartPosition = startPosition;
        Cards = cards;
        TableCards = tableCards;
        DoubleTableCards = doubleTableCards;
        DiscardedCards = discardedCards;
        HandPosition = handPosition;
        DiscardPosition = discardPosition;
        Block = block;
        Hp = hp;
        HiglightPosition = higlightPosition;
        ManaPopUp = manaPopUp;
        Strength = strength;
        BurnedCards = burnedCards;

    }
    public int Block
    {
        get => block;
        set
        {
            block = Mathf.Max(0, value);
        }
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
        }
    }

    public void DiscardCard()
    {
        DiscardCard(TableCards.Count);
    }

    public void DiscardCard(int numberOfCards)
    {
        int cardsToDiscard = Mathf.Min(numberOfCards, TableCards.Count);

        for (int i = 0; i < cardsToDiscard; i++)
        {
            GameObject card = TableCards[TableCards.Count - 1];
            TableCards.RemoveAt(TableCards.Count - 1);
            DiscardedCards.Add(card);
        }
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

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    public event Action<GameObject, Side, CardActionType> CardAction;
    //public event Action<GameObject, Side> CardDiscarded;
    //public event Action<Side> Shuffle;

    CardManager cardManager;

    [HideInInspector]
    public List<GameObject> Cards;
    [HideInInspector]
    public List<GameObject> TableCards;
    [HideInInspector]
    public List<GameObject> DoubleTableCards;
    [HideInInspector]
    public List<GameObject> DiscardedCards;
    public List<GameObject> BurnedCards;
    public Vector2 StartPosition;
    public Vector2 DiscardPosition;
    public int HandPosition;
    public int Hp;
    public int HiglightPosition;
    public int CardsOnTheTableCounter = 0;
    public GameObject ManaPopUp;
    public int Strength;
    public int StartDrawCards = 5;
    public int AddCard = 0;

    int _savedTurn = 0;
    private int _block;

    public int Block
    {
        get => _block;
        set
        {
            _block = Mathf.Max(0, value);
        }
    }

    public void DrawCard(GameObject card)
    {
        TableCards.Add(card);
        Cards.Remove(card);
        CardAction?.Invoke(card, this, CardActionType.Draw);
    }


    public void DrawCards()
    {
        DrawCards(StartDrawCards);
    }

    public void DrawCards(int numberOfCards)
    {
        int i = TableCards.Count;
        while (TableCards.Count < numberOfCards + i)
        {
            if (Cards.Count == 0)
            {
                ShufflingDrawDeck();
                CardAction?.Invoke(null, this, CardActionType.Shuffle);
            }
            GameObject card = Cards[0];
            DrawCard(card);
        }
    }

    public void DiscardCard(GameObject card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        DiscardedCards.Add(card);
        CardAction?.Invoke(card, this, CardActionType.Discard);
    }

    public void DiscardCards()
    {
        DiscardCards(TableCards.Count);
        DoubleTableCards.Clear();
    }

    public void DiscardCards(int numberOfCards)
    {
        int cardsToDiscard = Mathf.Min(numberOfCards, TableCards.Count);

        for (int i = 0; i < cardsToDiscard; i++)
        {
            GameObject card = TableCards[TableCards.Count - 1];
            DiscardCard(card);
            //cardManager.CallAnimation("discard", card);
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
            int randomIndex = UnityEngine.Random.Range(i, Cards.Count);
            Cards[i] = Cards[randomIndex];
            Cards[randomIndex] = temp;
        }
        //cardManager.CallAnimation("shuffle");
    }

    public void AddCardBuff(int numberOfCards)
    {
        AddCard += numberOfCards;
        StartDrawCards += numberOfCards;

        _savedTurn = cardManager.curentTurn;
    }

    void RemoveAddCardBuff()
    {
        if (cardManager.curentTurn > _savedTurn)
        {
            AddCard = 0;
            StartDrawCards = 5;

            _savedTurn = 0;
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

    void Awake()
    {
        //CardManager = ;
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        // Debug.Log("Awake in SIde happens");
    }

    void Update()
    {
        RemoveAddCardBuff();



        foreach (GameObject card in TableCards)
        {
        }

    }

}

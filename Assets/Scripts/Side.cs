using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Side : MonoBehaviour
{
    public event Action<GameObject, Side, IPlayable> CardAction;

    CardManager cardManager;
    public BurnAnimation burnAnimation;
    public ShuffleAnimation shuffleAnimation;
    public DiscardAnimation discardAnimation;
    public DrawAnimation drawAnimation;
    public AddAnimation playAnimation;

    public GameObject[] CardList = new GameObject[] {};

    //public string[] CardList = new 

    // У тебя точно под них область памяти выделяется, если ты их только объявляешь?
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
    //public GameObject ManaPopUp;
    public int Strength;
    public int Rage = 0;
    public int StartDrawCards = 5;
    public int AddCard = 0;

    public int DrawCounter = 0;
    public int DiscardCounter = 0;

    public bool PlanRevenge = false;

    public bool discardMode = false;
    public bool burnMode = false;


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
        CardAction?.Invoke(card, this, drawAnimation);
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
                CardAction?.Invoke(null, this, shuffleAnimation);
            }
            GameObject card = Cards[0];
            DrawCard(card);
        }
    }

    public void DiscardCard(GameObject card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        if (cardManager.playingCards.Contains(card)) { cardManager.playingCards.Remove(card); }
        DiscardedCards.Add(card);
        CardAction?.Invoke(card, this, discardAnimation);
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
    }

    public void PlayCard(GameObject card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        CardAction?.Invoke(card, this, playAnimation);
    }

    public void BurnCard(GameObject card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        BurnedCards.Add(card);
        CardAction?.Invoke(card, this, burnAnimation);
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

    public void RageBuff(int numberOfTurns)
    {
        Rage += numberOfTurns;
    }

    public void RageDecrease()
    {
        if (Rage > 0)
            Rage--;
    }

    void Awake()
    {
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        burnAnimation = new BurnAnimation();
        shuffleAnimation = new ShuffleAnimation();
        discardAnimation = new DiscardAnimation();
        drawAnimation= new DrawAnimation();
        playAnimation = new AddAnimation();

        Cards.Clear();
        foreach (GameObject card in CardList)
        {
            Cards.Add(card);
        }
}

    void Update()
    {
        RemoveAddCardBuff();
    }

}

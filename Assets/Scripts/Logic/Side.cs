using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Side;

// нам нужен класс сайд и классы наследники для разных сторон.
// у наследников будут уникальные эффекты и манаспенд

public class Side : MonoBehaviour
{
    public GameObject[] CardList = new GameObject[] { };
    public List<GameObject> Cards = new List<GameObject>();
    public List<GameObject> TableCards = new List<GameObject>();
    public List<GameObject> DoubleTableCards = new List<GameObject>();
    public List<GameObject> DiscardedCards = new List<GameObject>();
    public List<GameObject> BurnedCards = new List<GameObject>();
    public ManaType DefaultManaType;
    public ManaType AlterManaType;

    public event Action<GameObject, Side, IPlayable> CardAction;
    public event Action<Side, ManaType, int> StatsAction;

    public int Hp;
    public int Strength;
    public int Rage = 0;
    public int StartDrawCards = 5;
    public int AddCard = 0;
    public bool PlanRevenge = false;

    private int _block;

    public int Block
    {
        get => _block;
        set
        {
            _block = Mathf.Max(0, value);
        }
    }

    public enum ManaType
    {
        Sacrifice = 0,
        Discard = 1
    }

    public bool discardMode = false;
    public bool burnMode = false;
    public bool manaSpendMode = false;

    public void ManaSpend(0)
    {
        ManaSpend(-1, DefaultManaType);
    }

    public void ManaSpend(int mana)
    {
        ManaSpend(int mana, DefaultManaType);
    }

    public void ManaSpend(int mana, ManaType manaType)
    {
        manaSpendMode = true;
        int neededCardsCount = TableCards.Count - mana;
        CardsManaSpendActivation(manaType);
        StartCoroutine(ManaSpend(neededCardsCount));
        StatsAction?.Invoke(this, ManaType, mana);
    }

    public void CardsManaSpendActivation(ManaType manaType)
    {
        if (manaType == ManaType.Sacrifice)
            burnMode = true;
        else
            discardMode = true;
    }
    public void CardsManaSpendDeactivation()
    {
        burnMode = false;
        discardMode = false;
    }

    public IEnumerator ManaSpend(int mana)
    {
        yield return new WaitUntil(() => TableCards.Count == mana || !manaSpendMode);
        manaSpendMode = false;
        CardsManaSpendDeactivation();
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

    public int CalculateDamage(int damage)
    {
        int finalDamage = damage + Strength;
        finalDamage = Rage > 0 ? Convert.ToInt32(Math.Ceiling((1.5) * Convert.ToSingle(finalDamage))) : finalDamage;
        return finalDamage;
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

    public void EndTurn()
    {
        RageDecrease();
        DiscardCards();
        DrawCards();
    }

    // убрать
    // все после этих комментариев
    CardManager cardManager;
    public BurnAnimation burnAnimation;
    public ShuffleAnimation shuffleAnimation;
    public DiscardAnimation discardAnimation;
    public DrawAnimation drawAnimation;
    public AddAnimation playAnimation;

    public int HandPosition;
    public int HiglightPosition;
    public int CardsOnTheTableCounter = 0;
    public int DrawCounter = 0;
    public int DiscardCounter = 0;

    int _savedTurn = 0;


    void Awake()
    {
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        burnAnimation = new BurnAnimation();
        shuffleAnimation = new ShuffleAnimation();
        discardAnimation = new DiscardAnimation();
        drawAnimation = new DrawAnimation();
        playAnimation = new AddAnimation();

        Cards.Clear();
        foreach (GameObject card in CardList)
        {
            Cards.Add(card);
        }
    }

    // это надо в конце хода делать - отдельный скрипт для конца хода
    void RemoveAddCardBuff()
    {
        if (cardManager.curentTurn > _savedTurn)
        {
            AddCard = 0;
            StartDrawCards = 5;

            _savedTurn = 0;
        }
    }
}

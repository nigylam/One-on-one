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

    public event Action<Side, Card, CardActionType> CardAction;
    public event Action<Side, ManaType, int, Card, bool> SacrDiscPopup;
    public event Action<Side, StatActionType, Card, int> StatAction;

    public int Hp;
    public int Block;
    public int Strength;
    public int Rage = 0;
    public int StartDrawCards = 5;
    public int AddCard = 0;
    public bool PlanRevenge = false;

    public enum ManaType
    {
        Sacrifice = 0,
        Discard = 1
    }

    public enum CardActionType
    {
       Burn,
       Shuffle,
       Discard,
       Draw,
    }

    public enum StatActionType
    {
        Block,
        Damage,
        AddCard,
        AddRage,
        DecreaseRage,
        AddCardBuffRemove
    }

    // методы с отправкой ивентов
    public void ManaSpend(int mana, ManaType manaType, Card card)
    {
        manaSpendMode = true;
        int neededCardsCount = TableCards.Count - mana;
        CardsManaSpendActivation(manaType);
        StartCoroutine(ManaSpend(neededCardsCount));
        SacrDiscPopup?.Invoke(this, manaType, mana, card, true);
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
        SacrDiscPopup?.Invoke(this, null, null, null, false);
    }

    public void DrawCard(Card card)
    {
        TableCards.Add(card);
        Cards.Remove(card);
        CardAction?.Invoke(card, this, CardActionType.Draw);
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
        CardAction?.Invoke(null, this, CardActionType.Shuffle);
    }

    public void DiscardCard(Card card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        DiscardedCards.Add(card);
        CardAction?.Invoke(card, this, CardActionType.Discard);
    }

    public void BurnCard(Card card)
    {
        if (TableCards.Contains(card)) { TableCards.Remove(card); }
        BurnedCards.Add(card);
        CardAction?.Invoke(card, this, CardActionType.Burn);
    }

    public void AddCardBuff(int numberOfCards, Card card)
    {
        AddCard += numberOfCards;
        StartDrawCards += numberOfCards;
        StatAction?.Invoke(this, StatActionType.AddCard, card, numberOfCards);
    }

    public void DealDamage(int damage)
    {
        if (Block >= damage)
        {
            // для блока нужен отдельный метод
            GainBlock(-damage);
        }
        else
        {
            Hp -= (damage - Block);
            GainBlock(-Block);
        }

        //интересно, можно тут зис поставить для карты или с помощью конструктора это обыграть
        StatAction?.Invoke(this, StatActionType.Damage, this, damage);
    }

    public int GainBlock(int block)
    {
        Block += block;

        StatAction?.Invoke(this, StatActionType.Block, null, block);
    }

    public void RageBuff(int numberOfTurns, Card card)
    {
        Rage += numberOfTurns;
        StatAction?.Invoke(this, StatActionType.AddRage, card, numberOfTurns);
    }

    public void RageDecrease()
    {
        if (Rage > 0)
            Rage--;
        StatAction?.Invoke(this, StatActionType.AddRage, card, numberOfTurns);
    }

    public void AddCardBuffRemove()
    {
        AddCard = 0;
        StatAction?.Invoke(this, StatActionType.AddCardBuffRemove, null, null);
    }

    // служебные методы
    public void EndTurn()
    {
        RageDecrease();
        DiscardCards();
        DrawCards();
    }

    public IEnumerator ManaSpend(int mana)
    {
        yield return new WaitUntil(() => TableCards.Count == mana || !manaSpendMode);
        manaSpendMode = false;
        CardsManaSpendDeactivation();
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
            }
            GameObject card = Cards[0];
            DrawCard(card);
        }
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

    public int CalculateDamage(int damage)
    {
        int finalDamage = damage + Strength;
        finalDamage = Rage > 0 ? Convert.ToInt32(Math.Ceiling((1.5) * Convert.ToSingle(finalDamage))) : finalDamage;
        return finalDamage;
    }
}

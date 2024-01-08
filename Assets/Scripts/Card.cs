using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;
using Unity.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    //card meta
    public string cardId;
    public CardType cardType;
    public string CardName;
    public string CardDescription;

    //card logic
    public int Mana = 0;
    public bool IsSpecial = false;
    public int Damage = 0;
    public int Block = 0;
    public int GainStrength = 0;
    public int GainDexterity = 0;
    public int DrawCards = 0;
    public int AddCardBuff = 0;
    public int Rage = 0;

    public bool Playable = false;
    public bool Playing = false;

    //card side
    public Side cardSide;
    public Side otherSide;

    //��������� ����, � ������� �� ��������� ������ �� ������������� � ������ ��������
    //��� �� ���� ����������? ��� �������� ����� �������� ����������?
    //�� ���� ��� ���������� �������������� ��� ������ ����� ����� ��������: ����, �������, �����

    public Stack<CardEvent> eventStack = new Stack<CardEvent>();

    // ���������� ������ ��������, ��� � �����, ����� ���������� ���� � �� ������� ������ ��������� ����������

    public enum CardType
    {
        Attack = 0,
        Defend = 1,
        Skill = 3,
        Power = 4
    }

    public void ManaCheck(int manaAmount)
    {
        if(manaAmount >= Mana)
        {
            Playable = true;
        }
    }

    public void Play()
    {
        Playing = true;
        if (!IsSpecial)
        {
            cardSide.Block += Block;
            otherSide.DealDamage(cardSide.CalculateDamage(Damage));
            cardSide.Strength += GainStrength;
            cardSide.DrawCards(DrawCards);
            cardSide.AddCardBuff(AddCardBuff);
            cardSide.RageBuff(Rage);

            Playing = false;
        }
        else
        {
            StartCoroutine(PlaySpecialCard());
        }
    }

    public void RemoveFromTable()
    {
        if (!Playing)
        {
            if (cardType == CardType.Power) { cardSide.BurnCard(gameObject); }
            else
            {
                cardSide.DiscardCard(gameObject);
            }
        }
    }

    // ������ �����, ��� ������ ���������� �� ����� ������� � �������������? ��� �� ������ ������ � ��������� � �������� ����������?
    // ������� ��������, ��� � ������� ��� ������ ����� ����� ������� ��� ���� ��������� ����
    public IEnumerator PlaySpecialCard()
    {
        switch (cardId)
        {
            case "GraySkill1":
                cardSide.DrawCards(2);
                cardSide.ManaSpend(1, Side.ManaType.Discard, this);
                yield return new WaitUntil(() => cardSide.manaSpendMode);
                break;
            case "BlueAttack3":
                if (eventStack.Peek().Card.GetComponent<Card>().cardType == Card.CardType.Attack)
                {
                    otherSide.DealDamage(cardSide.CalculateDamage(Damage));
                }
                break;
            case "BlueAttack4":
                otherSide.DealDamage(cardSide.CalculateDamage(Math.Ceiling(Convert.ToSingle(card.cardSide.Block) / 2)));
                break;
            case "BlueSkill1":
                otherSide.ManaSpend(1, Side.ManaType.Discard, this);
                yield return new WaitUntil(() => cardSide.manaSpendMode);
                break;

            // � ��� � ���������� �� �����������, � ���� ���� ����� �������� �����
            case "BlueSkill2":
                int cardsDiscarded = 0;
                cardSide.ManaSpend(-1, Side.ManaType.Discard, this);
                yield return new WaitUntil(() => cardSide.manaSpendMode);
                foreach (CardEvent cardEvent in eventStack)
                {
                    cardsDiscarded++;
                    if (cardEvent.Card == this)
                        break;
                }
                otherSide.ManaSpend(cardsDiscarded, Side.ManaType.Discard, this);
                yield return new WaitUntil(() => cardSide.manaSpendMode);
                break;
        }

        Playing = false;
    }
}
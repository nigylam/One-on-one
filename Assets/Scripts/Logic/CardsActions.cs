using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardsActions : MonoBehaviour
{
    Card card;
    CardManager cardManager;
    StatManager statManager;


    void Start()
    {
        card = gameObject.GetComponent<Card>();
        statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
    }

    void Update()
    {

    }

    public IEnumerator PlaySpecialCard()
    {
        switch (card.cardId)
        {
            case "GraySkill1":
                card.cardSide.DrawCards(2);
                StartCoroutine(card.ManaSpending(card.cardSide, card.discardType, 1));
                yield return new WaitUntil(() => card.playerActionCompleted);
                break;
            case "GraySkill2":
                card.cardSide.AddCardBuff(1);
                card.playerActionCompleted = true;
                break;
            case "BlueAttack3":
                if (cardManager.eventStack.Peek().Card.GetComponent<Card>().cardType == Card.CardType.Attack)
                {
                    card.otherSide.DealDamage(card.finalDamage);
                }
                card.playerActionCompleted = true;
                break;
            case "BlueAttack4":
                card.otherSide.DealDamage(Convert.ToInt32(Math.Ceiling(Convert.ToSingle(card.cardSide.Block)/2)));
                card.playerActionCompleted = true;
                break;
            case "BlueSkill1":
                StartCoroutine(card.ManaSpending(card.otherSide, card.discardType, 1));
                yield return new WaitUntil(() => card.playerActionCompleted);
                break;
            case "BlueSkill2":
                card.cardsDiscarded = 0;
                StartCoroutine(card.ManaSpending(card.cardSide, card.discardType));
                yield return new WaitUntil(() => card.playerActionCompleted);
                card.playerActionCompleted = false;
                StartCoroutine(card.ManaSpending(card.otherSide, card.discardType, card.cardsDiscarded, true));
                yield return new WaitUntil(() => card.playerActionCompleted);
                card.cardsDiscarded = 0;
                break;
        }
    }
}

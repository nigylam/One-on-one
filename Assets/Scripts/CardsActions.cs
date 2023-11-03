using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsActions : MonoBehaviour
{
    CardScript cardScript;
    CardManager cardManager;
    StatManager statManager;


    void Start()
    {
        cardScript = gameObject.GetComponent<CardScript>();
        statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();

    }

    void Update()
    {

    }

    public IEnumerator PlaySpecialCard()
    {
        switch (cardScript.cardId)
        {
            case "GraySkill1":
                cardScript.cardSide.DrawCards(2);
                StartCoroutine(cardScript.ManaSpending(cardScript.cardSide, cardScript.discardType, 1));
                yield return new WaitUntil(() => cardScript.playerActionCompleted);
                break;
            case "GraySkill2":
                cardScript.cardSide.AddCardBuff(1);
                cardScript.playerActionCompleted = true;
                break;
            case "BlueAttack3":
                if (cardManager.eventStack.Peek().Card.GetComponent<CardScript>().cardType == CardScript.CardType.Attack)
                {
                    cardScript.otherSide.DealDamage(cardScript.finalDamage);
                }
                cardScript.playerActionCompleted = true;
                break;
            case "BlueSkill1":
                StartCoroutine(cardScript.ManaSpending(cardScript.otherSide, cardScript.discardType, 1));
                yield return new WaitUntil(() => cardScript.playerActionCompleted);
                break;
            case "BlueSkill2":
                StartCoroutine(cardScript.ManaSpending(cardScript.cardSide, cardScript.discardType));
                yield return new WaitUntil(() => cardScript.playerActionCompleted);
                cardScript.playerActionCompleted = false;
                StartCoroutine(cardScript.ManaSpending(cardScript.otherSide, cardScript.discardType, cardScript.cardsDiscarded, true));
                yield return new WaitUntil(() => cardScript.playerActionCompleted);
                cardScript.cardsDiscarded = 0;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsActions : MonoBehaviour
{
    CardScript CardScript;

    public GameObject CardManager;
    CardManager cardManagerScript;

    public GameObject StatManager;
    StatManager statManagerScript;

    bool playerActionCompleted = false;

    public Side cardSide;
    public Side otherSide;

    public IEnumerator PlayingCard()
    {
        switch (CardScript.cardId)
        {
            case "RedAttack1":
                DealDamage(6);
                break;
            case "RedAttack2":
                StartCoroutine(Sacrifice(2));
                yield return new WaitUntil(() => playerActionCompleted);
                DealDamage(12);
                break;
            case "RedDefend1":
                GainBlock(5);
                break;
            case "BlueAttack1":
                DealDamage(5);
                break;
            case "BlueAttack2":
                StartCoroutine(Discard(1));
                yield return new WaitUntil(() => playerActionCompleted);
                DealDamage(4);
                StartCoroutine(cardManagerScript.DrawingCard(cardSide, 1, 0));
                break;
            case "BlueDefend1":
                GainBlock(6);
                break;
        }
        cardSide.DiscardedCards.Add(gameObject);
        playerActionCompleted = false;
    }

    public IEnumerator Sacrifice(int numberOfCards)
    {
        int initialCardCount = cardManagerScript.enemyCardsOnTheTable.Count;
        statManagerScript.CardSacrPopUp.SetActive(true);
        CardScript.desiredPosition = transform.localPosition;
        transform.localScale = new Vector2(1f, 1f);
        CardScript.sprite.sortingLayerName = "Background";
        cardManagerScript.SacrificeMode = true;
        yield return new WaitUntil(() => cardManagerScript.enemyCardsOnTheTable.Count <= initialCardCount - numberOfCards);
        playerActionCompleted = true;
        statManagerScript.CardSacrPopUp.SetActive(false);
        cardManagerScript.SacrificeMode = false;
        CardScript.sprite.sortingLayerName = "Default";
    }

    public IEnumerator Discard(int numberOfCards)
    {
        int initialCardCount = cardManagerScript.cardsOnTheTable.Count;
        cardManagerScript.playingCards.Add(gameObject);
        statManagerScript.CardDiscPopUp.SetActive(true);
        CardScript.desiredPosition = transform.localPosition;
        transform.localScale = new Vector2(1f, 1f);
        CardScript.sprite.sortingLayerName = "Default";
        CardScript.sprite.sortingOrder = 0;
        cardManagerScript.DiscardMode = true;
        yield return new WaitUntil(() => cardManagerScript.cardsOnTheTable.Count <= initialCardCount - numberOfCards);
        playerActionCompleted = true;
        statManagerScript.CardDiscPopUp.SetActive(false);
        cardManagerScript.DiscardMode = false;
        CardScript.sprite.sortingLayerName = "Default";
        cardManagerScript.playingCards.Remove(gameObject);
    }

    public void DealDamage(int amountDamage)
    {
        
        if (otherSide.Block >= amountDamage)
        {
            otherSide.Block -= amountDamage;
        }
        else
        {
            otherSide.Hp -= amountDamage - otherSide.Block;
            otherSide.Block = 0;
        }
    }

    public void GainBlock(int amountBlock)
    {
        cardSide.Block += amountBlock;
    }

    void Start()
    {
        CardScript = gameObject.GetComponent<CardScript>();

        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();

        CardManager = GameObject.Find("Card Manager");
        cardManagerScript = CardManager.GetComponent<CardManager>();

        if(gameObject.tag == "Player")
        {
            cardSide = cardManagerScript.player;
            otherSide = cardManagerScript.enemy;
        } else
        {
            cardSide = cardManagerScript.enemy;
            otherSide = cardManagerScript.player;
        }
    }

    void Update()
    {

    }
}

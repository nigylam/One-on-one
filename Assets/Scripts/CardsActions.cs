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

    Side cardSide;
    Side otherSide;

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
            case "BlueDefend1":
                GainBlock(6);
                break;
        }
        Discard();
    }

    public IEnumerator Sacrifice(int numberOfCards)
    {
        int initialCardCount = cardManagerScript.enemyCardsOnTheTable.Count;
        statManagerScript.CardSacrPopUp.SetActive(true);
        CardScript.desiredPosition = transform.localPosition;
        transform.localScale = new Vector2(1f, 1f);
        CardScript.needHighliht = false;
        CardScript.sprite.sortingLayerName = "Background";
        cardManagerScript.SacrificeMode = true;
        yield return new WaitUntil(() => cardManagerScript.enemyCardsOnTheTable.Count <= initialCardCount - (numberOfCards));
        playerActionCompleted = true;
        statManagerScript.CardSacrPopUp.SetActive(false);
        cardManagerScript.SacrificeMode = false;
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

    public void Discard()
    {
        cardSide.TableCards.Remove(gameObject);
        CardScript.desiredPosition = cardSide.DiscardPosition;
        cardManagerScript.CalculateCardPosition(cardSide);
        cardSide.DiscardedCards.Add(gameObject);
    }

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {

    }
}

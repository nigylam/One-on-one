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

    Side side;

    public IEnumerator PlayingCard()
    {
        switch (CardScript.cardId)
        {
            case "RedAttack1":
                if (statManagerScript.block >= 6)
                {
                    statManagerScript.block -= 6;
                }
                else
                {
                    statManagerScript.hp -= 6 - statManagerScript.block;
                    statManagerScript.block = 0;
                }
                break;


            case "RedAttack2":

                Sacrifice(1);
                yield return new WaitUntil(() => playerActionCompleted);
                cardManagerScript.SacrificeMode = false;
                

                if (statManagerScript.block >= 12)
                {
                    statManagerScript.block -= 12;
                }
                else
                {
                    statManagerScript.hp -= 12 - statManagerScript.block;
                    statManagerScript.block = 0;
                }
                break;
            case "RedDefend1":
                statManagerScript.enemyBlock += 5;
                break;
            case "BlueAttack1":
                if (statManagerScript.enemyBlock >= 5)
                {
                    statManagerScript.enemyBlock -= 5;
                }
                else
                {
                    statManagerScript.enemyHp -= 5 - statManagerScript.enemyBlock;
                    statManagerScript.enemyBlock = 0;
                }
                break;
            case "BlueDefend1":
                statManagerScript.block += 6;
                break;
        }



        if (gameObject.tag == "clone")
        {
            cardManagerScript.cardsOnTheTable.Remove(gameObject);
            CardScript.desiredPosition = new Vector2(1150, -670);
            cardManagerScript.CalculateCardPosition(cardManagerScript.player);
            // добавить карту в список сброшенных карт!!!
        }
        else
        {
            cardManagerScript.enemyCardsOnTheTable.Remove(gameObject);
            CardScript.desiredPosition = new Vector2(1150, 670);
            // добавить карту в список!!!
        }

        playerActionCompleted = false;
    }

    public void Sacrifice(int numberOfCards)
    {
        statManagerScript.CardSacrPopUp.SetActive(true);

        CardScript.desiredPosition = transform.localPosition;
        transform.localScale = new Vector2(1f, 1f);
        CardScript.needHighliht = false;
        CardScript.sprite.sortingLayerName = "Background";

        cardManagerScript.SacrificeMode = true;

        StartCoroutine(WaitForPlayerAction());
    }

    private IEnumerator WaitForPlayerAction()
    {
        int initialCardCount = cardManagerScript.enemyCardsOnTheTable.Count;

        while (cardManagerScript.enemyCardsOnTheTable.Count > initialCardCount - 2) 
        {
            yield return null;
        }

        playerActionCompleted = true;
        statManagerScript.CardSacrPopUp.SetActive(false);
    }

    //public void DealDamage(int amountDamage)
    //{
    //    if (side.block >= 6)
    //    {
    //        side.block -= 6;
    //    }
    //    else
    //    {
    //       side.hp -= 6 - side.block;
    //        side.block = 0;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        CardScript = gameObject.GetComponent<CardScript>();

        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();

        CardManager = GameObject.Find("Card Manager");
        cardManagerScript = CardManager.GetComponent<CardManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

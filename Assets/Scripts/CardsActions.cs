using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsActions : MonoBehaviour
{
    CardScript cardInfo;

    public GameObject CardManager;
    CardManager cardManagerScript;

    public GameObject StatManager;
    StatManager statManagerScript;

    public void PlayingCard()
    {
        switch (cardInfo.cardId)
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
            cardInfo.desiredPosition = new Vector2(1150, -670);
            cardManagerScript.CalculateCardPosition(cardManagerScript.player);
            // добавить карту в список сброшенных карт!!!
        }
        else
        {
            cardManagerScript.enemyCardsOnTheTable.Remove(gameObject);
            cardInfo.desiredPosition = new Vector2(1150, 670);
            // добавить карту в список!!!
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cardInfo = gameObject.GetComponent<CardScript>();

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

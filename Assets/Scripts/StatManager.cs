using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class StatManager : MonoBehaviour
{
    public GameObject CardManager;
    CardManager cardManagerScript;

    public GameObject drawPile;
    public GameObject enemyDrawPile;
    public GameObject discardPile;
    public GameObject enemyDiscardPile;

    public GameObject healthPointsCounter;
    public GameObject enemyHealthPointsCounter;
    public GameObject blockPointsCounter;
    public GameObject enemyBlockPointsCounter;

    public GameObject strength;
    public GameObject enemyStrength;
    public GameObject strengthCounter;
    public GameObject enemyStrengthCounter;

    public GameObject enemyRage;
    TextMeshProUGUI enemyRageCounter;

    public GameObject AddCards;
    public GameObject AddCardsCounter;

    public int drawPileCounter = 5;
    public int enemyDrawPileCounter = 5;
    public int discardPileCounter = 0;
    public int enemyDiscardPileCounter = 0;
    public int hp = 30;
    public int enemyHp = 30;
    public int block = 4;
    public int enemyBlock = 5;

    TextMeshProUGUI drawCounter;
    TextMeshProUGUI enemyDrawCounter;
    TextMeshProUGUI discardCounter;
    TextMeshProUGUI enemyDiscardCounter;

    TextMeshProUGUI healthPoints;
    TextMeshProUGUI enemyHealthPoints;
    TextMeshProUGUI blockPoints;
    TextMeshProUGUI enemyBlockPoints;

    TextMeshProUGUI strengthCounterText;
    TextMeshProUGUI enemyStrengthCounterText;

    TextMeshProUGUI AddCardsCounterText;

    public bool cardsChoosed = true;

    public GameObject CardBurnDiscardPopUp;
    public GameObject CardDiscBurnButton;
    public GameObject CardDiscBurnButtonButton;
    //public GameObject CardDiscPopUp;

    public LocalizeStringEvent tipLocizeText;

    // Start is called before the first frame update
    void Start()
    {
        drawCounter = drawPile.GetComponent<TextMeshProUGUI>();
        enemyDrawCounter = enemyDrawPile.GetComponent<TextMeshProUGUI>();
        discardCounter = discardPile.GetComponent<TextMeshProUGUI>();
        enemyDiscardCounter = enemyDiscardPile.GetComponent<TextMeshProUGUI>();

        healthPoints = healthPointsCounter.GetComponent<TextMeshProUGUI>();
        enemyHealthPoints = enemyHealthPointsCounter.GetComponent<TextMeshProUGUI>();
        blockPoints = blockPointsCounter.GetComponent<TextMeshProUGUI>();
        enemyBlockPoints = enemyBlockPointsCounter.GetComponent<TextMeshProUGUI>();

        strengthCounterText = strengthCounter.GetComponent<TextMeshProUGUI>();
        enemyStrengthCounterText = enemyStrengthCounter.GetComponent<TextMeshProUGUI>();

        enemyRageCounter = enemyRage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        cardManagerScript = CardManager.GetComponent<CardManager>();

        AddCardsCounterText = AddCardsCounter.GetComponent<TextMeshProUGUI>();

        tipLocizeText = CardBurnDiscardPopUp.transform.GetChild(0).GetComponent<LocalizeStringEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        drawCounter.text = "" + cardManagerScript.player.DrawCounter;
        enemyDrawCounter.text = "" + cardManagerScript.enemy.DrawCounter;
        discardCounter.text = "" + cardManagerScript.player.DiscardCounter;
        enemyDiscardCounter.text = "" + cardManagerScript.enemy.DiscardCounter;

        healthPoints.text = "" + cardManagerScript.player.Hp;
        enemyHealthPoints.text = "" + cardManagerScript.enemy.Hp;
        blockPoints.text = "" + cardManagerScript.player.Block;
        enemyBlockPoints.text = "" + cardManagerScript.enemy.Block;

        strengthCounterText.text = "" + cardManagerScript.player.Strength;
        enemyStrengthCounterText.text = "" + cardManagerScript.enemy.Strength;

        AddCardsCounterText.text = "" + cardManagerScript.player.AddCard;

        enemyRageCounter.text = "" + cardManagerScript.enemy.Rage;

        if (cardManagerScript.player.Strength == 0)
        {
            strength.SetActive(false);
        } else
        {
            strength.SetActive(true);
        }
        if(cardManagerScript.enemy.Strength == 0)
        {
            enemyStrength.SetActive(false);
        }
        else
        {
            enemyStrength.SetActive(true);
        }
        if (cardManagerScript.player.AddCard == 0)
        {
            AddCards.SetActive(false);
        }
        else
        {
            AddCards.SetActive(true);
        }
        if (cardManagerScript.enemy.Rage == 0)
        {
            enemyRage.SetActive(false);
        }
        else
        {
            enemyRage.SetActive(true);
        }
    }

    /*
    public void EnablingPopUp(Side side, ITypeMana manaType, int numberOfCards = 0)
    {
        switch (popUpTextType)
        {
            case PopUpTextType.DiscardPlayerCard:
                tipLocizeText.StringReference.TableEntryReference = "DiscPlayerPop_Tip";
                break;
            case PopUpTextType.DiscardEnemyCard:
                tipLocizeText.StringReference.TableEntryReference = "DiscEnemyPop_Tip";
                break;
            case PopUpTextType.BurnPlayerCard:
                tipLocizeText.StringReference.TableEntryReference = "SacrPlayerPop_Tip";
                break;
            case PopUpTextType.BurnEnemyCard:
                tipLocizeText.StringReference.TableEntryReference = "SacrEnemyPop_Tip";
                break;
            case PopUpTextType.DiscardAnyPlayerCards:
                tipLocizeText.StringReference.TableEntryReference = "DiscAnyPlayerPop_Tip";
                CardDiscBurnButton.SetActive(true);
                break;
        }
    }
    */

    public void DisablingPopUp()
    {
        CardBurnDiscardPopUp.SetActive(false);
        CardDiscBurnButton.SetActive(false);
    }

    public enum PopUpTextType
    {
        DiscardPlayerCard,
        DiscardEnemyCard,
        BurnPlayerCard,
        BurnEnemyCard,
        DiscardAnyPlayerCards
    }
}
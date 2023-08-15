using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

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

    public GameObject CardSacrPopUp;
    public GameObject CardDiscPopUp;

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

        cardManagerScript = CardManager.GetComponent<CardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        drawCounter.text = "" + cardManagerScript.cards.Count;
        enemyDrawCounter.text = "" + cardManagerScript.enemyCards.Count;
        discardCounter.text = "" + cardManagerScript.discardedCards.Count;
        enemyDiscardCounter.text = "" + cardManagerScript.discardedEnemyCards.Count;

        healthPoints.text = "" + cardManagerScript.player.Hp;
        enemyHealthPoints.text = "" + cardManagerScript.enemy.Hp;
        blockPoints.text = "" + cardManagerScript.player.Block;
        enemyBlockPoints.text = "" + cardManagerScript.enemy.Block;

        strengthCounterText.text = "" + cardManagerScript.player.Strength;
        enemyStrengthCounterText.text = "" + cardManagerScript.enemy.Strength;

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
    }
}

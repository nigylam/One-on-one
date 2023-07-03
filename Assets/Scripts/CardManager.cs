using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject enemyCard1;
    public GameObject enemyCard2;
    public GameObject enemyCard3;
    public GameObject PlayingArea;
    public GameObject Canvas;

    public GameObject drawPile;
    public GameObject enemyDrawPile;
    public GameObject discardPile;
    public GameObject enemyDiscardPile;

    public GameObject healthPointsCounter;
    public GameObject enemyHealthPointsCounter;
    public GameObject blockPointsCounter;
    public GameObject enemyBlockPointsCounter;

    int drawPileCounter = 5;
    int enemyDrawPileCounter = 5;
    int discardPileCounter = 0;
    int enemyDiscardPileCounter = 0;
    public int hp = 30;
    public int enemyHp = 30;
    public int block = 0;
    public int enemyBlock = 5;

    List<GameObject> cards = new List<GameObject>();
    List<GameObject> enemyCards = new List<GameObject>();

    TextMeshProUGUI drawCounter;
    TextMeshProUGUI enemyDrawCounter;
    TextMeshProUGUI discardCounter;
    TextMeshProUGUI enemyDiscardCounter;

    TextMeshProUGUI healthPoints;
    TextMeshProUGUI enemyHealthPoints;
    TextMeshProUGUI blockPoints;
    TextMeshProUGUI enemyBlockPoints;

    void Start()
    {
        cards.Add(card1); cards.Add(card2);
        enemyCards.Add(enemyCard1); enemyCards.Add(enemyCard2); enemyCards.Add(enemyCard3);

        drawCounter = drawPile.GetComponent<TextMeshProUGUI>();
        enemyDrawCounter = enemyDrawPile.GetComponent<TextMeshProUGUI>();
        discardCounter = discardPile.GetComponent<TextMeshProUGUI>();
        enemyDiscardCounter = enemyDiscardPile.GetComponent<TextMeshProUGUI>();

        healthPoints = healthPointsCounter.GetComponent<TextMeshProUGUI>();
        enemyHealthPoints = enemyHealthPointsCounter.GetComponent<TextMeshProUGUI>();
        blockPoints = blockPointsCounter.GetComponent<TextMeshProUGUI>();
        enemyBlockPoints = enemyBlockPointsCounter.GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator DrawingCard()
    {
            if (Input.GetKey(KeyCode.Space))
            {
                for (int i = 0, n = -255; i < 2; i++, n += 105)
                    {
                        //GameObject card = Instantiate(cards[0], new Vector2(0, 0), Quaternion.identity);
                        //card.transform.SetParent(Canvas.transform, false);
                        GameObject card = cards[i];
                        CardScript grid = card.GetComponent<CardScript>();

                        if (Time.time >= grid.timestamp)
                        {
                            grid.startPosition = card.transform.localPosition;
                            grid.desiredPosition = new Vector2(n, -370);
                            grid.timestamp = Time.time + grid.timeBetweenMoves;
                            drawPileCounter--;
                        }
                        grid.startPosition = grid.desiredPosition;

                        yield return new WaitForSeconds(.3f);
                    //Debug.Log(i);
                    }
                for (int i = 0, n = -255; i < 3; i++, n += 105)
                    {
                        GameObject card = enemyCards[i];
                        //GameObject card = Instantiate(enemyCards[i], new Vector2(0, 0), Quaternion.identity);
                        //card.transform.SetParent(Canvas.transform, false);
                        EnemyCardScript grid = card.GetComponent<EnemyCardScript>();

                        if (Time.time >= grid.timestamp)
                        {
                            grid.startPosition = card.transform.localPosition;
                            grid.desiredPosition = new Vector2(n, 370);
                            grid.timestamp = Time.time + grid.timeBetweenMoves;
                            enemyDrawPileCounter--;
                        }
                        grid.startPosition = grid.desiredPosition;

                yield return new WaitForSeconds(.3f);
                }
                
            }
    }

    public IEnumerator DiscardingCard()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            for (int i = 4; i >= 0; i--)
            {
                GameObject card = cards[i];

                if (Time.time >= card.GetComponent<CardScript>().timestamp)
                {
                    card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                    card.GetComponent<CardScript>().desiredPosition = new Vector2(1150, -670);
                    card.GetComponent<CardScript>().timestamp = Time.time + card.GetComponent<CardScript>().timeBetweenMoves;
                    discardPileCounter++;
                }
                card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                yield return new WaitForSeconds(.3f);
            }

            for (int i = 4; i >= 0; i--)
            {
                GameObject card = enemyCards[i];

                if (Time.time >= card.GetComponent<EnemyCardScript>().timestamp)
                {
                    card.GetComponent<EnemyCardScript>().startPosition = card.transform.localPosition;
                    card.GetComponent<EnemyCardScript>().desiredPosition = new Vector2(1150, 670);
                    card.GetComponent<EnemyCardScript>().timestamp = Time.time + card.GetComponent<EnemyCardScript>().timeBetweenMoves;
                    enemyDiscardPileCounter++;
                }
                card.GetComponent<EnemyCardScript>().startPosition = card.transform.localPosition;
                yield return new WaitForSeconds(.3f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DrawingCard());
        StartCoroutine(DiscardingCard());
        drawCounter.text = "" + drawPileCounter;
        enemyDrawCounter.text = "" + enemyDrawPileCounter;
        discardCounter.text = "" + discardPileCounter;
        enemyDiscardCounter.text = "" + enemyDiscardPileCounter;

        healthPoints.text = "" + hp;
        enemyHealthPoints.text = "" + enemyHp;
        blockPoints.text = "" + block;
        enemyBlockPoints.text = "" + enemyBlock;

        if (enemyBlock < 0)
        {
            enemyBlock = 0;
        }
        if (block < 0)
        {
            block = 0;
        }
    }
}

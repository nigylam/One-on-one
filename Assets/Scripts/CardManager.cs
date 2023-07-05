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

    public GameObject StatManager;
    StatManager statManagerScript;

    List<GameObject> cards = new List<GameObject>();
    List<GameObject> enemyCards = new List<GameObject>();

    void Start()
    {
        cards.Add(card1); cards.Add(card2);
        enemyCards.Add(enemyCard1); enemyCards.Add(enemyCard2); enemyCards.Add(enemyCard3);
        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
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
                            statManagerScript.drawPileCounter--;
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
                            statManagerScript.enemyDrawPileCounter--;
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
            for (int i = (cards.Count); i > 0; i--)
            {
                GameObject card = cards[i-1];

                if (Time.time >= card.GetComponent<CardScript>().timestamp)
                {
                    card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                    card.GetComponent<CardScript>().desiredPosition = new Vector2(1150, -670);
                    card.GetComponent<CardScript>().timestamp = Time.time + card.GetComponent<CardScript>().timeBetweenMoves;
                    statManagerScript.discardPileCounter++;
                }
                card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                yield return new WaitForSeconds(.3f);
            }

            for (int i = (enemyCards.Count); i > 1; i--)
            {
                GameObject card = enemyCards[i-1];

                if (Time.time >= card.GetComponent<EnemyCardScript>().timestamp)
                {
                    card.GetComponent<EnemyCardScript>().startPosition = card.transform.localPosition;
                    card.GetComponent<EnemyCardScript>().desiredPosition = new Vector2(1150, 670);
                    card.GetComponent<EnemyCardScript>().timestamp = Time.time + card.GetComponent<EnemyCardScript>().timeBetweenMoves;
                    statManagerScript.enemyDiscardPileCounter++;
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public GameObject PlayingArea;
    public GameObject Canvas;
    private SpriteRenderer sprite;

    public GameObject StatManager;
    StatManager statManagerScript;

    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> enemyCards = new List<GameObject>();
    public List<GameObject> discardedCards = new List<GameObject>();
    public List<GameObject> discardedEnemyCards = new List<GameObject>();
    List<GameObject> cardsOnTheTable = new List<GameObject>();
    List<GameObject> enemyCardsOnTheTable = new List<GameObject>();

    void Start()
    {
        //cards.Add(card1); cards.Add(card2);
        //enemyCards.Add(enemyCard1); enemyCards.Add(enemyCard2); enemyCards.Add(enemyCard3);
        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
        ShufflingDeck(true, true);
        ShufflingDeck(false, true);
        StartCoroutine(DrawingCard(5, true, 2));
        StartCoroutine(DrawingCard(5, false, 3));
    }

    public IEnumerator DrawingCard(int amountOfCards, bool isPlayer, float pauseTime = 0)
    {
        yield return new WaitForSeconds(pauseTime);
        if (isPlayer)
        {
            for (int i = 0, n = -255; i < amountOfCards; i++, n += 105)
            {
                if (cards.Count == 0) { ShufflingDeck(true); }

                //int randomCard = Random.Range(0, cards.Count);
                GameObject card = cards[i];
                cards.RemoveAt(i);
                card.transform.localPosition = new Vector2(-1114, -716);
                CardScript grid = card.GetComponent<CardScript>();
                sprite = card.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = i;
                cardsOnTheTable.Add(card);

                if (Time.time >= grid.timestamp)
                {
                    grid.startPosition = card.transform.localPosition;
                    grid.desiredPosition = new Vector2(n, -370);
                    grid.timestamp = Time.time + grid.timeBetweenMoves;
                }
                grid.startPosition = grid.desiredPosition;
                yield return new WaitForSeconds(.3f);
            }
        }
        else
        {
            for (int i = 0, n = -255; i < amountOfCards; i++, n += 105)
            {
                if (enemyCards.Count == 0) { ShufflingDeck(true); }

                GameObject card = enemyCards[i];
                enemyCards.RemoveAt(i);
                card.transform.localPosition = new Vector2(-1114, 716);
                CardScript grid = card.GetComponent<CardScript>();
                sprite = card.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = i;
                enemyCardsOnTheTable.Add(card);

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

    public IEnumerator DiscardingCard(bool isPlayer)
    {
        if (isPlayer)
        {
            for (int i = cardsOnTheTable.Count; i > 0; i--)
            {
                GameObject card = cardsOnTheTable[i - 1];
                if (Time.time >= card.GetComponent<CardScript>().timestamp)
                {
                    card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                    card.GetComponent<CardScript>().desiredPosition = new Vector2(1150, -670);
                    card.GetComponent<CardScript>().timestamp = Time.time + card.GetComponent<CardScript>().timeBetweenMoves;
                    //statManagerScript.discardPileCounter++;
                }
                card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                discardedCards.Add(card);
                yield return new WaitForSeconds(.2f);
                //Destroy(card);
            }
            cardsOnTheTable.Clear();
            yield return new WaitForSeconds(1f);
        }
        else
        {
            for (int i = enemyCardsOnTheTable.Count; i > 0; i--)
            {
                GameObject card = enemyCardsOnTheTable[i - 1];
                if (Time.time >= card.GetComponent<CardScript>().timestamp)
                {
                    card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                    card.GetComponent<CardScript>().desiredPosition = new Vector2(1150, 670);
                    card.GetComponent<CardScript>().timestamp = Time.time + card.GetComponent<CardScript>().timeBetweenMoves;
                    statManagerScript.discardPileCounter++;
                }
                card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                yield return new WaitForSeconds(.3f);
            }
        }
    }

    public void ShufflingDeck(bool isPlayer, bool isThisAStartOfBattle = false)
    {   
        if (isPlayer)
        {
            if (isThisAStartOfBattle == false)
            {
                foreach (GameObject card in discardedCards)
                {
                    cards.Add(card);
                    //discardedCards.Remove(card);

                }
                discardedCards.Clear();
            }

            for (int i = 0; i < cards.Count; i++)
            {
                GameObject temp = cards[i];
                int randomIndex = Random.Range(i, cards.Count);
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
        else
        {
            if (isThisAStartOfBattle == false)
            {
                foreach (GameObject card in discardedEnemyCards)
                {
                    enemyCards.Add(card);
                    //discardedCards.Remove(card);

                }
                discardedEnemyCards.Clear();
            }

            for (int i = 0; i < enemyCards.Count; i++)
            {
                GameObject temp = cards[i];
                int randomIndex = Random.Range(i, cards.Count);
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}

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

    public Side player;
    public Side enemy;

    void Start()
    {
        player = new Side(new Vector2(-1114, -716), cards, cardsOnTheTable, discardedCards, -370, new Vector2(1150, -700));
        enemy = new Side(new Vector2(-1114, 716), enemyCards, enemyCardsOnTheTable, discardedEnemyCards, 370, new Vector2(1150, 700));

        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
        //ShufflingDeck(player, true);
        //ShufflingDeck(enemy, true);
        StartCoroutine(DrawingCard(player, 5, 2f));
        StartCoroutine(DrawingCard(enemy, 5, 3f));
    }

    public IEnumerator DrawingCard(Side side, int amountOfCards = 5, float pauseTime = 0)
    {
        yield return new WaitForSeconds(pauseTime);
        int cardsForRemoving = 0;
        while (side.TableCards.Count < amountOfCards)
        {
            if (side.Cards.Count == 0)
            {
                ShufflingDeck(side);
            }

            GameObject card = side.Cards[0]; // Get the first card in the list

            CardScript grid = card.GetComponent<CardScript>();
            grid.isDragging = false;
            card.transform.localPosition = side.StartPosition;
            sprite = card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardsForRemoving; cardsForRemoving++;
            side.TableCards.Add(card);
            grid.startPosition = card.transform.localPosition;
            grid.desiredPosition = new Vector2(cardsForRemoving * 105 - 355, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;

            // Remove the card from the original list after processing it
            side.Cards.RemoveAt(0);

            yield return new WaitForSecondsRealtime(.3f);
        }

        /*
        int cardsForRemoving = 0;
        List<GameObject> cardsCopy = new List<GameObject>(side.Cards); // Create a copy of the list

        foreach (GameObject card in cardsCopy)
        {
            if (side.TableCards.Count == amountOfCards) { break; }
            if (side.Cards.Count == 0)
            {
                Debug.Log("happens");
                ShufflingDeck(side);
                cardsCopy = new List<GameObject>(side.Cards); // isnt working
            }

            CardScript grid = card.GetComponent<CardScript>();
            grid.isDragging = false;
            card.transform.localPosition = side.StartPosition;
            sprite = card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardsForRemoving; cardsForRemoving++;
            side.TableCards.Add(card);
            grid.startPosition = card.transform.localPosition;
            grid.desiredPosition = new Vector2(cardsForRemoving * 105 - 255, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;

            // Remove the card from the original list after processing it
            side.Cards.Remove(card);

            yield return new WaitForSecondsRealtime(.3f);
        }





        
        int cardsForRemoving = 0;
        int i = 0;
        int n = -255;
        foreach (GameObject card in side.Cards)
        {
            if(side.TableCards.Count == amountOfCards) { break; }
            //if()

            CardScript grid = card.GetComponent<CardScript>();
            grid.isDragging = false;
            card.transform.localPosition = side.StartPosition;
            sprite = card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = i; i++;
            side.TableCards.Add(card);
            grid.startPosition = card.transform.localPosition;
            grid.desiredPosition = new Vector2(n, side.HandPosition); n += 105;
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
            //cardsForRemoving++;
            side.Cards.Remove(card);
            yield return new WaitForSeconds(.3f);
        }
        

        if (side.Cards.Count < amountOfCards)
        {
            for (int n = -255, i = 0; i < side.Cards.Count; i++, n += 105)
            {
                GameObject card = side.Cards[i];
                CardScript grid = card.GetComponent<CardScript>();
                grid.isDragging = false;
                card.transform.localPosition = side.StartPosition;
                sprite = card.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = i;
                side.TableCards.Add(card);

                //if (Time.time >= grid.timestamp)
                //{
                grid.startPosition = card.transform.localPosition;
                grid.desiredPosition = new Vector2(n, side.HandPosition);
                grid.timestamp = Time.time + grid.timeBetweenMoves;
                //}
                cardsForRemoving = i;
                grid.startPosition = grid.desiredPosition;
                yield return new WaitForSeconds(.3f);
            }
            amountOfCards -= side.Cards.Count;
            side.Cards.Clear();
            for (int n = -255, i = 0; i < side.Cards.Count; i++, n += 105)
            {
                GameObject card = side.Cards[i];
                CardScript grid = card.GetComponent<CardScript>();
                grid.isDragging = false;
                card.transform.localPosition = side.StartPosition;
                sprite = card.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = i;
                side.TableCards.Add(card);

                //if (Time.time >= grid.timestamp)
                //{
                grid.startPosition = card.transform.localPosition;
                grid.desiredPosition = new Vector2(n, side.HandPosition);
                grid.timestamp = Time.time + grid.timeBetweenMoves;
                //}
                cardsForRemoving = i;
                grid.startPosition = grid.desiredPosition;
                yield return new WaitForSeconds(.3f);
            }
        } else
        {
            for (int n = -255, i = 0; i < amountOfCards; i++, n += 105)
            {
                GameObject card = side.Cards[i];
                CardScript grid = card.GetComponent<CardScript>();
                grid.isDragging = false;
                card.transform.localPosition = side.StartPosition;
                sprite = card.GetComponent<SpriteRenderer>();
                sprite.sortingOrder = i;
                side.TableCards.Add(card);

                //if (Time.time >= grid.timestamp)
                //{
                grid.startPosition = card.transform.localPosition;
                grid.desiredPosition = new Vector2(n, side.HandPosition);
                grid.timestamp = Time.time + grid.timeBetweenMoves;
                //}
                cardsForRemoving = i;
                grid.startPosition = grid.desiredPosition;
                yield return new WaitForSeconds(.3f);
            }
            side.Cards.RemoveRange(0, cardsForRemoving + 1);
        }*/

    }

    public IEnumerator DiscardingCard(Side side)
    {
        for (int i = side.TableCards.Count; i > 0; i--)
        {
            GameObject card = side.TableCards[i - 1];
            if (Time.time >= card.GetComponent<CardScript>().timestamp)
            {
                card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
                card.GetComponent<CardScript>().desiredPosition = side.DiscardPosition;
                card.GetComponent<CardScript>().timestamp = Time.time + card.GetComponent<CardScript>().timeBetweenMoves;
                //statManagerScript.discardPileCounter++;
            }
            card.GetComponent<CardScript>().startPosition = card.transform.localPosition;
            side.DiscardedCards.Add(card);
            yield return new WaitForSeconds(.2f);
            //Destroy(card);
        }
        side.TableCards.Clear();
        yield return new WaitForSeconds(1f);
    }

    public void ShufflingDeck(Side side, bool isThisStartOfBattle = false)
    {
        if (isThisStartOfBattle == false)
        {
            foreach (GameObject card in side.DiscardedCards)
            {
                side.Cards.Add(card);
                //discardedCards.Remove(card);

            }
            side.DiscardedCards.Clear();
        }

        for (int i = 0; i < side.Cards.Count; i++)
        {
            GameObject temp = side.Cards[i];
            int randomIndex = Random.Range(i, side.Cards.Count);
            side.Cards[i] = side.Cards[randomIndex];
            side.Cards[randomIndex] = temp;
        }
    }
}

public class Side
{
    public Vector2 StartPosition;
    public List<GameObject> Cards;
    public List<GameObject> TableCards;
    public List<GameObject> DiscardedCards;
    public int HandPosition;
    public Vector2 DiscardPosition;

    public Side(Vector2 startPosition, List<GameObject> cards, List<GameObject> tableCards, List<GameObject> discardedCards, int handPosition, Vector2 discardPosition)
    {
        StartPosition = startPosition;
        Cards = cards;
        TableCards = tableCards;
        DiscardedCards = discardedCards;
        HandPosition = handPosition;
        DiscardPosition = discardPosition;
    }
}



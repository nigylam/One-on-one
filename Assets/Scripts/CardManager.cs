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
    public List<GameObject> cardsOnTheTable = new List<GameObject>();
    public List<GameObject> enemyCardsOnTheTable = new List<GameObject>();
    public List<GameObject> enemyBurnedCards = new List<GameObject>();

    public Side player;
    public Side enemy;

    public bool SacrificeMode;

    void Start()
    {
        player = new Side(new Vector2(-1114, -716), cards, cardsOnTheTable, discardedCards, -370, new Vector2(1150, -700));
        enemy = new Side(new Vector2(-1114, 716), enemyCards, enemyCardsOnTheTable, discardedEnemyCards, 370, new Vector2(1150, 700));

        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
        ShufflingDeck(player, true);
        ShufflingDeck(enemy, true);
        StartCoroutine(DrawingCard(player, 5, 2f));
        StartCoroutine(DrawingCard(enemy, 5, 3f));

        SacrificeMode = false;
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
            card.transform.localPosition = side.StartPosition;
            side.TableCards.Add(card);
            side.Cards.RemoveAt(0);

            CalculateCardPosition(side);
            cardsForRemoving++;

            yield return new WaitForSecondsRealtime(.3f);
        }
    }

    public void CalculateCardPosition(Side side)
    {
        int middleIndex = (side.TableCards.Count - 1) / 2;
        int offset = 105; // Adjust this value based on your desired spacing between cards
        float totalWidth = (side.TableCards.Count - 1) * offset;
        float halfTotalWidth = totalWidth / 2;

        for (int cardIndex = 0; cardIndex < side.TableCards.Count; cardIndex++)
        {
            GameObject card = side.TableCards[cardIndex];
            CardScript grid = card.GetComponent<CardScript>();
            grid.isDragging = false;
            //card.transform.localPosition = side.StartPosition;
            sprite = card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;

            // For the first card, we want it at X = -(halfTotalWidth) + (0 * offset) = -halfTotalWidth
            // For the middle card, we want it at X = 0
            // For the last card, we want it at X = halfTotalWidth - (totalWidth * offset) = halfTotalWidth
            float desiredX = -halfTotalWidth + cardIndex * offset;

            grid.startPosition = card.transform.localPosition;
            grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
        }
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
    void Update()
    {

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


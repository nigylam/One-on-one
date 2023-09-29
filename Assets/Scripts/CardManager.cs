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

    //public int StrTest = 1;

    public GameObject StatManager;
    StatManager statManagerScript;

    public CardData cardData;

    //side lists
    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> enemyCards = new List<GameObject>();
    public List<GameObject> discardedCards = new List<GameObject>();
    public List<GameObject> discardedEnemyCards = new List<GameObject>();
    public List<GameObject> cardsOnTheTable = new List<GameObject>();
    public List<GameObject> doubleCardsOnTheTable = new List<GameObject>();
    public List<GameObject> enemyCardsOnTheTable = new List<GameObject>();
    public List<GameObject> doubleEnemyCardsOnTheTable = new List<GameObject>();
    public List<GameObject> enemyBurnedCards = new List<GameObject>();
    public List<GameObject> burnedCards = new List<GameObject>();
    public List<GameObject> playingCards = new List<GameObject>();
    
    // common lists
    public List<GameObject> displayCards = new List<GameObject>();

    public Side player;
    public Side enemy;

    public bool popupMode = false;
    public bool discardMode;
    public bool burnMode;

    bool drawCompleted = false;

    //public bool DiscardMode;
    bool shuffledComplete = true;

    void Awake()
    {
        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
        player = new Side(new Vector2(-1114, -716), cards, cardsOnTheTable, doubleCardsOnTheTable, discardedCards, burnedCards, -370, new Vector2(1150, -700), 4, statManagerScript.hp, 50, statManagerScript.CardDiscPopUp, 0);
        enemy = new Side(new Vector2(-1114, 716), enemyCards, enemyCardsOnTheTable, doubleEnemyCardsOnTheTable, discardedEnemyCards, enemyBurnedCards, 370, new Vector2(1150, 700), statManagerScript.enemyBlock, statManagerScript.enemyHp, -50, statManagerScript.CardSacrPopUp, 0);

        cardData = gameObject.GetComponent<CardData>();
    }

    void Start()
    {
        enemy.DrawCard(5);
        player.DrawCard(5);
    }

    void Update()
    {
        CalculateCardPosition();
    }

    public void CalculateCardPosition()
    {
        Side[] sides = new[] { player, enemy };
        foreach (Side side in sides)
        {
            if (side.CardsOnTheTableCounter != side.TableCards.Count)
            {
                int middleIndex = (side.TableCards.Count - 1) / 2;
                int offset = 105;
                float totalWidth = (side.TableCards.Count - 1) * offset;
                float halfTotalWidth = totalWidth / 2;

                for (int cardIndex = 0; cardIndex < side.TableCards.Count; cardIndex++)
                {
                    GameObject card = side.TableCards[cardIndex];
                    CardScript grid = card.GetComponent<CardScript>();
                    //grid.isDragging = false;
                    sprite = card.GetComponent<SpriteRenderer>();
                    sprite.sortingOrder = cardIndex;
                    float desiredX = -halfTotalWidth + cardIndex * offset;

                    grid.startPosition = card.transform.localPosition;
                    grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
                    grid.timestamp = Time.time + grid.timeBetweenMoves;
                    grid.startPosition = grid.desiredPosition;
                }
            }
            side.CardsOnTheTableCounter = side.TableCards.Count;
        }
    }

    public IEnumerator CalculateTableCards()
    {
        Side[] sides = new[] { player, enemy };
        foreach (Side side in sides)
        {
            if (side.DoubleTableCards.Count < side.TableCards.Count)
            {

            }
        }
    }

    /* 
     public IEnumerator CalculateCardPosition()
     {
         Side[] sides = new[] { player, enemy };
         foreach (Side side in sides)
         {
             if (side.CardsOnTheTableCounter != side.TableCards.Count)
             {

                 yield return new WaitForSecondsRealtime(.3f);
                 int missingCards = side.TableCards.Count - side.CardsOnTheTableCounter;
                 side.CardsOnTheTableCounter = side.TableCards.Count;
                 List<GameObject> TableCards = new List<GameObject>();

                 if (missingCards > 0)
                 {
                     for (int i = 0; i < missingCards; i++)
                     {
                         TableCards.Add(side.TableCards[side.TableCards.Count - 1 - i]);

                         int middleIndex = (TableCards.Count - 1) / 2;
                         int offset = 125;
                         float totalWidth = (TableCards.Count - 1) * offset;
                         float halfTotalWidth = totalWidth / 2;

                         for (int cardIndex = 0; cardIndex < TableCards.Count; cardIndex++)
                         {
                             GameObject card = TableCards[cardIndex];
                             CardScript grid = card.GetComponent<CardScript>();
                             //grid.isDragging = false;
                             sprite = card.GetComponent<SpriteRenderer>();
                             sprite.sortingOrder = cardIndex;
                             float desiredX = -halfTotalWidth + cardIndex * offset;

                             //grid.startPosition = card.transform.localPosition;
                             grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
                             grid.timestamp = Time.time + grid.timeBetweenMoves;
                             grid.startPosition = grid.desiredPosition;
                         }

                         yield return new WaitForSecondsRealtime(.3f);
                         if (i == missingCards - 1) { drawCompleted = true; }
                     }
                 }

             }
             yield return new WaitUntil(() => drawCompleted);
             drawCompleted = false;
         }
     } */

    public IEnumerator DrawingCard(Side side, int amountOfCards = 5, float pauseTime = 0)
    {
        yield return new WaitForSeconds(pauseTime);
        int i = side.TableCards.Count;
        while (side.TableCards.Count < amountOfCards + i)
        {
            if (side.Cards.Count == 0)
            {
                shuffledComplete = false;
                StartCoroutine(ShufflingDeck(side));
                yield return new WaitUntil(() => shuffledComplete == true);
            }

            GameObject card = side.Cards[0];
            //card.transform.localPosition = side.StartPosition;
            side.TableCards.Add(card);
            side.Cards.RemoveAt(0);

            yield return new WaitForSecondsRealtime(.3f);
        }
    }

    public IEnumerator DiscardingCard(Side side)
    {
        for (int i = side.TableCards.Count; i > 0; i--)
        {
            GameObject card = side.TableCards[i - 1];
            side.DiscardedCards.Add(card);
            yield return new WaitForSeconds(.2f);
        }
        side.TableCards.Clear();
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator ShufflingDeck(Side side, bool isThisStartOfBattle = false)
    {
        if (isThisStartOfBattle == false)
        {
            while (side.DiscardedCards.Count > 0)
            {

                GameObject card = side.DiscardedCards[0];
                side.Cards.Add(card);
                side.DiscardedCards.RemoveAt(0);
            }
        }

        for (int i = 0; i < side.Cards.Count; i++)
        {
            GameObject temp = side.Cards[i];
            int randomIndex = Random.Range(i, side.Cards.Count);
            side.Cards[i] = side.Cards[randomIndex];
            side.Cards[randomIndex] = temp;
        }
        yield return new WaitForSeconds(1f);
        shuffledComplete = true;
    }

}


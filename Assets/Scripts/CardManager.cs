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

    public CardData cardData;

    public List<GameObject> cards = new List<GameObject>();
    public List<GameObject> enemyCards = new List<GameObject>();
    public List<GameObject> discardedCards = new List<GameObject>();
    public List<GameObject> discardedEnemyCards = new List<GameObject>();
    public List<GameObject> cardsOnTheTable = new List<GameObject>();
    public List<GameObject> enemyCardsOnTheTable = new List<GameObject>();
    public List<GameObject> enemyBurnedCards = new List<GameObject>();
    public List<GameObject> playingCards = new List<GameObject>();

    public Side player;
    public Side enemy;

    public bool manaSpendingMode;
    //public bool DiscardMode;
    bool shuffledComplete = true;

    void Awake()
    {
        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
        player = new Side(new Vector2(-1114, -716), cards, cardsOnTheTable, discardedCards, -370, new Vector2(1150, -700), 4, statManagerScript.hp, 50, statManagerScript.CardDiscPopUp, 0);
        enemy = new Side(new Vector2(-1114, 716), enemyCards, enemyCardsOnTheTable, discardedEnemyCards, 370, new Vector2(1150, 700), statManagerScript.enemyBlock, statManagerScript.enemyHp, -50, statManagerScript.CardSacrPopUp, 0);

        cardData = gameObject.GetComponent<CardData>();
    }

    void Start()
    {

        StartCoroutine(ShufflingDeck(player, true));
        StartCoroutine(ShufflingDeck(enemy, true));
        StartCoroutine(DrawingCard(player, 5, 2f));
        StartCoroutine(DrawingCard(enemy, 5, 3f));
    }

    void Update()
    {
        CalculateCardPosition(player);
        CalculateCardPosition(enemy);
    }

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

    public void CalculateCardPosition(Side side)
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

public class Side
{
    public Vector2 StartPosition;
    public List<GameObject> Cards;
    public List<GameObject> TableCards;
    public List<GameObject> DiscardedCards;
    public int HandPosition;
    public Vector2 DiscardPosition;
    private int block;
    public int Hp;
    public int HiglightPosition;
    public int CardsOnTheTableCounter = 0;
    public GameObject ManaPopUp;
    public int Strength;

    public Side(Vector2 startPosition, List<GameObject> cards, List<GameObject> tableCards, List<GameObject> discardedCards, int handPosition, Vector2 discardPosition, int block, int hp, int higlightPosition, GameObject manaPopUp, int strength)
    {
        StartPosition = startPosition;
        Cards = cards;
        TableCards = tableCards;
        DiscardedCards = discardedCards;
        HandPosition = handPosition;
        DiscardPosition = discardPosition;
        Block = block;
        Hp = hp;
        HiglightPosition = higlightPosition;
        ManaPopUp = manaPopUp;
        Strength = strength;

    }
    public int Block
    {
        get => block;
        set
        {
            block = Mathf.Max(0, value);
        }
    }

    public int DealDamage(int damage)
    {
        if (Block >= damage)
        {
            Block -= damage;
        }
        else
        {
            Hp -= (damage - Block);
            Block = 0;
        }

        return Hp;
    }
}


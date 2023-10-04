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

    bool drawCompleted = true;
    int drawHowTimes = 0;

    bool shuffledComplete = true;

    void Awake()
    {
        statManagerScript = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        player = GameObject.Find("Player Side").GetComponent<Side>();
        player.Hp = statManagerScript.hp;

        enemy = GameObject.Find("Enemy Side").GetComponent<Side>();
        enemy.Hp = statManagerScript.enemyHp;

        cardData = gameObject.GetComponent<CardData>();
    }

    void Start()
    {
        enemy.DrawCard(5);
        player.DrawCard(4);
        Debug.Log(5 / 2);
    }

    void Update()
    {
    }

    public void CallAnimation(string type, GameObject objectAnimation = null)
    {
        if (type == "draw") { StartCoroutine(CalculateCardPosition(objectAnimation)); }
        else if (type == "discard") { StartCoroutine(CalculateCardPositionDiscarding(objectAnimation)); }
        else if (type == "shuffle") { StartCoroutine(ShuffleCards()); }

    }

    public IEnumerator ShuffleCards()
    {
        while (!drawCompleted)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        drawCompleted = false;

    }

    public IEnumerator CalculateCardPosition(GameObject card)
    {
        while (!drawCompleted)
        {
            yield return null;
        }
        drawCompleted = false;
        Side side;
        if (player.TableCards.Contains(card)) { side = player; }
        else { side = enemy; }

        side.DoubleTableCards.Add(card);
        int offset = 105;
        int width = (side.DoubleTableCards.Count-1) * offset;
        int halfWidth = width / 2;
        int startX = 0 - halfWidth;
        for (int cardIndex = 0; cardIndex < side.DoubleTableCards.Count; cardIndex++)
        {
            GameObject Card = side.DoubleTableCards[cardIndex];
            CardScript grid = Card.GetComponent<CardScript>();
            sprite = Card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;
            float desiredX = startX + (offset * cardIndex);
            //grid.startPosition = Card.transform.localPosition;
            grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
        }
        yield return new WaitForSeconds(0.3f);
        drawCompleted = true;
    }

    public IEnumerator CalculateCardPositionDiscarding(GameObject card)
    {
        while (!drawCompleted)
        {
            yield return null; // Wait for the next frame.
        }

        drawCompleted = false;

        Side side;
        if (player.DiscardedCards.Contains(card)) { side = player; }
        else { side = enemy; }

        side.DoubleTableCards.Remove(card);
        int offset = 105;
        int width = (side.DoubleTableCards.Count - 1) * offset;
        int halfWidth = width / 2;
        int startX = 0 - halfWidth;
        for (int cardIndex = 0; cardIndex < side.DoubleTableCards.Count; cardIndex++)
        {
            GameObject Card = side.DoubleTableCards[cardIndex];
            CardScript grid = Card.GetComponent<CardScript>();
            sprite = Card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;
            float desiredX = startX + (offset * cardIndex);
            //grid.startPosition = Card.transform.localPosition;
            grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
        }
        yield return new WaitForSeconds(0.3f);
        drawCompleted = true;
    }

}


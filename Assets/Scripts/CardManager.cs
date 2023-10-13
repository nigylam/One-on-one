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

    int tableCards = 0;

    public int curentTurn = 1;
    //int drawHowTimes = 0;
    //bool shuffledComplete = true;


    // new code for events
    private Queue<CardEvent> cardEvents = new Queue<CardEvent>();

    private Queue<CardEvent> eventQueue = new Queue<CardEvent>();
    private bool isProcessingQueue = false;

    void Awake()
    {
        statManagerScript = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        player = GameObject.Find("Player Side").GetComponent<Side>();
        player.Hp = statManagerScript.hp;

        enemy = GameObject.Find("Enemy Side").GetComponent<Side>();
        enemy.Hp = statManagerScript.enemyHp;

        cardData = gameObject.GetComponent<CardData>();

        player.CardAction += OnCardAction;
        //player.CardDiscarded += OnCardDiscarded;

        enemy.CardAction += OnCardAction;
        //enemy.CardDiscarded += OnCardDiscarded;
    }

    void Start()
    {
        enemy.DrawCards();
        player.DrawCards();
    }

    void Update()
    {
        if (!isProcessingQueue && eventQueue.Count > 0)
        {
            StartCoroutine(ProcessEventQueue());
        }
        ProcessCardEvents();
    }

    IEnumerator ProcessEventQueue()
    {
        isProcessingQueue = true;
        CardEvent cardEvent = eventQueue.Dequeue();
        
        if (cardEvent.ActionType == CardActionType.Draw)
        {
            PlayDrawAnimation(cardEvent.Card, cardEvent.TriggeringSide);
        }
        else if (cardEvent.ActionType == CardActionType.Discard)
        {
            PlayDiscardAnimation(cardEvent.Card, cardEvent.TriggeringSide);
        }
        else if (cardEvent.ActionType == CardActionType.Shuffle)
        {
            PlayShuffleAnimation(cardEvent.TriggeringSide);
        }
        yield return new WaitForSeconds(0.3f);
        isProcessingQueue = false;
    }

    void OnDestroy()
    {
        player.CardAction -= OnCardAction;
        enemy.CardAction -= OnCardAction;
    }

    void OnCardAction(GameObject card, Side side, CardActionType cardActionType)
    {
        eventQueue.Enqueue(new CardEvent(card, cardActionType, side));
    }

    void PlayDrawAnimation(GameObject card, Side side)
    {
        side.DoubleTableCards.Add(card);
        card.transform.localPosition = side.StartPosition;
        CalculateTableCardsPosition(side);
    }

    void PlayDiscardAnimation(GameObject card, Side side)
    {
        side.DoubleTableCards.Remove(card);
        //if (side.Cards.Contains(card))
        //{ card.transform.localPosition = side.StartPosition; } else { card.transform.localPosition = side.DiscardPosition; }

        card.GetComponent<CardScript>().desiredPosition = side.DiscardPosition;
        //Debug.Log(card.name + " " + card.transform.localPosition);
        CalculateTableCardsPosition(side);
    }
    void PlayShuffleAnimation(Side side)
    {
        foreach(GameObject card in side.Cards)
        {
            card.transform.localPosition = side.StartPosition;
            card.GetComponent<CardScript>().desiredPosition = side.StartPosition;
        }
        foreach (GameObject card in side.DiscardedCards)
        {
            card.transform.localPosition = side.DiscardPosition;
            card.GetComponent<CardScript>().desiredPosition = side.DiscardPosition;
        }
    }

    public void AddCardEvent(GameObject card, CardActionType actionType, Side triggeringSide)
    {
        cardEvents.Enqueue(new CardEvent(card, actionType, triggeringSide));
    }

    public void CalculateTableCardsPosition(Side side)
    {
        int offset = 105;
        int width = (side.DoubleTableCards.Count - 1) * offset;
        int halfWidth = width / 2;
        for (int cardIndex = 0; cardIndex < side.DoubleTableCards.Count; cardIndex++)
        {
            GameObject Card = side.DoubleTableCards[cardIndex];
            CardScript grid = Card.GetComponent<CardScript>();
            sprite = Card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;
            float desiredX = -halfWidth + (offset * cardIndex);
            //grid.startPosition = Card.transform.localPosition;
            grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
        }
    }

    void ProcessCardEvents()
    {
        if (cardEvents.Count > 0)
        {
            CardEvent cardEvent = cardEvents.Dequeue();
            Debug.Log($"Event: {cardEvent.ActionType} triggered by {cardEvent.TriggeringSide}");
            switch (cardEvent.ActionType)
            {
                case CardActionType.Draw:
                    break;
                case CardActionType.Discard:
                    break;
            }
        }
    }
}

public enum CardActionType
{
    Draw,
    Discard,
    Shuffle
    // Add more action types as needed
}

public class CardEvent
{
    public GameObject Card;
    public CardActionType ActionType;
    public Side TriggeringSide;

    public CardEvent(GameObject card, CardActionType actionType, Side triggeringSide)
    {
        Card = card;
        ActionType = actionType;
        TriggeringSide = triggeringSide;
    }
}

/*
 * public void CallAnimation(string type, GameObject objectAnimation = null)
    {
        if (type == "draw") {
            StartCoroutine(CalculateCardPosition(objectAnimation)); 
        }
        else if (type == "discard") { StartCoroutine(CalculateCardPositionDiscarding(objectAnimation)); }
        else if (type == "shuffle") { StartCoroutine(ShuffleCards()); }
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
        CalculateTableCardsPosition(side);
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
        if (player.DiscardedCards.Contains(card)) { side = player; } else { side = enemy; }
        side.DoubleTableCards.Remove(card);
        CalculateTableCardsPosition(side);
        yield return new WaitForSeconds(0.3f);
        drawCompleted = true;
    }

    public void CalculateTableCardsPosition(Side side)
    {
        int offset = 105;
        int width = (side.DoubleTableCards.Count - 1) * offset;
        int halfWidth = width / 2;
        for (int cardIndex = 0; cardIndex < side.DoubleTableCards.Count; cardIndex++)
        {
            GameObject Card = side.DoubleTableCards[cardIndex];
            CardScript grid = Card.GetComponent<CardScript>();
            sprite = Card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;
            float desiredX = -halfWidth + (offset * cardIndex);
            //grid.startPosition = Card.transform.localPosition;
            grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
        }
    }

    public IEnumerator ShuffleCards()
    {
        Side[] sides = new Side[] { player, enemy };
        foreach (Side side in sides)
        {
            while (!drawCompleted)
            {
                yield return null; // Wait for the next frame.
            }
            drawCompleted = false;
            foreach (GameObject card in side.Cards)
            {

                card.transform.localPosition = side.StartPosition;
            }
        }
        yield return new WaitForSeconds(0.3f);
        drawCompleted = true;

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
        //shuffledComplete = true;
    }
*/
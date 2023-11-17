using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using System;

public class CardManager : MonoBehaviour
{
    public GameObject PlayingArea;
    public GameObject Canvas;
    private SpriteRenderer sprite;

    public GameObject StatManager;
    StatManager statManagerScript;

    public CardData cardData;

    public List<GameObject> playingCards = new List<GameObject>();
    public List<GameObject> displayCards = new List<GameObject>();

    public Side player;
    public Side enemy;

    public bool popupMode = false;
    //public bool discardMode;
    //public bool burnMode;

    public int curentTurn = 1;

    private Queue<CardEvent> eventQueue = new Queue<CardEvent>();
    private bool isProcessingQueue = false;

    public Stack<CardEvent> eventStack = new Stack<CardEvent>();

    void Awake()
    {
        statManagerScript = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        player = GameObject.Find("Player Side").GetComponent<Side>();
        player.Hp = statManagerScript.hp;

        enemy = GameObject.Find("Enemy Side").GetComponent<Side>();
        enemy.Hp = statManagerScript.enemyHp;

        cardData = gameObject.GetComponent<CardData>();

        player.CardAction += OnCardAction;
        enemy.CardAction += OnCardAction;
    }

    void Start()
    {
        player.DrawCounter = player.Cards.Count;
        player.DiscardCounter = 0;
        enemy.DrawCounter = enemy.Cards.Count; 
        enemy.DiscardCounter = 0;

        enemy.DrawCards();
        player.DrawCards();

        int i = 5;
        int e = 2;

        //Debug.Log(Math.Ceiling(Convert.ToSingle(i) / Convert.ToSingle(e)));
    }

    void Update()
    {
        if (!isProcessingQueue && eventQueue.Count > 0)
        {
            StartCoroutine(ProcessEventQueue());
        }

        // эти два форича должны быть заменены ивентами
        foreach(GameObject card in playingCards)
        {
            card.GetComponent<CardScript>().desiredPosition = new Vector2(0,0);
        }
    }

    IEnumerator ProcessEventQueue()
    {
        isProcessingQueue = true;
        CardEvent cardEvent = eventQueue.Dequeue();
        if (cardEvent.ActionType == null)
        {
            Debug.Log(cardEvent.Card.name);
            Debug.Log("ActionType component is not found!");
        }
        cardEvent.ActionType.PlayAnimation(cardEvent.Card, cardEvent.TriggeringSide);
        yield return new WaitForSeconds(0.3f);
        isProcessingQueue = false;
    }

    void OnDestroy()
    {
        player.CardAction -= OnCardAction;
        enemy.CardAction -= OnCardAction;
    }

    void OnCardAction(GameObject card, Side side, IPlayable cardActionType)
    {
        eventQueue.Enqueue(new CardEvent(card, cardActionType, side));
        eventStack.Push(new CardEvent(card, cardActionType, side));
    }
}

public interface IPlayable
{
    public void PlayAnimation(GameObject card, Side side);
}

public class AddAnimation : IPlayable
{
    CardManager cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
    public void PlayAnimation(GameObject card, Side side)
    {
        Debug.Log("check");
        cardManager.playingCards.Add(card);
        side.DoubleTableCards.Remove(card);
        CardAnimationUtility.CalculateTableCardsPosition(side);
    }
}

public class DrawAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, Side side)
    {
        side.DoubleTableCards.Add(card);
        card.transform.localPosition = side.StartPosition;
        CardAnimationUtility.CalculateTableCardsPosition(side);
        side.DrawCounter--;
    }
}

public class DiscardAnimation : IPlayable
{
    CardManager cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
    public void PlayAnimation(GameObject card, Side side)
    {
        if (cardManager.playingCards.Contains(card))
            cardManager.playingCards.Remove(card);
        if (side.DoubleTableCards.Contains(card))
            side.DoubleTableCards.Remove(card);
        card.GetComponent<CardScript>().desiredPosition = side.DiscardPosition;
        CardAnimationUtility.CalculateTableCardsPosition(side);
        side.DiscardCounter++;
    }
}

public class ShuffleAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, Side side)
    {
        foreach (GameObject cardD in side.Cards)
        {
            cardD.transform.localPosition = side.StartPosition;
            cardD.GetComponent<CardScript>().desiredPosition = side.StartPosition;
        }
        foreach (GameObject cardD in side.DiscardedCards)
        {
            cardD.transform.localPosition = side.DiscardPosition;
            cardD.GetComponent<CardScript>().desiredPosition = side.DiscardPosition;
        }

        side.DrawCounter += side.DiscardCounter;
        side.DiscardCounter = 0;
    }
}
public class BurnAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, Side side)
    {
        if (side.DoubleTableCards.Contains(card)) { side.DoubleTableCards.Remove(card); }
        card.transform.localPosition = new Vector2(2000, 2000);
        card.GetComponent<CardScript>().isntDragging = false;
        CardAnimationUtility.CalculateTableCardsPosition(side);
    }
}

public class CardEvent
{
    public GameObject Card;
    public IPlayable ActionType;
    public Side TriggeringSide;

    public CardEvent(GameObject card, IPlayable actionType, Side triggeringSide)
    {
        Card = card;
        ActionType = actionType;
        TriggeringSide = triggeringSide;
    }
}

public static class CardAnimationUtility
{
    public static void CalculateTableCardsPosition(Side side)
    {
        int offset = 125;
        int width = (side.DoubleTableCards.Count - 1) * offset;
        int halfWidth = width / 2;
        for (int cardIndex = 0; cardIndex < side.DoubleTableCards.Count; cardIndex++)
        {
            GameObject Card = side.DoubleTableCards[cardIndex];
            CardScript grid = Card.GetComponent<CardScript>();
            SpriteRenderer sprite = Card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;
            float desiredX = -halfWidth + (offset * cardIndex);
            grid.desiredPosition = new Vector2(desiredX, side.HandPosition);
            grid.timestamp = Time.time + grid.timeBetweenMoves;
            grid.startPosition = grid.desiredPosition;
        }
    }
}

public enum CardActionType
{
    Draw,
    Discard,
    Shuffle,
    Burn,
    Playing
}

/*
 *void PlayDrawAnimation(GameObject card, Side side)
    {
        side.DoubleTableCards.Add(card);
        card.transform.localPosition = side.StartPosition;
        CalculateTableCardsPosition(side);
        side.DrawCounter--;
    }

    void PlayDiscardAnimation(GameObject card, Side side)
    {
        side.DoubleTableCards.Remove(card);
        card.GetComponent<CardScript>().desiredPosition = side.DiscardPosition;
        CalculateTableCardsPosition(side);
        side.DiscardCounter++;
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

        side.DrawCounter += side.DiscardCounter;
        side.DiscardCounter = 0;
    }

    void PlayBurnAnimation(GameObject card, Side side)
    {
        if (side.DoubleTableCards.Contains(card)) { side.DoubleTableCards.Remove(card); }
        card.transform.localPosition = new Vector2(2000, 2000);
        card.GetComponent<CardScript>().isntDragging = false;
        CalculateTableCardsPosition(side);
    }
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
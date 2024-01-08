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
    private Queue<StatEvent> statEventQueue = new Queue<StatEvent>();

    private bool isProcessingQueue = false;
    private bool isProcessingStatEventQueue = false;

    public Stack<CardEvent> eventStack = new Stack<CardEvent>();
    public Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

    public Vector2 PlayerStartPosition = new Vector2(-1114, -716);
    public Vector2 PlayerDiscardPosition = new Vector2(1150, -700);

    public Vector2 EnemyStartPosition = new Vector2(-1114, 716);
    public Vector2 EnemyDiscardPosition = new Vector2(1150, 700);
    public int PlayerHandPosition = -370;
    public int EnemyHandPosition = 370;


    PlayerUi enemyUi;
    PlayerUi playerUi;

    public List<GameObject> playerTableCards = new List<GameObject>();
    public List<GameObject> enemyTableCards = new List<GameObject>();


    void Awake()
    {
        enemyUi = new PlayerUi { StartPosition = EnemyStartPosition, DiscardPosition = EnemyDiscardPosition, TableCards = enemyTableCards, HandPosition = EnemyHandPosition, Side = enemy };
        playerUi = new PlayerUi { StartPosition = PlayerStartPosition, DiscardPosition = PlayerDiscardPosition, TableCards = playerTableCards, HandPosition = PlayerHandPosition, Side = player };

        cardDictionary["BlueAttack1"] = new Card { Damage = 5 };
        cardDictionary["BlueAttack2"] = new Card { Mana = 1, Damage = 5, DrawCards = 2 };
        cardDictionary["BlueAttack3"] = new Card { IsSpecial = true, Damage = 9, Mana = 1 };
        cardDictionary["BlueAttack4"] = new Card { Mana = 1, IsSpecial = true };
        cardDictionary["BlueDefend1"] = new Card { Block = 8 };
        cardDictionary["BlueSkill1"] = new Card { IsSpecial = true, Mana = 1 };
        cardDictionary["BlueSkill2"] = new Card { IsSpecial = true };
        cardDictionary["RedAttack1"] = new Card { Damage = 6 };
        cardDictionary["RedAttack2"] = new Card { Damage = 12, Mana = 1 };
        cardDictionary["RedDefend1"] = new Card { Block = 5 };
        cardDictionary["RedPower1"] = new Card { GainStrength = 1, Mana = 1 };
        cardDictionary["GraySkill1"] = new Card { IsSpecial = true, DrawCards = 2 };
        cardDictionary["GraySkill2"] = new Card { AddCardBuff = 1 };
        cardDictionary["RedAttack3"] = new Card { Mana = 1, Rage = 1, Damage = 8 };

        cardDictionary["RedPower2"] = new Card { IsSpecial = true };



        statManagerScript = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        player = GameObject.Find("Player Side").GetComponent<Side>();
        player.Hp = statManagerScript.hp;

        enemy = GameObject.Find("Enemy Side").GetComponent<Side>();
        enemy.Hp = statManagerScript.enemyHp;

        creatingCards();

        cardData = gameObject.GetComponent<CardData>();

        player.CardAction += OnCardAction;
        enemy.CardAction += OnCardAction;

        player.StatsAction += OnStatsAction;
        enemy.StatsAction += OnStatsAction;
    }

    void Start()
    {
        player.DrawCounter = player.Cards.Count;
        player.DiscardCounter = 0;
        enemy.DrawCounter = enemy.Cards.Count;
        enemy.DiscardCounter = 0;

        enemy.DrawCards();
        player.DrawCards();

        playerUi.TableCards = player.Cards;
        enemyUi.TableCards = enemy.Cards;
    }

    void Update()
    {
        if (!isProcessingQueue && eventQueue.Count > 0)
        {
            StartCoroutine(ProcessEventQueue());
        }

        // эти два форича должны быть заменены ивентами
        foreach (GameObject card in playingCards)
        {
            card.GetComponent<CardAnimationsControl>().desiredPosition = new Vector2(0, 0);
        }

        PlanRevengeMode();

    }

    void creatingCards()
    {
        Side[] Sides = new Side[] { player, enemy };
        foreach (Side side in Sides)
        {
            foreach (GameObject card in side.CardList)
            {
                Vector2 CardStartPosition = side == player ? PlayerStartPosition : EnemyStartPosition;
                Instantiate(card, CardStartPosition, Quaternion.identity);

                Card cardScript = card.GetComponent<Card>();
                cardScript.cardSide = side;
                cardScript.otherSide = Sides[1 - IndexOf(Sides, side)];
                cardScript.eventStack = eventStack;

            }

        }
    }

    void PlanRevengeMode()
    {
        if (enemy.PlanRevenge)
        {
            CardEvent newEvent = eventStack.Pop();
            if (newEvent.ActionType == enemy.discardAnimation || newEvent.ActionType == enemy.burnAnimation)
                enemy.AddCardBuff(1);
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

        PlayerUi side = cardEvent.TriggeringSide == player ? playerUi : enemyUi;

        cardEvent.ActionType.PlayAnimation(cardEvent.Card, side);
        yield return new WaitForSeconds(0.3f);
        isProcessingQueue = false;
    }

    IEnumerator ProcessStatEventQueue()
    {
        isProcessingStatEventQueue = true;
        StatEvent statEvent = statEventQueue.Dequeue();

        PlayerUi side = cardEvent.TriggeringSide == player ? playerUi : enemyUi;
        PlayAnimation(statEvent.Side, statEvent.ManaType, statEvent.Mana)

        cardEvent.ActionType.PlayAnimation(cardEvent.Card, side);
        yield return new WaitForSeconds(0.3f);
        isProcessingQueue = false;
    }

    void PlayAnimation(Side side, Side.ManaType manaType, int mana)
    {

    }

    void OnDestroy()
    {
        player.CardAction -= OnCardAction;
        enemy.CardAction -= OnCardAction;

        player.StatsAction -= OnStatsAction;
        enemy.StatsAction -= OnStatsAction;
    }

    void OnCardAction(GameObject card, Side side, IPlayable cardActionType)
    {
        eventQueue.Enqueue(new CardEvent(card, cardActionType, side));
        eventStack.Push(new CardEvent(card, cardActionType, side));
    }

    void OnStatsAction(Side side, Side.ManaType manaType, int mana)
    {
        //в ту же очередь загружаем ивенты для активации поп-апов всяких, но не загружаем их в карточный стак. это окей?
        statEventQueue.Enqueue(new StatEvent(side, manaType, mana));
    }
}

public interface IPlayable
{
    public void PlayAnimation(GameObject card, PlayerUi side);
    // CardManager cardManager;
}

public class AddAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, PlayerUi side)
    {
        CardManager cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        cardManager.playingCards.Add(card);
        side.TableCards.Remove(card);
        CardAnimationUtility.CalculateTableCardsPosition(side);
    }
}

public class DrawAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, PlayerUi side)
    {
        side.TableCards.Add(card);
        card.transform.localPosition = side.StartPosition;
        CardAnimationUtility.CalculateTableCardsPosition(side);
        side.DrawCounter--;
    }
}

public class DiscardAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, PlayerUi side)
    {
        CardManager cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        if (cardManager.playingCards.Contains(card))
            cardManager.playingCards.Remove(card);
        if (side.TableCards.Contains(card))
            side.TableCards.Remove(card);
        card.GetComponent<CardAnimationsControl>().desiredPosition = side.DiscardPosition;
        CardAnimationUtility.CalculateTableCardsPosition(side);
        side.DiscardCounter++;
    }
}

public class ShuffleAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, PlayerUi side)
    {
        foreach (GameObject cardD in side.Side.Cards)
        {
            cardD.transform.localPosition = side.StartPosition;
            cardD.GetComponent<CardAnimationsControl>().desiredPosition = side.StartPosition;
        }
        foreach (GameObject cardD in side.Side.DiscardedCards)
        {
            cardD.transform.localPosition = side.DiscardPosition;
            cardD.GetComponent<CardAnimationsControl>().desiredPosition = side.DiscardPosition;
        }

        side.DrawCounter += side.DiscardCounter;
        side.DiscardCounter = 0;
    }
}
public class BurnAnimation : IPlayable
{
    public void PlayAnimation(GameObject card, PlayerUi side)
    {
        if (side.TableCards.Contains(card)) { side.TableCards.Remove(card); }
        card.transform.localPosition = new Vector2(2000, 2000);
        card.GetComponent<CardAnimationsControl>().isntDragging = false;
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

public class StatEvent
{
    public Side TriggeringSide;
    public Side.ManaType ManaType;
    public int Mana;

    public StatEvent(Side side, Side.ManaType manaType, int mana)
    {
        TriggeringSide = side;
        ManaType = manaType;
        Mana = mana;
    }
}

public static class CardAnimationUtility
{

    public static void CalculateTableCardsPosition(PlayerUi side)
    {
        int offset = 125;
        int width = (side.TableCards.Count - 1) * offset;
        int halfWidth = width / 2;
        for (int cardIndex = 0; cardIndex < side.TableCards.Count; cardIndex++)
        {
            GameObject Card = side.TableCards[cardIndex];
            CardAnimationsControl сard = Card.GetComponent<CardAnimationsControl>();
            SpriteRenderer sprite = Card.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = cardIndex;
            float desiredX = -halfWidth + (offset * cardIndex);
            сard.desiredPosition = new Vector2(desiredX, side.HandPosition);
            сard.timestamp = Time.time + сard.timeBetweenMoves;
            сard.startPosition = сard.desiredPosition;
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

public class PlayerUi
{
    public Vector2 StartPosition = new Vector2(-1114, -716);
    public Vector2 DiscardPosition = new Vector2(1150, -700);
    public List<GameObject> TableCards = new List<GameObject>();
    public int DrawCounter = 0;
    public int DiscardCounter = 0;
    public int HandPosition = 0;

    public Side Side;
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
        card.GetComponent<Card>().desiredPosition = side.DiscardPosition;
        CalculateTableCardsPosition(side);
        side.DiscardCounter++;
    }
    void PlayShuffleAnimation(Side side)
    {
        foreach(GameObject card in side.Cards)
        {
            card.transform.localPosition = side.StartPosition;
            card.GetComponent<Card>().desiredPosition = side.StartPosition;
        }
        foreach (GameObject card in side.DiscardedCards)
        {
            card.transform.localPosition = side.DiscardPosition;
            card.GetComponent<Card>().desiredPosition = side.DiscardPosition;
        }

        side.DrawCounter += side.DiscardCounter;
        side.DiscardCounter = 0;
    }

    void PlayBurnAnimation(GameObject card, Side side)
    {
        if (side.DoubleTableCards.Contains(card)) { side.DoubleTableCards.Remove(card); }
        card.transform.localPosition = new Vector2(2000, 2000);
        card.GetComponent<Card>().isntDragging = false;
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
            Card grid = Card.GetComponent<Card>();
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
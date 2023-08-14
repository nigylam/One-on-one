using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;

public class CardScript : MonoBehaviour
{
    public float timeBetweenMoves = 0.333f;
    public float timestamp;
    public float interpolationSpeed = 6;
    public Vector2 desiredPosition;
    public Vector2 startPosition;
    public SpriteRenderer sprite;

    public GameObject StatManager;
    public GameObject PlayingArea;
    StatManager statManagerScript;
    public GameObject CardManager;
    CardManager cardManagerScript;
    CardsActions CardsActions;

    public int cardMana;
    public string cardId;
    public string cardDescription;
    public CardType cardType;
    public CardColor cardColor;

    public bool needHighliht = true;
    bool isOverDropZone = false;
    //public bool isDragging = false;

    public enum CardType
    {
        Attack = 0,
        Defend = 1,
        Skill = 3,
        Power = 4
    }
    public enum CardColor
    {
        Red = 0,
        Blue = 1,
    }

    void Awake()
    {
        //isDragging = true;
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        PlayingArea = GameObject.Find("Playing Area");
        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();

        CardManager = GameObject.Find("Card Manager");
        cardManagerScript = CardManager.GetComponent<CardManager>();

        CardsActions = GetComponent<CardsActions>();

        //listStoringGameObject = CardsActions.cardSide.Cards.Contains(gameObject);
    }
    void Update()
    {
        CardPlacing();
        transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
    }

    public void CardPlacing()
    {
        if (CardsActions.cardSide.DiscardedCards.Contains(gameObject))
        {
            desiredPosition = CardsActions.cardSide.DiscardPosition;
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (CardsActions.cardSide.Cards.Contains(gameObject))
        {
            desiredPosition = CardsActions.cardSide.StartPosition;
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (cardManagerScript.playingCards.Contains(gameObject))
        {
            desiredPosition = new Vector2 (0,0);
        }
    }

    public void OnMouseEnter()
    {
        if (Time.time >= timestamp)
        {
            if (CardsActions.cardSide.TableCards.Contains(gameObject))
            {
                sprite.sortingLayerName = "Top";
                desiredPosition += new Vector2(0, CardsActions.cardSide.HiglightPosition);
                timestamp = Time.time + timeBetweenMoves;
                transform.localScale = new Vector2(1.2f, 1.2f);
            }
        }
    }
    public void OnMouseExit()
    {
        if (CardsActions.cardSide.TableCards.Contains(gameObject))
        {
            desiredPosition = startPosition;
            timestamp = Time.time + timeBetweenMoves;
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        PlayingArea = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        PlayingArea = null;
    }

    public void OnMouseDrag()
    {
        sprite.sortingLayerName = "Default";
        transform.localScale = new Vector2(1f, 1f);
        if (CardsActions.cardSide.TableCards.Contains(gameObject))
        {
            //isDragging = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }

    public void OnMouseUp()
    {
        if (CardsActions.cardSide.TableCards.Contains(gameObject))
        {
            if (cardManagerScript.SacrificeMode == true)
            {
                cardManagerScript.enemyCardsOnTheTable.Remove(gameObject);
                cardManagerScript.enemyBurnedCards.Add(gameObject);
                //isDragging = true;
                transform.localPosition = new Vector2(1000, 1000);
            }
            else if (cardManagerScript.DiscardMode == true)
            {
                cardManagerScript.cardsOnTheTable.Remove(gameObject);
                cardManagerScript.discardedCards.Add(gameObject);
                //isDragging = false;
                //transform.localPosition = new Vector2(1150, 700);
            }
            else
            {
                //isDragging = false;
                if (cardMana <= cardManagerScript.enemyCardsOnTheTable.Count - 1)
                {
                    if (isOverDropZone)
                    {
                        CardsActions.cardSide.TableCards.Remove(gameObject);
                        StartCoroutine(CardsActions.PlayingCard());
                    }
                }
            }
        }
    }
}

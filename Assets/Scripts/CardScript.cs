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
    bool discard = false;
    public bool isDragging;

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
        //desiredPosition = transform.localPosition;
        isDragging = true;
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
    }
    void Update()
    {
        if (isDragging == false)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
        }
    }
    public void OnMouseEnter()
    {
        if (Time.time >= timestamp)
        {
            sprite.sortingLayerName = "Top";
            desiredPosition = transform.localPosition;

            if (needHighliht) 
            {
                // вот тут тоже нужен сайд, чтобы убрать лишний код!!!
                if (gameObject.tag == "Respawn")
                {
                    desiredPosition += new Vector2(0, -50);
                }
                else
                {
                    desiredPosition += new Vector2(0, 50);
                }
                timestamp = Time.time + timeBetweenMoves;
                transform.localScale = new Vector2(1.2f, 1.2f);
            }
        }
    }
    public void OnMouseExit()
    {
        if (discard == false)
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
        //Debug.Log(desiredPosition);
        PlayingArea = null;
    }

    public void OnMouseDrag()
    {
        isDragging = true;
        //Debug.Log(isDragging);
        //transform.localPosition = new Vector2(Input.mousePosition.x - 1000, Input.mousePosition.y - 450);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void OnMouseUp()
    {
        if (cardManagerScript.SacrificeMode == true)
        {
            cardManagerScript.enemyCardsOnTheTable.Remove(gameObject);
            cardManagerScript.enemyBurnedCards.Add(gameObject);
            isDragging = true;
            transform.localPosition = new Vector2(1000, 1000);
        }
        else
        {
            isDragging = false;
            if (cardMana <= cardManagerScript.enemyCardsOnTheTable.Count - 1)
            {
                if (isOverDropZone)
                {
                    discard = true;
                }
                if (discard)
                {
                    StartCoroutine(CardsActions.PlayingCard());
                }
            }
        }
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;
using Unity.UI;
using TMPro;
using static CardScript;
using Cards;

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
    [TextArea()]
    public string cardDescription;
    public string cardDescriptionDynamic;
    public string cardTitle;
    public CardType cardType;
    public CardColor cardColor;
    public int cardDamage;

    public GameObject CardDescription;
    public GameObject CardTitle;
    public GameObject CardTypePrint;

    TextMeshProUGUI CardDescriptionText;
    TextMeshProUGUI CardTitleText;
    TextMeshProUGUI CardTypeText;

    public GameObject CardCanvas;
    Canvas cardCanvas;

    bool isOverDropZone = false;
    public bool isntDragging = true;

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

        //CardCanvas = gameObject.transform.Find("Canvas").gameObject;
        //CardDescription = CardCanvas.transform.Find("Text (TMP)").gameObject;

        CardDescriptionText = CardDescription.GetComponent<TextMeshProUGUI>();
        CardTitleText = CardTitle.GetComponent<TextMeshProUGUI>();
        CardTypeText = CardTypePrint.GetComponent<TextMeshProUGUI>();
        cardCanvas = CardCanvas.GetComponent<Canvas>();

        
        CardTitleText.text = cardTitle;

        switch (cardType)
        {
            case CardType.Attack:
                CardTypeText.text = "Атака";
                break;
            case CardType.Defend:
                CardTypeText.text = "Защита";
                break;
            case CardType.Skill:
                CardTypeText.text = "Прием";
                break;
            case CardType.Power:
                CardTypeText.text = "Техника";
                break;
        }
        
        //cardDescription = CardsDescriptions.BlueAttack1.Description;
    }
    void Update()
    {
        DescriptionTranscription();
        CardPlacing();
        if (isntDragging)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
            cardCanvas.sortingOrder = sprite.sortingOrder;
        }
        else
        {
            cardCanvas.sortingOrder = sprite.sortingOrder+1;
        }
        cardCanvas.sortingLayerName = sprite.sortingLayerName;
        CardDescriptionText.text = cardDescriptionDynamic;
    }

    public void DescriptionTranscription()
    {
        //string cardDescriptionDynamicWithoutTags;
        if (cardDescription.Contains("["))
        {
            int firstSym = cardDescription.IndexOf('[');
            int secondSym = cardDescription.IndexOf(']');
            string damage = "";
            for (int i = firstSym + 1; i < secondSym; i++)
                {
                    damage += cardDescription[i];
                }
            cardDamage = Int32.Parse(damage);
            cardDescriptionDynamic = cardDescription.Replace("[", "");
            cardDescriptionDynamic = cardDescriptionDynamic.Replace("]", "");
            int finalDamage = cardDamage + CardsActions.cardSide.Strength;
            cardDescriptionDynamic = cardDescriptionDynamic.Replace(damage, finalDamage.ToString());

            if (CardsActions.cardSide.Strength != 0)
            {
                string coloredDamage = "<color=" + (CardsActions.cardSide.Strength > 0 ? "green" : "red") + ">" + finalDamage.ToString() + "</color>";
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(finalDamage.ToString(), coloredDamage);
            }
            
        } else
        {
            cardDescriptionDynamic = cardDescription;
        }
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
            transform.localScale = new Vector2(1f, 1f);
            sprite.sortingLayerName = "Default";
            sprite.sortingOrder = 0;
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
        isntDragging = false;
        sprite.sortingLayerName = "Top";
        transform.localScale = new Vector2(1f, 1f);
        //desiredPosition = transform.position;
        if (CardsActions.cardSide.TableCards.Contains(gameObject))
        {
            //isDragging = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }

        //sprite.sortingOrder = 2;
    }

    public void OnMouseUp()
    {
        isntDragging = true;
        if (CardsActions.cardSide.TableCards.Contains(gameObject))
        {
            if (cardManagerScript.manaSpendingMode == true)
            {
                if (CardsActions.cardSide == cardManagerScript.enemy)
                {
                    cardManagerScript.enemyCardsOnTheTable.Remove(gameObject);
                    cardManagerScript.enemyBurnedCards.Add(gameObject);
                    transform.localPosition = new Vector2(1000, 1000);
                    desiredPosition = new Vector2(1000, 1000);
                } else
                {
                    cardManagerScript.cardsOnTheTable.Remove(gameObject);
                    cardManagerScript.discardedCards.Add(gameObject);
                }
            }
            else
            {
                if (cardMana <= CardsActions.cardSide.TableCards.Count - 1)
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

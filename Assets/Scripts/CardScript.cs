using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;
using Unity.UI;
using TMPro;
using static CardScript;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour
{
    // animations and controll
    public float timeBetweenMoves = 0.333f;
    public float timestamp;
    public float interpolationSpeed = 6;
    public Vector2 desiredPosition;
    public Vector2 startPosition;
    public SpriteRenderer sprite;
    bool isOverDropZone = false;
    public bool isntDragging = true;
    public bool playerActionCompleted = false;

    //access to another objects
    public GameObject StatManager;
    public GameObject PlayingArea;
    StatManager statManagerScript;
    public GameObject CardManager;
    CardManager cardManagerScript;
    CardsActions CardsActions;
    public GameObject CardDescription;
    public GameObject CardTitle;
    public GameObject CardTypePrint;
    TextMeshProUGUI CardDescriptionText;
    TextMeshProUGUI CardTitleText;
    TextMeshProUGUI CardTypeText;
    public GameObject CardCanvas;
    public Canvas cardCanvas;

    //card data
    public int cardMana;
    public string cardId;
    //[TextArea()]
    //public string cardDescription;
    public string cardDescriptionDynamic;
    //public string cardTitle;
    public CardType cardType;
    public int cardDamage = 0;
    public int finalDamage = 0;
    public int cardBlock = 0;
    public int cardStrength = 0;
    public int cardDraw = 0;
    public int lastStrength = -1;
    public Side cardSide;
    public Side otherSide;
    public bool special;

    //Card card;

    public enum CardType
    {
        Attack = 0,
        Defend = 1,
        Skill = 3,
        Power = 4
    }

    void Awake()
    {
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
        CardDescriptionText = CardDescription.GetComponent<TextMeshProUGUI>();
        CardTitleText = CardTitle.GetComponent<TextMeshProUGUI>();
        CardTypeText = CardTypePrint.GetComponent<TextMeshProUGUI>();
        cardCanvas = CardCanvas.GetComponent<Canvas>();
        desiredPosition = new Vector2(-2000, 0);

        //card = cardManagerScript.cardData.cardDictionary[cardId];

        if (gameObject.tag == "Player")
        {
            cardSide = cardManagerScript.player;
            otherSide = cardManagerScript.enemy;
        }
        else
        {
            cardSide = cardManagerScript.enemy;
            otherSide = cardManagerScript.player;
        }

        CardTitleText.text = LocalizationSettings.StringDatabase.GetLocalizedString(cardId + "_Title");

        switch (cardType)
        {
            case CardType.Attack:
                CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Attack_Type");
                break;
            case CardType.Defend:
                CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Defend_Type");
                break;
            case CardType.Skill:
                CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Type"); ;
                break;
            case CardType.Power:
                CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Power_Type"); ;
                break;
        }
    }
    void Update()
    {
        //if(cardId == "BlueAttack2") { Debug.Log(desiredPosition); }
        DescriptionTranscription();
        CardPlacing();
        if (isntDragging)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
            cardCanvas.sortingOrder = sprite.sortingOrder;
        }
        else if (cardManagerScript.popupMode)
        {
            return;
        }
        else
        {
            cardCanvas.sortingOrder = sprite.sortingOrder + 2;
        }
        cardCanvas.sortingLayerName = sprite.sortingLayerName;
        CardDescriptionText.text = cardDescriptionDynamic;
    }

    public void DescriptionTranscription()
    {
        if (cardSide.Strength != lastStrength)
        {
            lastStrength = cardSide.Strength;
            finalDamage = 0;
            cardDescriptionDynamic = LocalizationSettings.StringDatabase.GetLocalizedString(cardId + "_Description");
            //string cardDescriptionDynamicWithoutTags;
            if (cardDescriptionDynamic.Contains("["))
            {
                int firstSym = cardDescriptionDynamic.IndexOf('[');
                int secondSym = cardDescriptionDynamic.IndexOf(']');
                string damage = "";
                for (int i = firstSym + 1; i < secondSym; i++)
                {
                    damage += cardDescriptionDynamic[i];
                }
                cardDamage = Int32.Parse(damage);
                cardDescriptionDynamic = cardDescriptionDynamic.Replace("[", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace("]", "");
                finalDamage = cardDamage + cardSide.Strength;
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(damage, finalDamage.ToString());


                if (cardSide.Strength != 0)
                {
                    string coloredDamage = "<color=" + (cardSide.Strength > 0 ? "green" : "red") + ">" + finalDamage.ToString() + "</color>";
                    cardDescriptionDynamic = cardDescriptionDynamic.Replace(finalDamage.ToString(), coloredDamage);
                }

            }
            if (cardDescriptionDynamic.Contains(";"))
            {
                int firstSym = cardDescriptionDynamic.IndexOf(';');
                int secondSym = cardDescriptionDynamic.IndexOf('?');
                string block = "";
                for (int i = firstSym + 1; i < secondSym; i++)
                {
                    block += cardDescriptionDynamic[i];
                }
                cardBlock = Int32.Parse(block);
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(";", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace("?", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(block, cardBlock.ToString());
            }
            if (cardDescriptionDynamic.Contains("{"))
            {
                int firstSym = cardDescriptionDynamic.IndexOf('{');
                int secondSym = cardDescriptionDynamic.IndexOf('}');
                string draw = "";
                for (int i = firstSym + 1; i < secondSym; i++)
                {
                    draw += cardDescriptionDynamic[i];
                }
                cardDraw = Int32.Parse(draw);
                cardDescriptionDynamic = cardDescriptionDynamic.Replace("{", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace("}", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(draw, cardDraw.ToString());
            }
            if (cardDescriptionDynamic.Contains("("))
            {
                int firstSym = cardDescriptionDynamic.IndexOf('(');
                int secondSym = cardDescriptionDynamic.IndexOf(')');
                string strength = "";
                for (int i = firstSym + 1; i < secondSym; i++)
                {
                    strength += cardDescriptionDynamic[i];
                }
                cardStrength = Int32.Parse(strength);
                cardDescriptionDynamic = cardDescriptionDynamic.Replace("(", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(")", "");
                cardDescriptionDynamic = cardDescriptionDynamic.Replace(strength, cardStrength.ToString());
            }
        }

    }

    public void CardPlacing()
    {

        if (cardSide.DiscardedCards.Contains(gameObject))
        {
            //desiredPosition = cardSide.DiscardPosition;
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (cardSide.Cards.Contains(gameObject))
        {
            //desiredPosition = cardSide.StartPosition;
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (cardManagerScript.playingCards.Contains(gameObject))
        {
            //desiredPosition = new Vector2(0, 0);
            transform.localScale = new Vector2(1f, 1f);
            sprite.sortingLayerName = "Default";
            sprite.sortingOrder = 0;
        }
        else if (cardSide.BurnedCards.Contains(gameObject))
        {
            //transform.localPosition = new Vector2(1000, 1000);
            //desiredPosition = new Vector2(1000, 1000);
        }
        else if (cardManagerScript.displayCards.Contains(gameObject))
        {
            isntDragging = false;
            sprite.sortingLayerName = "Top";
            sprite.sortingOrder = 3;
        }
    }

    public void OnMouseEnter()
    {
        if (Time.time >= timestamp)
        {
            if (cardSide.TableCards.Contains(gameObject) && cardManagerScript.popupMode == false)
            {
                sprite.sortingLayerName = "Top";
                desiredPosition += new Vector2(0, cardSide.HiglightPosition);
                timestamp = Time.time + timeBetweenMoves;
                transform.localScale = new Vector2(1.2f, 1.2f);
            }
        }
    }
    public void OnMouseExit()
    {
        if (cardSide.TableCards.Contains(gameObject))
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
        if (cardSide.TableCards.Contains(gameObject) && cardManagerScript.popupMode == false)
        {
            isntDragging = false;
            sprite.sortingLayerName = "Top";
            transform.localScale = new Vector2(1f, 1f);
            //desiredPosition = transform.position;
            if (cardSide.TableCards.Contains(gameObject))
            {
                //isDragging = true;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;
            }

            //sprite.sortingOrder = 2;
        }
    }

    public void OnMouseUp()
    {
        isntDragging = true;
        if (cardSide.TableCards.Contains(gameObject))
        {
            if (cardManagerScript.discardMode)
            {          
                cardSide.DiscardCard(gameObject);
            }
            else if (cardManagerScript.burnMode)
            {
                cardSide.TableCards.Remove(gameObject);
                cardSide.BurnedCards.Add(gameObject);
            }
            else
            {
                if (cardMana <= cardSide.TableCards.Count - 1)
                {
                    if (isOverDropZone)
                    {
                        cardSide.TableCards.Remove(gameObject);
                        StartCoroutine(CardPlaying());
                    }
                }
            }
        }
    }

    public IEnumerator CardPlaying()
    {
        if (cardMana > 0)
        {
            if (cardSide == cardManagerScript.enemy)
            {
                StartCoroutine(ManaSpending(cardMana, false));
            } else
            {
                StartCoroutine(ManaSpending(cardMana));
            }
            yield return new WaitUntil(() => playerActionCompleted);
        }
        if (!special)
        {
            cardSide.Block += cardBlock;
            otherSide.Hp = finalDamage > 0 ? otherSide.DealDamage(finalDamage) : otherSide.Hp;
            cardSide.Strength += cardStrength;
            cardSide.DrawCards(1);
        }
        else
        {
            StartCoroutine(CardsActions.PlaySpecialCard());
            yield return new WaitUntil(() => playerActionCompleted);
        }
        playerActionCompleted = false;
        if (cardType == CardType.Power) { cardSide.BurnedCards.Add(gameObject); }
        else { cardSide.DiscardCard(gameObject); }
    }

    public IEnumerator ManaSpending(int numberOfCards, bool isDiscard = true)
    {
        int initialCardCount = cardSide.TableCards.Count;
        cardManagerScript.playingCards.Add(gameObject);
        if (isDiscard) { 
            cardManagerScript.discardMode = true;
            cardManagerScript.player.ManaPopUp.SetActive(true);
        } else { 
            cardManagerScript.burnMode = true;
            cardManagerScript.enemy.ManaPopUp.SetActive(true);
        }
        
        yield return new WaitUntil(() => cardSide.TableCards.Count <= initialCardCount - numberOfCards);
        playerActionCompleted = true;
        cardSide.ManaPopUp.SetActive(false);
        otherSide.ManaPopUp.SetActive(false);
        cardManagerScript.discardMode = false;
        cardManagerScript.burnMode = false;
        cardManagerScript.playingCards.Remove(gameObject);
    }
}

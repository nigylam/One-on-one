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
    //card data
    public int Mana;
    public string cardId;
    //public string cardDescriptionDynamic;
    //public string cardTitle;
    public CardType cardType;
    public Side cardSide;
    public Side otherSide;
    public int lastStrength = -1;
    public int finalDamage = 0;

    /*
    public int cardDamage = 0;
    public int cardBlock = 0;
    public int cardStrength = 0;
    public int cardDraw = 0;
    public bool special;
    */

    //access to another objects
    public GameObject PlayingArea;
    StatManager statManager;
    CardManager cardManager;
    CardsActions cardsActions;
    public GameObject CardDescription;
    public GameObject CardTitle;
    public GameObject CardTypePrint;
    TextMeshProUGUI CardDescriptionText;
    TextMeshProUGUI CardTitleText;
    TextMeshProUGUI CardTypeText;
    public GameObject CardCanvas;
    public Canvas cardCanvas;
    CardData cardData;

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
    public bool enemyCardsDiscarded = false;

    public Card card;

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
        statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
        cardsActions = GetComponent<CardsActions>();
        CardDescriptionText = CardDescription.GetComponent<TextMeshProUGUI>();
        CardTitleText = CardTitle.GetComponent<TextMeshProUGUI>();
        CardTypeText = CardTypePrint.GetComponent<TextMeshProUGUI>();
        cardCanvas = CardCanvas.GetComponent<Canvas>();
        desiredPosition = new Vector2(-2000, 0);
        cardData = GameObject.Find("Card Manager").GetComponent<CardData>();
        card = cardData.cardDictionary[cardId];
        CardSideDefining();
        CardTypeDefining();

        CardTitleText.text = LocalizationSettings.StringDatabase.GetLocalizedString(cardId + "_Title");
    }
    void Update()
    {
        //if(cardId == "BlueAttack2") { Debug.Log(desiredPosition); }
        //DescriptionTranscription();
        CardPlacing();
        finalDamage = card.Damage + cardSide.Strength;
        LerpAndLayerControlling();
        cardCanvas.sortingLayerName = sprite.sortingLayerName;
    }

    void LerpAndLayerControlling()
    {
        if (isntDragging)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
            cardCanvas.sortingOrder = sprite.sortingOrder;
        }
        else if (cardManager.popupMode)
        {
            return;
        }
        else
        {
            cardCanvas.sortingOrder = sprite.sortingOrder + 2;
        }
    }

    // вот это заменить тоже как с интерфейсами в кардменеджере
    void CardTypeDefining()
    {
        if (cardId.Contains("Attack"))
        {
            cardType = CardType.Attack;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Attack_Type");
        }
        else if (cardId.Contains("Defend"))
        {
            cardType = CardType.Defend;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Defend_Type");
        }
        else if (cardId.Contains("Skill"))
        {
            cardType = CardType.Skill;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Type");
        }
        else if (cardId.Contains("Power"))
        {
            cardType = CardType.Power;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Power_Type");
        }
    }

    void CardSideDefining()
    {
        if (gameObject.tag == "Player")
        {
            cardSide = cardManager.player;
            otherSide = cardManager.enemy;
        }
        else
        {
            cardSide = cardManager.enemy;
            otherSide = cardManager.player;
        }
    }

    public void CardPlacing()
    {

        if (cardSide.DiscardedCards.Contains(gameObject))
        {
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (cardSide.Cards.Contains(gameObject))
        {
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (cardManager.playingCards.Contains(gameObject))
        {
            transform.localScale = new Vector2(1f, 1f);
            sprite.sortingLayerName = "Default";
            sprite.sortingOrder = 0;
        }
        else if (cardManager.displayCards.Contains(gameObject))
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
            if (cardSide.TableCards.Contains(gameObject) && cardManager.popupMode == false)
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
        if (cardSide.TableCards.Contains(gameObject) && cardManager.popupMode == false)
        {
            isntDragging = false;
            sprite.sortingLayerName = "Top";
            transform.localScale = new Vector2(1f, 1f);
            if (cardSide.TableCards.Contains(gameObject))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;
            }
        }
    }

    public void OnMouseUp()
    {
        isntDragging = true;
        if (cardSide.TableCards.Contains(gameObject))
        {
            if (cardSide.discardMode)
            {          
                cardSide.DiscardCard(gameObject);
            }
            else if (cardSide.burnMode)
            {
                cardSide.BurnCard(gameObject);
            }
            else
            {
                if (card.Mana <= cardSide.TableCards.Count - 1)
                {
                    if (isOverDropZone)
                    {
                        cardSide.PlayCard(gameObject);
                        StartCoroutine(CardPlaying());
                    }
                }
            }
        }
    }

    public IEnumerator CardPlaying()
    {
        if (card.Mana > 0)
        {
            if (cardSide == cardManager.enemy)
            {
                StartCoroutine(ManaSpending(card.Mana, cardSide, false));
            } else
            {
                StartCoroutine(ManaSpending(card.Mana, cardSide));
            }
            yield return new WaitUntil(() => playerActionCompleted);
        }
        if (!card.IsSpecial)
        {
            cardSide.Block += card.Block;
            otherSide.Hp = finalDamage > 0 ? otherSide.DealDamage(finalDamage) : otherSide.Hp;
            cardSide.Strength += card.GainStrength;
            cardSide.DrawCards(card.DrawCards);
            cardSide.AddCardBuff(card.AddCardBuff);
        }
        else
        {
            StartCoroutine(cardsActions.PlaySpecialCard());
            yield return new WaitUntil(() => playerActionCompleted);
        }
        playerActionCompleted = false;
        if (cardType == CardType.Power) { cardSide.BurnCard(gameObject); }
        else { cardSide.DiscardCard(gameObject); }
    }

    public IEnumerator ManaSpending(int numberOfCards, Side side, bool isDiscard = true)
    {
        int initialCardCount = side.TableCards.Count;
        cardManager.playingCards.Add(gameObject);

        // надо упростить
        if (side == cardSide)
        {
            if (isDiscard)
            {
                side.discardMode = true;
                statManager.EnablingPopUp(StatManager.PopUpTextType.DiscardPlayerCard);
            }
            else
            {
                side.burnMode = true;
                statManager.EnablingPopUp(StatManager.PopUpTextType.BurnPlayerCard);
            }
        } else
        {
            if (isDiscard)
            {
                side.discardMode = true;
                statManager.EnablingPopUp(StatManager.PopUpTextType.DiscardEnemyCard);
            }
            else
            {
                side.burnMode = true;
                statManager.EnablingPopUp(StatManager.PopUpTextType.BurnEnemyCard);
            }
        }
        
        
        yield return new WaitUntil(() => side.TableCards.Count <= initialCardCount - numberOfCards);
        playerActionCompleted = true;
        enemyCardsDiscarded = true;
        statManager.CardBurnDiscardPopUp.SetActive(false);
        statManager.CardBurnDiscardPopUp.SetActive(false);
        side.discardMode = false;
        side.burnMode = false;
        cardManager.playingCards.Remove(gameObject);
    }
}


/*
 * public void DescriptionTranscription()
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
*/
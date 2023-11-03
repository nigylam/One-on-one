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
    public CardType cardType;
    public Side cardSide;
    public Side otherSide;
    public int lastStrength = -1;
    public int finalDamage = 0;

    //access to another objects
    public GameObject PlayingArea;
    public StatManager statManager;
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

    public int cardsDiscarded = 0;

    public Card card;

    public DiscardType discardType = new DiscardType();
    public SacrificeType sacrificeType = new SacrificeType();

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
        CardPlacing();
        finalDamage = card.Damage + cardSide.Strength;
        LerpAndLayerControlling();
        cardCanvas.sortingLayerName = sprite.sortingLayerName;

        if (cardManager.playingCards.Contains(gameObject))
        {
            if(cardManager.eventStack.Count > 0)
            {
                CardEvent newEvent = cardManager.eventStack.Pop();
                if (newEvent.ActionType == cardSide.discardAnimation)
                    cardsDiscarded++;
            }
        }
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
            StartCoroutine(ManaSpending());
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
            playerActionCompleted = false;
            StartCoroutine(cardsActions.PlaySpecialCard());
            yield return new WaitUntil(() => playerActionCompleted);
        }
        playerActionCompleted = false;
        if (cardType == CardType.Power) { cardSide.BurnCard(gameObject); }
        else
        {
            cardSide.DiscardCard(gameObject);
        }
    }

    public IEnumerator ManaSpending()
    {
        if (cardSide == cardManager.enemy)
            StartCoroutine(ManaSpending(cardSide, sacrificeType, card.Mana));
        else
            StartCoroutine(ManaSpending(cardSide, discardType, card.Mana));
        yield return null;
    }

    public IEnumerator ManaSpending(Side side, ITypeMana manaType, int numberOfCards = 0, bool isEnemy = false)
    {
        int initialCardCount = side.TableCards.Count;

        manaType.ActivateMode(side, numberOfCards, isEnemy);

        yield return new WaitUntil(() => statManager.cardsChoosed);
        yield return new WaitUntil(() => side.TableCards.Count <= initialCardCount - numberOfCards);
        playerActionCompleted = true;
        manaType.DisableMode(side);
    }
}

public interface ITypeMana
{
    public void ActivateMode(Side side, int numberOfCards, bool isEnemy);
    public void DisableMode(Side side);
}
public class DiscardType : ITypeMana
{
    public void ActivateMode(Side side, int numberOfCards, bool isEnemy = false)
    {
        StatManager statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        side.discardMode = true;
        statManager.CardBurnDiscardPopUp.SetActive(true);
        if (numberOfCards == 0)
        {
            statManager.CardDiscBurnButton.SetActive(true);
            statManager.tipLocizeText.StringReference.TableEntryReference = "DiscAnyPlayerPop_Tip";
        }
        else
        {
            if (!isEnemy)
            {
                statManager.tipLocizeText.StringReference.TableEntryReference = "DiscPlayerPop_Tip";
            }
            else
            {
                statManager.tipLocizeText.StringReference.TableEntryReference = "DiscEnemyPop_Tip";
            }
        }
    }

    public void DisableMode(Side side)
    {
        StatManager statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        side.discardMode = false;
        statManager.DisablingPopUp();
    }
}

public class SacrificeType : ITypeMana
{
    public void ActivateMode(Side side, int numberOfCards, bool isEnemy = false)
    {
        StatManager statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        side.burnMode = true;
        statManager.CardBurnDiscardPopUp.SetActive(true);
        if (numberOfCards == 0)
        {
            statManager.CardDiscBurnButton.SetActive(true);

            // добавить этот текст в словарь
            statManager.tipLocizeText.StringReference.TableEntryReference = "SacrAnyPlayerPop_Tip";
        }
        else
        {
            if (!isEnemy)
            {
                statManager.tipLocizeText.StringReference.TableEntryReference = "SacrPlayerPop_Tip";
            }
            else
            {
                statManager.tipLocizeText.StringReference.TableEntryReference = "SacrEnemyPop_Tip";
            }
        }
    }

    public void DisableMode(Side side)
    {
        StatManager statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        side.burnMode = false;
        statManager.DisablingPopUp();
    }
}
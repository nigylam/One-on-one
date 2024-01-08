using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;
using Unity.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.EventSystems;

public class CardAnimationsControl : MonoBehaviour
{
    //public Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();


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

    public int finalDamage = 0;

    public int cardsDiscarded = 0;

    public Card card;

    public DiscardType discardType = new DiscardType();
    public SacrificeType sacrificeType = new SacrificeType();

    public bool playerActionCompleted = false;
    public int lastStrength = -1;


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

        card = gameObject.GetComponent<Card>();
        //card = cardData.cardDictionary[cardId];
        //CardSideDefining();
        //CardTypeDefining();

        CardTitleText.text = LocalizationSettings.StringDatabase.GetLocalizedString(card.cardId + "_Title");
    }
    void Update()
    {
        finalDamage = card.cardSide.Rage > 0 ? Convert.ToInt32(Math.Ceiling((1.5) * Convert.ToSingle(card.Damage + card.cardSide.Strength))) : card.Damage + card.cardSide.Strength;

        CardTypeDefining();
        CardPlacing();

        LerpAndLayerControlling();
        cardCanvas.sortingLayerName = sprite.sortingLayerName;

        // тоже костыль какой-то между логикой и контролем
        if (cardManager.playingCards.Contains(gameObject) && card.cardId == "BlueSkill2")
        {
            if (cardManager.eventStack.Count > 0)
            {
                CardEvent newEvent = cardManager.eventStack.Pop();
                if (newEvent.ActionType == card.cardSide.discardAnimation)
                    cardsDiscarded++;
            }
        }
    }

    void CardTypeDefining()
    {
        if (card.cardId.Contains("Attack"))
        {
            card.cardType = Card.CardType.Attack;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Attack_Type");
        }
        else if (card.cardId.Contains("Defend"))
        {
            card.cardType = Card.CardType.Defend;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Defend_Type");
        }
        else if (card.cardId.Contains("Skill"))
        {
            card.cardType = Card.CardType.Skill;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Skill_Type");
        }
        else if (card.cardId.Contains("Power"))
        {
            card.cardType = Card.CardType.Power;
            CardTypeText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Power_Type");
        }
    }

    public void CardPlacing()
    {
        if (card.cardSide.DiscardedCards.Contains(gameObject))
        {
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (card.cardSide.Cards.Contains(gameObject))
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

    public void OnMouseEnter()
    {
        if (Time.time >= timestamp)
        {
            // вот этот иф должке быть заменен другим условием, которое здесь кратко записано. типа bool Showable
            if (card.cardSide.TableCards.Contains(gameObject) && cardManager.popupMode == false)
            {
                sprite.sortingLayerName = "Top";
                desiredPosition += new Vector2(0, card.cardSide.HiglightPosition);
                timestamp = Time.time + timeBetweenMoves;
                transform.localScale = new Vector2(1.2f, 1.2f);
            }
        }
    }

    public void OnMouseExit()
    {
        if (card.cardSide.TableCards.Contains(gameObject))
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
        if (card.cardSide.TableCards.Contains(gameObject) && cardManager.popupMode == false)
        {
            isntDragging = false;
            sprite.sortingLayerName = "Top";
            transform.localScale = new Vector2(1f, 1f);
            if (card.cardSide.TableCards.Contains(gameObject))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePosition;
            }
        }
    }

    public void OnMouseUp()
    {
        isntDragging = true;


        if (card.cardSide.TableCards.Contains(gameObject))
        {
            if (card.cardSide.discardMode)
            {
                card.cardSide.DiscardCard(gameObject);
            }
            else if (card.cardSide.burnMode)
            {
                card.cardSide.BurnCard(gameObject);
            }
            else
            {
                if (isOverDropZone && card.Mana <= card.cardSide.TableCards.Count - 1)
                {
                    StartCoroutine(CardPlaying());
                }
            }
        }
    }

    public IEnumerator CardPlaying()
    {
        card.cardSide.PlayCard(gameObject);
        card.cardSide.ManaSpend(card.Mana);
        yield return new WaitUntil(() => !card.cardSide.manaSpendMode);
        card.Play();
        yield return new WaitUntil(() => !card.Playing);

        card.RemoveFromTable();
    }


    public IEnumerator ManaSpending()
    {
        if (cardSide == cardManager.enemy)
            StartCoroutine(ManaSpending(cardSide, sacrificeType, Mana));
        else
            StartCoroutine(ManaSpending(cardSide, discardType, Mana));
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class DeckShow : MonoBehaviour
{
    public GameObject Deck;
    GameObject CardManager;
    CardManager cardManager;
    GameObject Front;
    SpriteRenderer frontSprite;
    GameObject Content;
    GameObject Button;
    Button button;
    GameObject ClosePopup;
    CloseCardsPopUp closeCardsPopUp;
    GameObject TitleCardPopUpCanvas;
    Canvas titleCardPopUpCanvas;
    GameObject TitleCardPopupText;
    LocalizeStringEvent titleCardPopupText;

    public List<GameObject> cards = new List<GameObject>();

    void Awake()
    {
        ClosePopup = GameObject.Find("Close Popup");
        closeCardsPopUp = ClosePopup.GetComponent<CloseCardsPopUp>();
    }
    // Start is called before the first frame update
    void Start()
    {
        CardManager = GameObject.Find("Card Manager");
        Content = GameObject.Find("Content");
        cardManager = CardManager.GetComponent<CardManager>();
        Front = GameObject.Find("Scroll View");
        frontSprite = Front.GetComponent<SpriteRenderer>();
        Button = GameObject.Find("End Turn");
        button = Button.GetComponent<Button>();
        TitleCardPopUpCanvas = GameObject.Find("TitleCardPopUpCanvas");
        titleCardPopUpCanvas = TitleCardPopUpCanvas.GetComponent<Canvas>();
        TitleCardPopupText = GameObject.Find("TitleCardPopupText");
        titleCardPopupText = TitleCardPopupText.GetComponent<LocalizeStringEvent>();

        switch (Deck.name)
        {
            case "Draw Deck":
                cards = cardManager.player.Cards;
                titleCardPopupText.StringReference.TableEntryReference = "DrawPile_Popup_Title";
                break;
            case "Discard Deck":
                cards = cardManager.player.DiscardedCards;
                titleCardPopupText.StringReference.TableEntryReference = "DiscardPile_Popup_Title";
                break;
            case "Enemy Draw Deck":
                cards = cardManager.enemy.Cards;
                titleCardPopupText.StringReference.TableEntryReference = "DrawPile_Popup_Title";
                break;
            case "Enemy Discard Deck":
                cards = cardManager.enemy.DiscardedCards;
                titleCardPopupText.StringReference.TableEntryReference = "DiscardPile_Popup_Title";
                break;
        }
    }

    public void OnMouseUp()
    {
        if (!cardManager.popupMode)
        {
            frontSprite.sortingLayerName = "Top";
            frontSprite.sortingOrder = 2;
            titleCardPopUpCanvas.sortingLayerName = "Top";
            titleCardPopUpCanvas.sortingOrder = 3;
            cardManager.popupMode = true;
            button.interactable = false;
            ClosePopup.SetActive(true);

            int i = 1000;
            foreach (GameObject card in cards)
            {
                GameObject cardCopy = Instantiate(card, Content.transform);
                cardCopy.transform.localScale = new Vector2(1.2f, 1.2f);
                CardScript cardScript;
                cardScript = cardCopy.GetComponent<CardScript>();
                cardScript.cardCanvas.sortingOrder = i;
                cardScript.cardCanvas.sortingLayerName = "Top";

                cardManager.displayCards.Add(cardCopy);
                cardCopy.tag = "Popup";
                i--;
            }

            switch (Deck.name)
            {
                case "Draw Deck":
                case "Enemy Draw Deck":
                    titleCardPopupText.StringReference.TableEntryReference = "DrawPile_Popup_Title";
                    break;
                case "Enemy Discard Deck":
                case "Discard Deck":
                    titleCardPopupText.StringReference.TableEntryReference = "DiscardPile_Popup_Title";
                    break;
            }
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}

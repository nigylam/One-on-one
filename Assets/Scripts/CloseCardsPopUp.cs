using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseCardsPopUp : MonoBehaviour
{
    GameObject CardManager;
    CardManager cardManager;
    GameObject Front;
    SpriteRenderer frontSprite;
    GameObject Content;
    GameObject Button;
    Button button;
    GameObject TitleCardPopUpCanvas;
    Canvas titleCardPopUpCanvas;

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

        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        List<GameObject> cardsToRemove = new List<GameObject>();

        foreach (var card in cardManager.displayCards)
        {
            cardsToRemove.Add(card);
        }

        foreach (var cardToRemove in cardsToRemove)
        {
            Destroy(cardToRemove);
            cardManager.displayCards.Remove(cardToRemove);
        }

        frontSprite.sortingLayerName = "Background";
        frontSprite.sortingOrder = -1;
        titleCardPopUpCanvas.sortingLayerName = "Background";
        titleCardPopUpCanvas.sortingOrder = -1;
        cardManager.popupMode = false;
        button.interactable = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cardManager.popupMode)
        {
            gameObject.SetActive(true);
        }
    }
}

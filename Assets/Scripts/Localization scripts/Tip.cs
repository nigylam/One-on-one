using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class Tip : MonoBehaviour
{
    public GameObject TipObject;
    public GameObject TipText;
    TextMeshProUGUI Text;
    bool isMouseOver;

    GameObject CardManager;
    CardManager cardManager;

    void Start()
    {
        TipText = TipObject.transform.GetChild(0).gameObject;
        Text = TipText.GetComponent<TextMeshProUGUI>();
        TipObject.SetActive(false);
        CardManager = GameObject.Find("Card Manager");
        cardManager = CardManager.GetComponent<CardManager>();
    }
    public void OnMouseOver()
    {
        if (Text.text != "" && !Input.GetMouseButton(0) && cardManager.popupMode == false)
        {
            TipObject.SetActive(true);
            isMouseOver = true;
        } else if (Text.text != "" && gameObject.tag == "Popup")
        {
            TipObject.SetActive(true);
            isMouseOver = true;
        }
    }

    public void OnMouseExit()
    {
        TipObject.SetActive(false);
        isMouseOver = false;
    }

    public void OnMouseDrag()
    {
        if (isMouseOver)
        {
            TipObject.SetActive(false);
            isMouseOver = false;
        }
    }
}

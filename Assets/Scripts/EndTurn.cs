using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public GameObject CardManager;
    CardManager cardManagerScript;
    int curentTurn = 1;

    void Start()
    {
        CardManager = GameObject.Find("Card Manager");
        cardManagerScript = CardManager.GetComponent<CardManager>();
        // Debug.Log("Clones:" + clones.Length);

    }

    public void OnClick()
    {
        if (curentTurn % 2 == 1)
        {
            StartCoroutine(cardManagerScript.DiscardingCard(cardManagerScript.player));
            StartCoroutine(cardManagerScript.DrawingCard(cardManagerScript.player, 5, 2f));
            curentTurn++;
        } else
        {
            StartCoroutine(cardManagerScript.DiscardingCard(cardManagerScript.enemy));
            StartCoroutine(cardManagerScript.DrawingCard(cardManagerScript.enemy, 5, 2f));
            curentTurn++;
        }
    }
}

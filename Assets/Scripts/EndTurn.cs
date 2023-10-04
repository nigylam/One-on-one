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
    }

    public void OnClick()
    {
        if (curentTurn % 2 == 1)
        {
            cardManagerScript.player.DiscardCards();
            //cardManagerScript.player.DrawCard();
            curentTurn++;
        } else
        {
            cardManagerScript.player.DrawCard();
            //cardManagerScript.enemy.DiscardCards();
            //cardManagerScript.enemy.DrawCard();
            //cardManagerScript.enemy.DrawCard(5);
            curentTurn++;
        }
    }
}

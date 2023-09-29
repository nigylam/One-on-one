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
            cardManagerScript.player.DiscardCard(5);
            //cardManagerScript.player.DrawCard(5);
            curentTurn++;
        } else
        {
            cardManagerScript.player.DrawCard(5);
            //cardManagerScript.enemy.DiscardCard();
            //cardManagerScript.enemy.DrawCard(5);
            curentTurn++;
        }
    }
}

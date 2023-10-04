using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    CardManager cardManagerScript;

    void Start()
    {
        cardManagerScript = GameObject.Find("Card Manager").GetComponent<CardManager>();
    }

    public void OnClick()
    {
        if (cardManagerScript.curentTurn % 2 == 1)
        {
            cardManagerScript.player.DiscardCards();
            cardManagerScript.player.DrawCard();
            cardManagerScript.curentTurn++;
        } else
        {
            //cardManagerScript.player.DrawCard();
            cardManagerScript.enemy.DiscardCards();
            cardManagerScript.enemy.DrawCard();
            //cardManagerScript.enemy.DrawCard(5);
            cardManagerScript.curentTurn++;
        }
    }
}

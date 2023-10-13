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
            cardManagerScript.player.DrawCards();
            cardManagerScript.curentTurn++;
        }
        else
        {
            cardManagerScript.enemy.DiscardCards();
            cardManagerScript.enemy.DrawCards();
            cardManagerScript.curentTurn++;
        }

        string i = "";

        Debug.Log("Table cards: ");
        foreach (GameObject Card in cardManagerScript.player.TableCards)
        {
            i += Card.name;
            i += ", ";
        }

        Debug.Log(i);

        i = "";

        Debug.Log("Draw cards: ");
        foreach (GameObject Card in cardManagerScript.player.Cards)
        {
            i += Card.name;
            i += ", ";
        }

        Debug.Log(i);
    }
}

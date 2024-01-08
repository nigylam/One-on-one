using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardsActions : MonoBehaviour
{
    Card card;
    CardManager cardManager;
    StatManager statManager;


    void Start()
    {
        card = gameObject.GetComponent<Card>();
        statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
    }

    void Update()
    {

    }

    
}

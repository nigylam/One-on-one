using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsActions : MonoBehaviour
{
    CardScript CardScript;

    public GameObject CardManager;
    CardManager cardManagerScript;

    public GameObject StatManager;
    StatManager statManagerScript;

    public Side cardSide;
    public Side otherSide;

    void Start()
    {
        CardScript = gameObject.GetComponent<CardScript>();

        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();

        CardManager = GameObject.Find("Card Manager");
        cardManagerScript = CardManager.GetComponent<CardManager>();

    }

    void Update()
    {

    }
}

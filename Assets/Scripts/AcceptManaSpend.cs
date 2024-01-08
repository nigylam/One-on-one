using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptManaSpend : MonoBehaviour
{
    CardManager cardManager;

    public void OnClick()
    {
        cardManager.enemy.manaSpendMode = false;
        cardManager.player.manaSpendMode = false;
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Start()
    {
        //statManager = GameObject.Find("Stat Manager").GetComponent<StatManager>();
        cardManager = GameObject.Find("Card Manager").GetComponent<CardManager>();
    }

    void Update()
    {
    }
}

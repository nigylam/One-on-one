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

    public IEnumerator PlaySpecialCard()
    {
        switch (CardScript.cardId)
        {
            case "GraySkill1":
                CardScript.cardSide.DrawCard(2);
                //yield return new WaitForSecondsRealtime(.6f);
                StartCoroutine(CardScript.ManaSpending(1));
                yield return new WaitUntil(() => CardScript.playerActionCompleted);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class CardDescription : MonoBehaviour
{
    public int finalDamage;
    public int block;
    public int mana = 0;
    public int gainStrength = 0;
    public int gainDexterity = 0;
    public int drawCards = 0;
    public int addCardBuff = 0;
    public int sideStrength = 0;

    public CardScript cardScript;
    LocalizeStringEvent localizeStringEventComp;
    // Start is called before the first frame update
    void Awake()
    {
        cardScript = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<CardScript>();
        localizeStringEventComp = gameObject.GetComponent<LocalizeStringEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        finalDamage = cardScript.finalDamage;
        block = cardScript.card.Block;
        mana = cardScript.card.Mana;
        gainStrength = cardScript.card.GainStrength;
        gainDexterity = cardScript.card.GainDexterity;
        drawCards = cardScript.card.DrawCards;
        addCardBuff = cardScript.card.AddCardBuff;
        sideStrength = cardScript.cardSide.Strength;
        localizeStringEventComp.RefreshString();
    }
}
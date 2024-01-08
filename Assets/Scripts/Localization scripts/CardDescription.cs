using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using System;

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
    public int damage = 0;

    public Card card;
    LocalizeStringEvent localizeStringEventComp;
    // Start is called before the first frame update
    void Awake()
    {
        card = gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Card>();
        localizeStringEventComp = gameObject.GetComponent<LocalizeStringEvent>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        damage = card.Damage;
        finalDamage = card.finalDamage;
        block = card.Block;
        mana = card.Mana;
        gainStrength = card.GainStrength;
        gainDexterity = card.GainDexterity;
        drawCards = card.DrawCards;
        addCardBuff = card.AddCardBuff;
        sideStrength = card.cardSide.Strength;
        localizeStringEventComp.RefreshString();
    }
}

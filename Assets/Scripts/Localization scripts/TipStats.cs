using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;

public class TipStats : MonoBehaviour
{
    public GameObject CardManager;
    CardManager CardManagerScript;

    LocalizeStringEvent localizeStringEventComp;

    Side tipSide;

    public bool isPlayer;

    [HideInInspector]
    public int strength;
    [HideInInspector]
    public int block;

    string finalString;

    [HideInInspector]
    public int finalDamageHalfBlock;

    void Start()
    {
        CardManager = GameObject.Find("Card Manager");
        CardManagerScript = CardManager.GetComponent<CardManager>();
        localizeStringEventComp = gameObject.GetComponent<LocalizeStringEvent>();
        if (isPlayer) { tipSide = CardManagerScript.player; }
        else { tipSide = CardManagerScript.enemy; }

        finalString = localizeStringEventComp.StringReference.GetLocalizedString();
    }

    void Update()
    {
        //Debug.Log(strength);
        strength = tipSide.Strength;
        block = tipSide.Block;
        finalDamageHalfBlock = Convert.ToInt32(Math.Ceiling(Convert.ToSingle(tipSide.Block) / 2));

        localizeStringEventComp.RefreshString();
    }
}

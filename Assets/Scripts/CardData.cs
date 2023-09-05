using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CardData : MonoBehaviour
{
    public Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

    void Awake()
    {
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en-US");

        cardDictionary["BlueAttack1"] = new Card { Title = GetLocalizedString("BlueAttack1_Title"), Description = GetLocalizedString("BlueAttack1_Description"), Type = Card.CardType.Attack };
        cardDictionary["BlueAttack2"] = new Card { Title = GetLocalizedString("BlueAttack2_Title"), Description = GetLocalizedString("BlueAttack2_Description"), Type = 0, Mana = 1 };
        cardDictionary["BlueDefend1"] = new Card { Title = GetLocalizedString("BlueDefend1_Title"), Description = GetLocalizedString("BlueDefend1_Description"), Type = Card.CardType.Defend };
        cardDictionary["RedAttack1"] = new Card { Title = GetLocalizedString("RedAttack1_Title"), Description = GetLocalizedString("RedAttack1_Description"), Type = Card.CardType.Attack };
        cardDictionary["RedAttack2"] = new Card { Title = GetLocalizedString("RedAttack2_Title"), Description = GetLocalizedString("RedAttack2_Description"), Type = Card.CardType.Attack, Mana = 2 };
        cardDictionary["RedDefend1"] = new Card { Title = GetLocalizedString("RedDefend1_Title"), Description = GetLocalizedString("RedDefend1_Description"), Type = Card.CardType.Defend };
        cardDictionary["RedPower1"] = new Card { Title = GetLocalizedString("RedPower1_Title"), Description = GetLocalizedString("RedPower1_Description"), Type = Card.CardType.Power, Mana = 1 };
        //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
        //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
        //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
        //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
    }

    string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(key);
    }

    void Start()
    {
        Card Red = new Card { Title = "Удар", Description = "Наносит 5 урона", Type = 0 };


    }
}

public class Card
{
    public string Title;
    public string Description;
    public CardType Type;
    public int Mana = 0;
    public bool IsSpecial = false;

    public enum CardType
    {
        Attack = 0,
        Defend = 1,
        Skill = 3,
        Power = 4
    }
}


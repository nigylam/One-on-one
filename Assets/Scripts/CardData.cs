using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

    void Awake()
    {
        cardDictionary["BlueAttack1"] = new Card {Damage = 5 };
        cardDictionary["BlueAttack2"] = new Card { Mana = 1, Damage = 4, DrawCards = 2 };
        cardDictionary["BlueDefend1"] = new Card { Block = 6 };
        cardDictionary["RedAttack1"] = new Card { Damage = 6 };
        cardDictionary["RedAttack2"] = new Card { Damage = 12, Mana = 2 };
        cardDictionary["RedDefend1"] = new Card { Block = 5 };
        cardDictionary["RedPower1"] = new Card { GainStrength = 1, Mana = 1 };
        cardDictionary["GraySkill1"] = new Card { IsSpecial = true, DrawCards = 2 };
        cardDictionary["GraySkill2"] = new Card { AddCardBuff = 1 };
        cardDictionary["BlueAttack3"] = new Card { IsSpecial = true, Damage = 9, Mana = 1 };
        cardDictionary["BlueSkill1"] = new Card { IsSpecial = true, Mana = 1 };
    }
}

public class Card
{
    public int Mana = 0;
    public bool IsSpecial = false;
    public int Damage = 0;
    public int Block = 0;
    public int GainStrength = 0;
    public int GainDexterity = 0;
    public int DrawCards = 0;
    public int AddCardBuff = 0;
}

/*
public enum CardType
{
    Attack = 0,
    Defend = 1,
    Skill = 3,
    Power = 4
}*/
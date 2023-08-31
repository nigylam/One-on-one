using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CardData : MonoBehaviour
    {
        public Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

        void Awake()
        {
            cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит 5 урона", Type = 0 };
        }

        void Start()
        {
            //Card Red = new Card { Title = "Удар", Description = "Наносит 5 урона", Type = 0 };


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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class CardData : MonoBehaviour
    {
        public Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

        void Awake()
        {
            cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = Card.CardType.Attack };
            cardDictionary["BlueAttack2"] = new Card { Title = "Удар снизу", Description = "Обмен: 1. Наносит 4 урона. Вытаскивает {1} карту.", Type = 0, Mana = 1 };
            cardDictionary["BlueDefend1"] = new Card { Title = "Блок", Description = "Ставит ;6? блока", Type = Card.CardType.Defend };
            cardDictionary["RedAttack1"] = new Card { Title = "Удар", Description = "Наносит [6] урона", Type = Card.CardType.Attack };
            cardDictionary["RedAttack2"] = new Card { Title = "Ногой в корпус", Description = "Жертва: 2. Наносит [12] урона", Type = Card.CardType.Attack, Mana = 2};
            cardDictionary["RedDefend1"] = new Card { Title = "Блок", Description = "Ставит ;5? блока.", Type = Card.CardType.Defend };
            cardDictionary["RedPower1"] = new Card { Title = "Твердая рука", Description = "Жертва: 1. Увеличивает силу на (1).", Type = Card.CardType.Power, Mana = 1 };
            //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
            //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
            //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
            //cardDictionary["BlueAttack1"] = new Card { Title = "Удар", Description = "Наносит [5] урона", Type = 0 };
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


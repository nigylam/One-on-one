using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "All ", menuName = "Card Game/All Card Data", order = 1)]
public class AllCardDataSO : ScriptableObject
{
    public CardData[] allCards;
}


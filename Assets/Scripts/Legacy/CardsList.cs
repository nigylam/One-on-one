using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsList : MonoBehaviour
{
    public GameObject BlueAttack1;
    public GameObject BlueDefend1;
    public GameObject RedAttack1;
    public GameObject RedDefend1;
    private GameObject dropZone;

    public void Play(string cardName)
    {
        Destroy(GameObject.Find(cardName));
        switch (cardName)
        {
            case "BlueAttack1(Clone)1":
                gameObject.GetComponent<CardsEffects>().DealDamage(5, "BlueAttack1(Clone)1");
                GameObject.Find("DrawCards").GetComponent<DrawCards>().EnemyDiscardPileCount += 1;
                break;
            case "BlueDefend1(Clone)1":
                gameObject.GetComponent<CardsEffects>().GainBlock(6, "BlueDefend1(Clone)1");
                GameObject.Find("DrawCards").GetComponent<DrawCards>().EnemyDiscardPileCount += 1;
                break;
            case "RedAttack1(Clone)1":
                gameObject.GetComponent<CardsEffects>().DealDamage(6, "RedAttack1(Clone)1");
                GameObject.Find("DrawCards").GetComponent<DrawCards>().DiscardPileCount += 1;
                break;
            case "RedDefend1(Clone)1":
                gameObject.GetComponent<CardsEffects>().GainBlock(5, "RedDefend1(Clone)1");
                GameObject.Find("DrawCards").GetComponent<DrawCards>().DiscardPileCount += 1;
                break;
            case "RedAttack2(Clone)1":
                gameObject.GetComponent<CardsEffects>().Sacrifice(2, "RedAttack2(Clone)1");
                gameObject.GetComponent<CardsEffects>().DealDamage(12, "RedAttack2(Clone)1");
                break;
        }
    }
}

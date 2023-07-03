using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardsEffects : MonoBehaviour
{
    int enemyHp;
    int hp;
    int enemyBlock;
    int block;
    HpManager hpM;
    public GameObject ChooseCard;
    public GameObject[] playerHand;
    public GameObject PlayerArea;
    int playerHandStart;
    int sacrAmountForUpdate;

    void Start()
    {
        hpM = GameObject.Find("Hp Manager").GetComponent<HpManager>();
        enemyHp = hpM.GetEnemyHP();
        enemyBlock = hpM.GetEnemyBlock();
        hp = hpM.GetHP();
        block = hpM.GetBlock();
    }

    public void DealDamage(int damageAmount, string cardName)
    {
        if (GameObject.Find(cardName).tag == "Respawn")
        {
            if (enemyBlock > 0)
            {
                if (damageAmount > enemyBlock)
                {
                    enemyHp -= damageAmount - enemyBlock;
                    enemyBlock = 0;
                }
                else
                {
                    enemyBlock -= damageAmount;
                }
            } else
            {
                enemyHp -= damageAmount;
            }
        }
        else
        {
            if (block > 0)
            {
                if (damageAmount > block)
                {
                    enemyHp -= damageAmount - block;
                    block = 0;
                }
                else
                {
                    block -= damageAmount;
                }
            }
            else
            {
                hp -= damageAmount;
            }
        }
        hpM.HP = hp;
        hpM.enemyHP = enemyHp;
        hpM.block = block;
        hpM.enemyBlock = enemyBlock;
    }

    public void GainBlock (int blockAmount, string cardName)
    {
        if (GameObject.Find(cardName).tag == "Respawn")
        {
            block += blockAmount;
            Debug.Log("Block from CardEffects:" + block);
            Debug.Log("Block from HpManager:" + GameObject.Find("Hp Manager").GetComponent<HpManager>().block);
        }
        else
        {
            enemyBlock += blockAmount;
            Debug.Log("Enemy block:" + enemyBlock);
        }
        hpM.HP = hp;
        hpM.enemyHP = enemyHp;
        hpM.block = block;
        hpM.enemyBlock = enemyBlock;
    }

    public void Sacrifice(int sacrAmount, string cardName)
    {
        if(GameObject.Find(cardName).tag == "Respawn") 
        {
            ChooseCard.SetActive(true);
            GameObject TextTitle = GameObject.Find("Text For Choose Card Bar");
            Debug.Log(TextTitle.name + " is finded");
            GameObject CardPlacer = GameObject.Find("CardPlacer");
            TextTitle.GetComponent<TextMeshProUGUI>().text = "Choose " + sacrAmount + " cards to sacrifice";
            playerHand = GameObject.FindGameObjectsWithTag("Respawn");
            foreach (GameObject card in playerHand)
            {
                card.transform.SetParent(CardPlacer.transform, false);
            }
            playerHandStart = playerHand.Length;
            Debug.Log(playerHand.Length);
            sacrAmountForUpdate = sacrAmount;
        }
        else
        {

        }
    }

    public void Update()
    {
        if (playerHand.Length == playerHandStart - sacrAmountForUpdate)
        {
            ChooseCard.SetActive(false);
            foreach (GameObject card in playerHand)
            {
                if (card == null)
                {
                    return;
                }
                card.transform.SetParent(PlayerArea.transform, false);
            }
        }
    }
}

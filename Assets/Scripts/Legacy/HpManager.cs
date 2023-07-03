using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpManager : MonoBehaviour
{
    GameObject EnemyTextHp;
    GameObject TextHp;
    public int HP = 10;
    public int enemyHP = 10;
    public GameObject Player1WinText;
    public GameObject Player2WinText;
    public GameObject Canvas;
    public GameObject BlockText;
    public GameObject EnemyBlockText;
    public int block = 0;
    public int enemyBlock = 0;
    public GameObject BlockBar;
    public GameObject EnemyBlockBar;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        EnemyTextHp = GameObject.Find("Enemy Health");
        TextHp = GameObject.Find("Health");
        //BlockBar = GameObject.Find("BlockBar");
        //EnemyBlockBar = GameObject.Find("EnemyBlockBar");
    }

    public int GetBlock()
    {
        return block;
    }
    public int GetEnemyBlock()
    {
        return enemyBlock;
    }
    public int GetHP()
    {
        return HP;
    }
    public int GetEnemyHP()
    {
        return enemyHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (block < 1)
        {
            BlockBar.SetActive(false);
        } else
        {
            BlockBar.SetActive(true);
        }
        if (enemyBlock < 1)
        {
            EnemyBlockBar.SetActive(false);
        } else
        {
            EnemyBlockBar.SetActive(true);
        }

        BlockText.GetComponent<TextMeshProUGUI>().text = "" + block;
        EnemyBlockText.GetComponent<TextMeshProUGUI>().text = "" + enemyBlock;

        EnemyTextHp.GetComponent<TextMeshProUGUI>().text = "" + enemyHP;
        TextHp.GetComponent<TextMeshProUGUI>().text = "" + HP;

        if (enemyHP < 0)
        {
            GameObject p1Win = Instantiate(Player1WinText, new Vector2(0, 0), Quaternion.identity);
            p1Win.transform.SetParent(Canvas.transform, false);
        }
        if (HP < 0)
        {
            GameObject p2Win = Instantiate(Player2WinText, new Vector2(0, 0), Quaternion.identity);
            p2Win.transform.SetParent(Canvas.transform, false);
        }
    }
}

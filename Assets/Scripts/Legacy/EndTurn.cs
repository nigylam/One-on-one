using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    GameObject[] clones;
    GameObject[] enemyClones;
    public GameObject Background;
    public GameObject StartNewTurn;
    int whosTurn = 0;

    /*void Start ()
    {
        Background = GameObject.Find("Background");
        GameObject[] clones = GameObject.FindGameObjectsWithTag("Respawn");
       // Debug.Log("Clones:" + clones.Length);

    } */

    IEnumerator TurnEnding()
    {
        StartNewTurn = GameObject.Find("DrawCards");
        Background = GameObject.Find("Background");
        whosTurn += 1;

        if (whosTurn%2 == 1)
        {
            GameObject[] clones = GameObject.FindGameObjectsWithTag("Respawn"); ;
            for (int i = 0; i < clones.Length; i++)
            {
                GameObject clone;
                clone = clones[i];
                Destroy(clone);
                StartNewTurn.GetComponent<DrawCards>().DiscardPileCount += 1;
                yield return new WaitForSeconds(.2f);
            }
            StartCoroutine(StartNewTurn.GetComponent<DrawCards>().Drawing(5, false));
            if (GameObject.Find("Hp Manager").GetComponent<HpManager>().enemyBlock > 0)
            {
                GameObject blockText = GameObject.Find("Block(Clone)");
                GameObject.Find("Hp Manager").GetComponent<HpManager>().enemyBlock = 0;
            }
        }
        else
        {
            GameObject[] enemyClones = GameObject.FindGameObjectsWithTag("clone"); ;
            for (int e = 0; e < enemyClones.Length; e++)
            {
                GameObject enemyClone;
                enemyClone = enemyClones[e];
                Destroy(enemyClone);
                StartNewTurn.GetComponent<DrawCards>().EnemyDiscardPileCount += 1;
                yield return new WaitForSeconds(.2f);
            }
            StartCoroutine(StartNewTurn.GetComponent<DrawCards>().Drawing(5, true));
            if (GameObject.Find("Hp Manager").GetComponent<HpManager>().block > 0)
            {
                GameObject enemyBlockText = GameObject.Find("EnemyBlock(Clone)");
                Destroy(enemyBlockText);
                GameObject.Find("Hp Manager").GetComponent<HpManager>().block = 0;
            }
        }
    }

        public void OnClick()
    {
        StartCoroutine(TurnEnding());
    }
}

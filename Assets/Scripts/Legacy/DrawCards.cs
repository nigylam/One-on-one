using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawCards : MonoBehaviour
{
    public Vector2 startPosition;

    public GameObject Card1;
    public GameObject EnemyCard1;
    public GameObject Card2;
    public GameObject EnemyCard2;
    public GameObject Card3;

    public GameObject MainCanvas;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    public GameObject DrawPileCounter;
    public GameObject DiscardPileCounter;
    public GameObject EnemyDrawPileCounter;
    public GameObject EnemyDiscardPileCounter;

    public int DrawPileCount = 30;
    public int DiscardPileCount = 0;
    public int EnemyDrawPileCount = 30;
    public int EnemyDiscardPileCount = 0;

    List<GameObject> enemyCards = new List<GameObject>();
    List<GameObject> cards = new List<GameObject>();

    public IEnumerator DrawingCard(int startX, int startY, int finalX, int finalY, GameObject area, List<GameObject> whosCard, int deltaX, int deltaY, System.Action<GameObject> callback)
    {
        GameObject card = Instantiate(whosCard[Random.Range(0, whosCard.Count)], new Vector2(0, 0), Quaternion.identity);
        card.transform.SetParent(MainCanvas.transform, false);
        card.transform.localPosition = new Vector2(startX, startY);
        for (int n = startX, p = startY; n == finalX || p == finalY; n += deltaX, p += deltaY)
        {
            card.transform.localPosition = new Vector2(n, p);
                yield return new WaitForSeconds(.01f);
            Debug.Log(card.transform.localPosition);
        }
        card.transform.SetParent(area.transform, false);
        startPosition = card.transform.localPosition;
        yield return new WaitForSeconds(0.1f);

        callback.Invoke(card);
    }

    public IEnumerator Drawing(int cardsToDraw, bool isEnemy)
    {
        if (isEnemy) {
            yield return new WaitForSeconds(.5f);
            for (int i = 0; i < cardsToDraw; i++)
            {
                GameObject card = null;
                yield return StartCoroutine(CoroutineWrapper<GameObject>(DrawingCard(-870, 680, 140, 380, EnemyArea, enemyCards, 2, -1, c => card = c)));
                card.name = (card.name + i);
                //yield return new WaitForSeconds(.2f);
            }
        }
        else if (!isEnemy) {
            yield return new WaitForSeconds(.5f);
            for (int i = 0; i < cardsToDraw; i++)
            {
                // Start the DrawingCard coroutine and capture the returned GameObject using a coroutine wrapper function
                GameObject card = null;
                yield return StartCoroutine(CoroutineWrapper<GameObject>(DrawingCard(-870, -680, 140, -380, PlayerArea, cards, 2, 1, c => card = c)));

                // Set the card's name
                card.name = (card.name + i);

                //yield return new WaitForSeconds(.2f);
            }
        }
    }

    public IEnumerator CoroutineWrapper<T>(IEnumerator coroutine, System.Action<T> callback = null)
    {
        while (coroutine.MoveNext())
        {
            yield return coroutine.Current;
        }

        if (callback != null && coroutine.Current is T)
        {
            callback((T)coroutine.Current);
        }
    }

    public IEnumerator FirstDrawing()
    {
        StartCoroutine(Drawing(5, true));
        yield return new WaitForSeconds(1);
        StartCoroutine(Drawing(5, false));
    }

    void Start()
    {
        DrawPileCounter = GameObject.Find("Draw Pile Counter");
        DiscardPileCounter = GameObject.Find("Discard Pile Counter");
        EnemyDrawPileCounter = GameObject.Find("Enemy Draw Pile Counter");
        EnemyDiscardPileCounter = GameObject.Find("Enemy Discard Pile Counter");
        cards.Add(Card1);
        cards.Add(Card2);
        cards.Add(Card3);
        enemyCards.Add(EnemyCard1);
        enemyCards.Add(EnemyCard2);
        StartCoroutine(FirstDrawing());
    }

    // Update is called once per frame
    void Update()
    {
        DrawPileCounter.GetComponent<TextMeshProUGUI>().text = "" + DrawPileCount;
        DiscardPileCounter.GetComponent<TextMeshProUGUI>().text = "" + DiscardPileCount;
        EnemyDrawPileCounter.GetComponent<TextMeshProUGUI>().text = "" + EnemyDrawPileCount;
        EnemyDiscardPileCounter.GetComponent<TextMeshProUGUI>().text = "" + EnemyDiscardPileCount;
    }
}

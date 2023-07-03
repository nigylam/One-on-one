using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardsManipulationManager : MonoBehaviour
{
    public GameObject Card1;
    public GameObject EnemyCard1;
    public GameObject Card2;
    public GameObject EnemyCard2;
    public GameObject Card3;

    List<GameObject> enemyCards = new List<GameObject>();
    List<GameObject> cards = new List<GameObject>();

    public float timeBetweenMoves = 0.3333f;
    private float timestamp;
    public float interpolationSpeed = 6;
    public Vector2 desiredPosition;
    public Vector2 startPosition;
    private SpriteRenderer sprite;

    public GameObject MainCanvas;
    public GameObject PlayerArea;
    public GameObject EnemyArea;

    GameObject card;
    /*
    public GameObject DrawPileCounter;
    public GameObject DiscardPileCounter;
    public GameObject EnemyDrawPileCounter;
    public GameObject EnemyDiscardPileCounter;
    */

    // Start is called before the first frame update
    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
        cards.Add(Card3);
        enemyCards.Add(EnemyCard1);
        enemyCards.Add(EnemyCard2);
        CardsDrawing(false, 5);
        CardsDrawing(true, 5);
    }

    // Update is called once per frame
    void Update()
    {
        card.transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
    }

    public void CardsDrawing(bool isPlayer, int numberOfCardsToDraw)
    {
        if (isPlayer)
        {
            for (int i = 0; i < numberOfCardsToDraw; i++)
            {
                card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector2(0, 0), Quaternion.identity);
                card.transform.SetParent(MainCanvas.transform, false);
                card.transform.localPosition = new Vector2(-870, -680);
                startPosition = card.transform.localPosition;
                desiredPosition = new Vector2(140, -380);
                timestamp = Time.time + timeBetweenMoves;

                card.transform.localPosition = new Vector2(140, -380);
                card.transform.SetParent(PlayerArea.transform, false);
            }
        }
        else
        {

        }
    }
}

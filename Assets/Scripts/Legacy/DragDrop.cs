using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragDrop : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject drawCards;
    public GameObject BlockText;
    public GameObject EnemyBlockText;
    CardsManipulationManager CardsManipulationManager;
    //  public GameObject DropZone;
    //public PlayerManager PlayerManager;

    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;  //переназвать, а то дублируется
    private bool isOverDropZone;

    private bool isDragging = false;

    public float timeBetweenMoves = 0.3333f;
    private float timestamp;
    public float interpolationSpeed = 6;
    public Vector2 desiredPosition;

     

    //private bool isDraggable = true;

    void Awake()
    {
        CardsManipulationManager = GameObject.Find("Card Manager").GetComponent<CardsManipulationManager>();
    }

    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        // DropZone = GameObject.Find("Drop Zone");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
        //Debug.Log("Colliding");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
        //Debug.Log("UNColliding");
    }

    public void StartDrag()
    {
        isDragging = true;
        //startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        drawCards = GameObject.Find("DrawCards");
        //if (!isDraggable) return;
        isDragging = false;
        if (isOverDropZone)
        {
            gameObject.name = gameObject.name + "1";
            string cardName = gameObject.name;
            gameObject.transform.SetParent(dropZone.transform, false);
            GameObject.Find("Card Manager").GetComponent<CardsList>().Play(cardName);
        }
        else
        {
            transform.position = startPosition;
           // transform.SetParent(startParent.transform, false);
        }
    }

    public void SacrificeChoose()
    {
        if(gameObject.transform.parent.name == "CardPlacer")
        {
            Destroy(gameObject);
            GameObject cardsEffects = GameObject.Find("Card Manager");
            Debug.Log(cardsEffects.GetComponent<CardsEffects>().playerHand.Length);
            Array.Resize(ref cardsEffects.GetComponent<CardsEffects>().playerHand, cardsEffects.GetComponent<CardsEffects>().playerHand.Length - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging == true)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
        else
        {
            /*
            startPosition = startPosition.CardsManipulationManager();
            desiredPosition = desiredPosition.CardsManipulationManager();
            transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
            */
        }
    }
}

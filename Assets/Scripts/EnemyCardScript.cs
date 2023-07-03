using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;

public class EnemyCardScript : MonoBehaviour
{
    public float timeBetweenMoves = 0.3333f;
    public float timestamp;
    public float interpolationSpeed = 6;
    public Vector2 desiredPosition;
    public Vector2 startPosition;
    private SpriteRenderer sprite;

    public GameObject CardManager;
    public GameObject PlayingArea;
    bool isOverDropZone = false;
    bool discard = false;

    public bool isDragging = false;

    CardManager CardManagerForInts;

    void Start()
    {
        startPosition = transform.localPosition;
        desiredPosition = transform.localPosition;
        sprite = GetComponent<SpriteRenderer>();
        PlayingArea = GameObject.Find("Playing Area");
        CardManager = GameObject.Find("Card Manager");
        CardManagerForInts = CardManager.GetComponent<CardManager>();
    }
    void Update()
    {
        if (isDragging == false)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
        }
    }
    public void OnMouseEnter()
    {
        if (Time.time >= timestamp)
        {
            sprite.sortingLayerName = "Top";
            desiredPosition = transform.localPosition;
            desiredPosition += new Vector2(0, -50);
            timestamp = Time.time + timeBetweenMoves;
            transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }
    public void OnMouseExit()
    {
        if (discard == false)
        {
            desiredPosition = startPosition;
            timestamp = Time.time + timeBetweenMoves;
            sprite.sortingLayerName = "Default";
            transform.localScale = new Vector2(1f, 1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        PlayingArea = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        //Debug.Log(desiredPosition);
        PlayingArea = null;
    }

    public void OnMouseDrag()
    {
        isDragging = true;
        //Debug.Log(isDragging);
        transform.localPosition = new Vector2(Input.mousePosition.x - 1000, Input.mousePosition.y - 450);
    }

    public void OnMouseUp()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            discard = true;
        }
        if (discard)
        {
            desiredPosition = new Vector2(1150, 670);
            //timestamp = Time.time + timeBetweenMoves;
            if (gameObject.name.Contains("Attack1"))
            {
                CardManagerForInts.hp += CardManagerForInts.block - 6;
                CardManagerForInts.block -= 6;
            }
            else
            {
                CardManagerForInts.enemyBlock += 5;
            }
        }
    }
}

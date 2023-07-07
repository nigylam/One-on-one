using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Akassets.SmoothGridLayout;

public class CardScript : MonoBehaviour
{
    public float timeBetweenMoves = 0.3333f;
    public float timestamp;
    public float interpolationSpeed = 6;
    public Vector2 desiredPosition;
    public Vector2 startPosition;
    private SpriteRenderer sprite;

    public GameObject StatManager;
    public GameObject PlayingArea;
    bool isOverDropZone = false;
    bool discard = false;

    public bool isDragging = false;

    StatManager statManagerScript;


    void Start()
    {
        //startPosition = transform.localPosition;
        //desiredPosition = transform.localPosition;
        sprite = GetComponent<SpriteRenderer>();
        PlayingArea = GameObject.Find("Playing Area");
        StatManager = GameObject.Find("Stat Manager");
        statManagerScript = StatManager.GetComponent<StatManager>();
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
            if (gameObject.tag == "Respawn")
            {
                desiredPosition += new Vector2(0, -50);
            }
            else
            {
                desiredPosition += new Vector2(0, 50);
            }
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
        //transform.localPosition = new Vector2(Input.mousePosition.x - 1000, Input.mousePosition.y - 450);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
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
            desiredPosition = new Vector2(1150, -670);
            //timestamp = Time.time + timeBetweenMoves;
            if (gameObject.name.Contains("Attack1"))
            {
                statManagerScript.enemyHp += statManagerScript.enemyBlock - 5;
                statManagerScript.enemyBlock -= 5;
                statManagerScript.discardPileCounter++;
            }
            else
            {
                statManagerScript.block += 6;
                statManagerScript.discardPileCounter++;
            }
        }
    }

}

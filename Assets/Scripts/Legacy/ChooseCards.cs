using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCards : MonoBehaviour
{
    
    public float timeBetweenMoves = 0.3333f;
    private float timestamp;
    public float interpolationSpeed = 6;
    Vector2 desiredPosition;
    Vector2 startPosition;
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        //startPosition = GameObject.Find("DrawCards").GetComponent<DrawCards>().startPosition;
    }

    void Update()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, desiredPosition, interpolationSpeed * Time.deltaTime);
    }

    public void OnMouseOver()
    {
        // desiredPosition 
        startPosition = transform.localPosition;

        if (Time.time >= timestamp)
        {
            desiredPosition = startPosition + new Vector2(0, 200);
            timestamp = Time.time + timeBetweenMoves;
            sprite.sortingLayerName = "Top";
        }
    }

    public void OnMouseExit()
    {
        //startPosition = transform.localPosition;
        //if (Time.time >= timestamp)
        //{
        desiredPosition = startPosition;
        timestamp = Time.time + timeBetweenMoves;
        sprite.sortingLayerName = "Default";
        //}
    }
    
}
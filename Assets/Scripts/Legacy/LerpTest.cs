using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public float timeBetweenMoves = 0.3333f;
    private float timestamp;
    public float interpolationSpeed = 10.0F;
    Vector2 desiredPosition;

    void Start()
    {
        desiredPosition = transform.position;
    }

    void Update()
    {
        ArrowKeyMovement();
        transform.position = Vector2.Lerp(transform.position, desiredPosition, interpolationSpeed * Time.deltaTime);
    }

    public void OnHoverEnter()
    {
        if (Time.time >= timestamp)
        {
            desiredPosition += new Vector2(0, 100);
            timestamp = Time.time + timeBetweenMoves;
        }
    }

    public void OnHoverExit()
    {
        desiredPosition += Vector2.down * 100;
    }

    void ArrowKeyMovement()
    {
        if (Time.time >= timestamp)
        {
            if (Input.GetKey("up"))
            {
                desiredPosition += new Vector2(0, 100);

                timestamp = Time.time + timeBetweenMoves;
            }
            else if (Input.GetKey("down"))
            {
                desiredPosition -= new Vector2(0, 100);

                timestamp = Time.time + timeBetweenMoves;
            }
            else if (Input.GetKey("left"))
            {
                desiredPosition += Vector2.left;

                timestamp = Time.time + timeBetweenMoves;
            }
            else if (Input.GetKey("right"))
            {
                desiredPosition += Vector2.right;

                timestamp = Time.time + timeBetweenMoves;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 startingPosition;
    private Vector2 targetPosition;
    private bool selectionState;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.up);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector2.left);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.down);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector2.right);
            }
            timer = 0.1f;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (!selectionState)
            {
                Collider2D col = Physics2D.OverlapPoint(transform.position);
                if (col != null)
                {
                    startingPosition = transform.position;
                    selectionState = true;
                    Debug.Log("Entering Selection State");
                }
            }
            else
            {
                Debug.Log("Hello");
                targetPosition = transform.position;
                Debug.DrawRay(startingPosition, (targetPosition - startingPosition), Color.white, 10f);
                selectionState = false;
            }
        }
    }
}

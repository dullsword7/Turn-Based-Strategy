using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCursorInput : MonoBehaviour
{
    [SerializeField] private GameObject[] menuButtons;
    [SerializeField] private GameObject playerCursor;
    private PlayerInput playerInput;
    private int currentMenuButtonIndex;
    private float timer;
    private float timeoutLength;

    void Start()
    {
        playerInput = playerCursor.GetComponent<PlayerInput>();
        timeoutLength = 0.1f;
    }
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            HandleMenuCursorInput();
            timer = timeoutLength;
        }

        
    }

    private void HandleMenuCursorInput()
    {
        if (!playerInput.selectUnitActionState) return;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            NextMenuButton();
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            PreviousMenuButton();
        }
    }

    private void NextMenuButton()
    {
        currentMenuButtonIndex += 1;
        if (currentMenuButtonIndex >= menuButtons.Length) currentMenuButtonIndex = 0;
        Vector3 buttonPosition = menuButtons[currentMenuButtonIndex].transform.position;
        transform.position = new Vector3(transform.position.x, buttonPosition.y, transform.position.z);
    }

    private void PreviousMenuButton()
    {
        currentMenuButtonIndex -= 1;
        if (currentMenuButtonIndex < 0) currentMenuButtonIndex = menuButtons.Length - 1;
        Vector3 buttonPosition = menuButtons[currentMenuButtonIndex].transform.position;
        transform.position = new Vector3(transform.position.x, buttonPosition.y, transform.position.z);
    }

}

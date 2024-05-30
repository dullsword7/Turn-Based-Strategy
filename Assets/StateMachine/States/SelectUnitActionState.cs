using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitActionState : IState
{
    private PlayerController player;
    private PlayerUnit selectedPlayerUnit;
    private int currentMenuButtonIndex;
    private bool onCoolDown;
    private float timer;
    private float timeoutLength;
    private Transform selectUnitActionCursor;
    public SelectUnitActionState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
        selectUnitActionCursor = player.SelectUnitActionCursor.transform;
    }
    public void Enter()
    {
        Debug.Log("Entering SelectUnitActionState");
        player.UnitActionMenu.SetActive(true);
    }
    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            HandleMenuCursorInput();
        }

    }
    public void Exit()
    {
        Debug.Log("Exiting SelectUnitActionState");
        player.UnitActionMenu.SetActive(false);
    }
    private void HandleMenuCursorInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            NextMenuButton();
            Debug.Log(player.UnitActionMenuButtons[currentMenuButtonIndex]);
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            PreviousMenuButton();
            Debug.Log(player.UnitActionMenuButtons[currentMenuButtonIndex]);
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.X))
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
        }
    }

    private void NextMenuButton()
    {
        currentMenuButtonIndex += 1;
        if (currentMenuButtonIndex >= player.UnitActionMenuButtons.Length) currentMenuButtonIndex = 0;
        Vector3 buttonPosition = player.UnitActionMenuButtons[currentMenuButtonIndex].transform.position;
        selectUnitActionCursor.position = new Vector3(selectUnitActionCursor.position.x, buttonPosition.y, selectUnitActionCursor.position.z);
    }

    private void PreviousMenuButton()
    {
        currentMenuButtonIndex -= 1;
        if (currentMenuButtonIndex < 0) currentMenuButtonIndex = player.UnitActionMenuButtons.Length - 1;
        Vector3 buttonPosition = player.UnitActionMenuButtons[currentMenuButtonIndex].transform.position;
        selectUnitActionCursor.position = new Vector3(selectUnitActionCursor.position.x, buttonPosition.y, selectUnitActionCursor.position.z);
    }
}

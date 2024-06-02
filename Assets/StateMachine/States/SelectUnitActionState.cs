using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        InitializeButtonEvents();
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

        player.PlayerStateMachine.PreviousState = this;
    }
    private void HandleMenuCursorInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            NextMenuButton();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            PreviousMenuButton();
            timer = timeoutLength;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            player.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Button>().onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
        }
    }

    private void NextMenuButton()
    {
        currentMenuButtonIndex += 1;
        if (currentMenuButtonIndex >= player.UnitActionMenuButtons.Count) currentMenuButtonIndex = 0;
        Vector3 buttonPosition = player.UnitActionMenuButtons[currentMenuButtonIndex].transform.position;
        selectUnitActionCursor.position = new Vector3(selectUnitActionCursor.position.x, buttonPosition.y, selectUnitActionCursor.position.z);
    }

    private void PreviousMenuButton()
    {
        currentMenuButtonIndex -= 1;
        if (currentMenuButtonIndex < 0) currentMenuButtonIndex = player.UnitActionMenuButtons.Count - 1;
        Vector3 buttonPosition = player.UnitActionMenuButtons[currentMenuButtonIndex].transform.position;
        selectUnitActionCursor.position = new Vector3(selectUnitActionCursor.position.x, buttonPosition.y, selectUnitActionCursor.position.z);
    }

    private void InitializeButtonEvents()
    {
        foreach (GameObject go in player.UnitActionMenuButtons)
        {
            SetupButtonEventListeners(go);
        }
    }
    private void SetupButtonEventListeners(GameObject go)
    {
        if (go.name == "Attack Button")
        { 
            go.GetComponent<Button>().onClick.AddListener(OnAttackButtonClicked);
        }
        else if (go.name == "End Turn Button")
        {
            go.GetComponent<Button>().onClick.AddListener(OnEndTurnButtonClicked);
        }
    }
    private void OnAttackButtonClicked() 
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectAttackTargetState);
    }
    private void OnEndTurnButtonClicked()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
}

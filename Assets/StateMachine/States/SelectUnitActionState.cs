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

        Color color;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
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

        if (player.transform.position != player.PlayerUnit.transform.position)
        {
            player.PlayerUnit.TurnOffMovementRange();
        }


        player.PlayerStateMachine.PreviousState = this;
    }
    private void HandleMenuCursorInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            PreviousMenuButton();
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            NextMenuButton();
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
        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
        currentMenuButtonIndex += 1;
        if (currentMenuButtonIndex >= player.UnitActionMenuButtons.Count) currentMenuButtonIndex = 0;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
    }

    private void PreviousMenuButton()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
        currentMenuButtonIndex -= 1;
        if (currentMenuButtonIndex < 0) currentMenuButtonIndex = player.UnitActionMenuButtons.Count - 1;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
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
        if (go.name == "AttackButton")
        { 
            go.GetComponent<Button>().onClick.AddListener(OnAttackButtonClicked);
        }
        else if (go.name == "ItemButton")
        {
            go.GetComponent<Button>().onClick.AddListener(OnItemButtonClicked);
        }
        else if (go.name == "WaitButton")
        {
            go.GetComponent<Button>().onClick.AddListener(OnWaitButtonClicked);
        }
        else if (go.name == "EndTurnButton")
        {
            go.GetComponent<Button>().onClick.AddListener(OnEndTurnButtonClicked);
        }
    }
    private void OnAttackButtonClicked() 
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectAttackTargetState);
    }
    private void OnItemButtonClicked()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
    private void OnWaitButtonClicked()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
    private void OnEndTurnButtonClicked()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUnitActionState : IState
{
    private PlayerController player;
    private int currentMenuButtonIndex;
    private float timer;
    private float timeoutLength;
    public SelectUnitActionState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;

        foreach (PlayerUnit playerUnit in player.UnitManager.playerUnitList)
        {
            InitializeButtonEvents(playerUnit);
        }
    }
    public void Enter()
    {
        Debug.Log("Entering SelectUnitActionState");
        player.PlayerUnit.UnitActionsPanel.SetActive(true);

        Color color;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.PlayerUnit.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
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
        player.PlayerUnit.UnitActionsPanel.SetActive(false);

        if (player.transform.position != player.PlayerUnit.transform.position)
        {
            player.PlayerUnit.TurnOffMovementRange();
        }

        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        foreach (GameObject gameObject in player.PlayerUnit.UnitActionMenuButtons)
        {
            gameObject.GetComponent<Image>().color = color;
            
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
            player.PlayerUnit.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Button>().onClick.Invoke();
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
        player.PlayerUnit.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
        currentMenuButtonIndex += 1;
        if (currentMenuButtonIndex >= player.PlayerUnit.UnitActionMenuButtons.Count) currentMenuButtonIndex = 0;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.PlayerUnit.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
    }

    private void PreviousMenuButton()
    {
        Color color;
        ColorUtility.TryParseHtmlString(Constants.DEFAULT_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.PlayerUnit.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
        currentMenuButtonIndex -= 1;
        if (currentMenuButtonIndex < 0) currentMenuButtonIndex = player.PlayerUnit.UnitActionMenuButtons.Count - 1;
        ColorUtility.TryParseHtmlString(Constants.SELECTED_UNIT_ACTION_UI_BUTTON_COLOR, out color);
        player.PlayerUnit.UnitActionMenuButtons[currentMenuButtonIndex].GetComponent<Image>().color = color;
    }

    private void InitializeButtonEvents(PlayerUnit player)
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
        else if (go.name == "MoveButton")
        {
            go.GetComponent<Button>().onClick.AddListener(OnMoveButtonClicked);
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
    private void OnMoveButtonClicked() 
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectMovePositionState);
    }
    private void OnItemButtonClicked()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
    private void OnWaitButtonClicked()
    {
        player.PlayerUnit.UnitHasCompletedAllActions();
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
    }
    private void OnEndTurnButtonClicked()
    {
        player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.playerToEnemyTurnState);
    }
}

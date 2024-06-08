using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerToEnemyTurnState : IState
{
    private PlayerController player;
    private TextMeshProUGUI transitionText;
    private float targetFontSize = 40f;
    private float timeOut = 0.1f;
    private float timer;
    private float transitionTimer;
    public PlayerToEnemyTurnState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        // if there are no enemies left, skip to win screen
        if (player.UnitManager.enemyUnitList.Count == 0) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.enemyToPlayerTurnState);

        player.PlayerToEnemyTurnTransition.SetActive(true);
        transitionText = player.PlayerToEnemyTurnTransition.GetComponent<TextMeshProUGUI>();
        transitionText.fontSize = 1; 
        timer = timeOut;
        transitionTimer = 1f;
    }

    public void Exit()
    {
        Debug.Log("Exiting Player To Enemy Turn State");
        player.PlayerToEnemyTurnTransition.SetActive(false);
    }

    public void Update()
    {
        if (transitionText.fontSize < targetFontSize)
        {
            if (timer <= 0)
            {
                transitionText.fontSize += 4;
                timer = timeOut;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else
        {
            if (transitionTimer > 0)
            {
                transitionTimer -= Time.deltaTime;
            }
            else
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.enemyBattlePhaseState);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyToPlayerTurnState : IState
{
    private PlayerController player;
    private TextMeshProUGUI transitionText;
    private float targetFontSize = 40f;
    private float timeOut = 0.1f;
    private float timer;
    private float transitionTimer;
    public EnemyToPlayerTurnState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        player.EnemyToPlayerTurnTransition.SetActive(true);
        transitionText = player.EnemyToPlayerTurnTransition.GetComponent<TextMeshProUGUI>();
        transitionText.fontSize = 1; 
        timer = timeOut;
        transitionTimer = 1f;
    }

    public void Exit()
    {
        player.EnemyToPlayerTurnTransition.SetActive(false);
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
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
            }
        }
    }
}

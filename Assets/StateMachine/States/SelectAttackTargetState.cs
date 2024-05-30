using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAttackTargetState : IState
{
    private PlayerController player;

    private float timer;
    private float timeoutLength;
    public SelectAttackTargetState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
    }
    public void Enter()
    {
        Debug.Log("Entering SelectAttackTargetState");
    }
    public void Update ()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            HandlePlayerMovement();
        }

        if (Input.GetKeyDown(KeyCode.X)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectUnitActionState);
        if (Input.GetKeyDown(KeyCode.Z)) AttackTargetSelected();
    }
    public void Exit()
    {

    }

    private void HandlePlayerMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.Translate(Vector2.up);
            CheckValidPosition(Vector2.up);
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Translate(Vector2.left);
            CheckValidPosition(Vector2.left);
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.Translate(Vector2.down);
            CheckValidPosition(Vector2.down);
            timer = timeoutLength;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Translate(Vector2.right);
            CheckValidPosition(Vector2.right);
            timer = timeoutLength;
        }
    }
    private void CheckValidPosition(Vector2 direction)
    {
        if (player.PlayerUnit == null) return;

        // Add (0, 0, 1) because cursor is has z position of -1, while all valid positions have z position of 0
        if (player.PlayerUnit.validPositions.Contains(player.transform.position + new Vector3(0, 0, 1))) return;

        player.transform.Translate(direction * -1);
    }
    private void AttackTargetSelected()
    {

    }
}


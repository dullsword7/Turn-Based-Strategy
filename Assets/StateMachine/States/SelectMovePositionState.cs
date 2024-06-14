using System.Collections;
using UnityEngine;

public class SelectMovePositionState : IState
{
    private PlayerController player;

    private float timer;
    private float timeoutLength;
    private bool lockControls;

    private BattleUnit currentBattleUnit;
    private BattleUnit previousBattleUnit;
    public SelectMovePositionState(PlayerController player)
    {
        this.player = player;
        timeoutLength = 0.2f;
    }
    public void Enter()
    {
        Debug.Log("Entering SelectMovePositionState");
    }

    public void Exit()
    {
        Debug.Log("Exiting SelectMovePositionState");
        lockControls = false;
    }

    public void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (lockControls) return;
            HandlePlayerMovement();
        }

        if (lockControls) return;
        if (Input.GetKeyDown(KeyCode.X)) player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.selectUnitActionState);
        if (Input.GetKeyDown(KeyCode.Z)) MovePositionSelected();
    }

    /// <summary>
    /// Handles all player input.
    /// </summary>
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

    /// <summary>
    /// When a tile has been selected, moves to that tile.
    /// </summary>
    private void MovePositionSelected()
    {
        Collider2D col = Physics2D.OverlapPoint(player.transform.position);
        if (col == null)
        {
            lockControls = true;
            player.PlayerUnit.TurnOffMovementRange();
            player.PlayerUnit.TurnOffInfo();
            player.StartCoroutine(MoveToPosition());
        }
    }

    /// <summary>
    /// Moves to the selected position and marks the unit's turn as completed.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToPosition()
    {
        yield return player.StartCoroutine(player.PlayerUnit.TryMoveToPosition(player.transform.position));
        if (player.PlayerUnit.TryMovementSucess)
        {
            player.PlayerUnit.UnitHasCompletedAllActions();
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.viewMapState);
        }
        else
        {
            yield break;
        }
    }

    /// <summary>
    /// Checks if the player input will be outside the select unit's movement range. If it is, then undo the movement.
    /// </summary>
    /// <param name="direction">the opposite direction to send the player cursor</param>
    private void CheckValidPosition(Vector2 direction)
    {
        if (player.PlayerUnit == null) return;

        if (player.PlayerUnit.AllTilePositionsInMovementRange.Contains(player.transform.position)) return;

        player.transform.Translate(direction * -1);
    }
}

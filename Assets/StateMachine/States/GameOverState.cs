using UnityEngine;

public class GameOverState : IState
{
    PlayerController player;
    public GameOverState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entering GameOverState");
    }

    public void Exit() { }

    public void Update() { }
}

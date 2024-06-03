using System;

[Serializable]
public class StateMachine
{
    public IState CurrentState { get; private set; }
    public IState PreviousState { get; set; }

    public ViewMapState viewMapState;
    public SelectUnitActionState selectUnitActionState;
    public SelectAttackTargetState selectAttackTargetState;
    public AttackSuccessfulState attackSuccessfulState;
    public EnemyBattlePhaseState enemyBattlePhaseState;
    public PlayerToEnemyTurnState playerToEnemyTurnState;
    public EnemyToPlayerTurnState enemyToPlayerTurnState;

    public StateMachine(PlayerController player)
    {
        this.viewMapState = new ViewMapState(player);
        this.selectUnitActionState = new SelectUnitActionState(player);
        this.selectAttackTargetState = new SelectAttackTargetState(player);
        this.attackSuccessfulState = new AttackSuccessfulState(player);
        this.enemyBattlePhaseState = new EnemyBattlePhaseState(player);
        this.playerToEnemyTurnState = new PlayerToEnemyTurnState(player);
        this.enemyToPlayerTurnState = new EnemyToPlayerTurnState(player);
    }
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        PreviousState = CurrentState;
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}
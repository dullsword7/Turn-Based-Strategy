using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public PlayerUnit player;
    public List<EnemyUnit> enemyUnitList;
    public List<PlayerUnit> playerUnitList;
    public List<GameObject> enemies;
    public List<GameObject> playerUnits;
    public List<PlayerUnit> playerUnitsWithActionsLeft;

    [SerializeField] private GameObject gameOverCanvas;
    private Action gameOver;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUnit>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        playerUnits = GameObject.FindGameObjectsWithTag("Player").ToList();

        foreach (GameObject playerUnit in playerUnits)
        {
            playerUnitList.Add(playerUnit.GetComponent<PlayerUnit>());
        }
        foreach (GameObject enemy in enemies)
        {
            enemyUnitList.Add(enemy.GetComponent<EnemyUnit>());
        }


        foreach (EnemyUnit enemyUnit in enemyUnitList)
        {
            enemyUnit.BattleUnitDeath += DestroyBattleUnitOnDeath;
        }
        foreach (PlayerUnit playerUnit in playerUnitList)
        {
            playerUnit.BattleUnitDeath += DestroyBattleUnitOnDeath;
        }

        playerUnitsWithActionsLeft = new List<PlayerUnit>(playerUnitList);

        gameOver += GameOver;
    }

    // need to unsubscribe each enemy and player unit from DestoyBattleUnitOnDeath
    private void OnDestroy() 
    {
        gameOver -= GameOver;
    }

    /// <summary>
    /// Makes a game object inactive once their health has reached 0.
    /// </summary>
    /// <param name="gameObject">the game object to make inactive</param>
    public void DestroyBattleUnitOnDeath(GameObject gameObject)
    {
        BattleUnit battleUnit = gameObject.GetComponent<BattleUnit>();
        if (battleUnit is PlayerUnit playerUnit)
        {
            Debug.Log("Player Unit Dead");
            playerUnitList.Remove(playerUnit);
            playerUnitsWithActionsLeft.Remove(playerUnit);
            if (playerUnitList.Count < 1) gameOver?.Invoke();
        }
        if (battleUnit is EnemyUnit enemyUnit)
        {
            Debug.Log("Enemy Unit Dead");
            enemyUnitList.Remove(enemyUnit);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Checks if all player units are dead.
    /// </summary>
    /// <returns>true if all player units are dead, false otherwise</returns>
    public bool AllPlayerUnitsDead()
    {
        return playerUnitList.Count == 0;
    }

    /// <summary>
    /// Checks if all enemy units are dead.
    /// </summary>
    /// <returns>true if all enemy units are dead, false otherwise</returns>
    public bool AllEnemyUnitsDead()
    {
        return enemyUnitList.Count == 0;
    }

    /// <summary>
    /// Checks if there are any player units with actions left.
    /// </summary>
    /// <returns>true if no player units can act, false otherwise</returns>
    public bool NoPlayerUnitsWithActionsLeft()
    {
        foreach (PlayerUnit playerUnit in playerUnitsWithActionsLeft)
        {
            // if a player unit with an action left exists
            if (!playerUnit.noMoreActions) return false;
        }
        return true;
    }

    /// <summary>
    /// Allows each player unit to act again.
    /// </summary>
    public void ResetPlayerUnitActions()
    {
        foreach (PlayerUnit playerUnit in playerUnitsWithActionsLeft)
        {
            playerUnit.noMoreActions = false;
        }
    }

    /// <summary>
    /// Fades in the game over canvas if all player units are dead.
    /// </summary>
    public void GameOver()
    {
        Debug.Log("Game Over");
        gameOverCanvas.GetComponent<FadeInCanvasGroup>().FadeInGameOver();
    }
}

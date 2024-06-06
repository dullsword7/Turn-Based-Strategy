using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public PlayerUnit player;
    public List<EnemyUnit> enemyUnitList;
    public List<GameObject> enemies;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUnit>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        foreach (GameObject enemy in enemies)
        {
            enemyUnitList.Add(enemy.GetComponent<EnemyUnit>());
        }


        if (player != null)
        {
            player.BattleUnitDeath += DestroyBattleUnitOnDeath;
        }

        foreach (EnemyUnit enemyUnit in enemyUnitList)
        {
            enemyUnit.BattleUnitDeath += DestroyBattleUnitOnDeath;
        }
    }
    private void OnDestroy()
    {
        if (player != null)
        {
            player.BattleUnitDeath -= DestroyBattleUnitOnDeath;
        }
        foreach (EnemyUnit enemyUnit in enemyUnitList)
        {
            enemyUnit.BattleUnitDeath -= DestroyBattleUnitOnDeath;
        }
    }

    public void DestroyBattleUnitOnDeath(GameObject gameObject)
    {
        BattleUnit battleUnit = gameObject.GetComponent<BattleUnit>();
        if (battleUnit is PlayerUnit)
        {
            Debug.Log("Game Over");
        }
        if (battleUnit is EnemyUnit)
        {
            Debug.Log("Enemy Unit Dead");
            enemyUnitList.Remove(battleUnit as EnemyUnit);
        }
        gameObject.SetActive(false);
    }
}

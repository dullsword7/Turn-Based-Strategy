using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private PlayerUnit player;
    private List<GameObject> enemies;
    private List<EnemyUnit> enemyUnitList;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUnit>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //foreach (GameObject enemy in enemies)
        //{
            //enemyUnitList.Add(enemy.GetComponent<EnemyUnit>());
        //}


        if (player != null)
        {
            player.BattleUnitDeath += DestroyUnitOnDeath;
        }
        //foreach (EnemyUnit enemyUnit in enemyUnitList)
        //{

        //}
    }
    private void OnDestroy()
    {
        if (player != null)
        {
            player.BattleUnitDeath -= DestroyUnitOnDeath;
        }
    }

    public void DestroyUnitOnDeath()
    {
        Debug.Log("You are dead");
        player.gameObject.SetActive(false);
    }
}

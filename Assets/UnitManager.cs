using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public PlayerUnit player;
    private void Awake()
    {
        if (player != null)
        {
            player.PlayerUnitDeath += DestroyUnitOnDeath;
        }
    }
    private void OnDestroy()
    {
        if (player != null)
        {
            player.PlayerUnitDeath -= DestroyUnitOnDeath;
        }
    }

    public void DestroyUnitOnDeath()
    {
        Debug.Log("You are dead");
        player.gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleUnit
{
    //public HashSet<Vector3> AllCrossableTilePositionsInMovementRange { get; set;}
    public void InitializeBattleStats();
    public IEnumerator ReceiveDamage(float damageAmount, BattleUnit battleUnit);
    public Vector3 ClosestValidAttackPosition(Vector3 attackTargetPosition);
    public IEnumerator TryMoveToPosition(Vector3 attackTargetPosition, Action onComplete = null);
}

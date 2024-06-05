using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleUnit
{
    public HashSet<Vector3> ValidPositions { get; set;}
    public void InitalizeBattleStats();
    public IEnumerator ReceiveDamage(float damageAmount, BattleUnit battleUnit, Action onComplete);
    public Vector3 ValidAttackPositions(Vector3 attackTargetPosition);
    public IEnumerator MoveToPosition(Vector3 attackTargetPosition, Action onComplete = null);
}

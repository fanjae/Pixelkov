using System;
using UnityEngine;

public static class DamageHandler
{
    public static event Action<Vector2, int> OnPlayerAttack; // 플레이어가 공격 성공 시 호출
    public static event Action<Vector2, int> OnEnemyAttack; // 적이 공격 성공 시 호출

    public static void PlayerAttack(Vector2 targetPos,  int damage)
    {
        OnPlayerAttack?.Invoke(targetPos, damage);
    }
    public static void EnemyAttack(Vector2 targetPos,  int damage)
    {
        OnEnemyAttack?.Invoke(targetPos, damage);
    }
}

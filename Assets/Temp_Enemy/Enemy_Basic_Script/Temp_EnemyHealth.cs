using System.Collections;
using UnityEngine;

public class Temp_EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHp = 3;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Temp_EnemyUI enemyUI;
    [SerializeField] private Temp_EnemyAnimationController animationController;

    [Header("Death")]
    [SerializeField] private float deathDelay = 2.5f;

    private int currentHp;
    private bool isDead;
    private void Awake()
    {
        currentHp = maxHp;

        if (enemyUI != null) enemyUI.Initialize(maxHp);
    }

    public void TakeDamage(int damage)
    {
        if(isDead) return;
        currentHp -= damage;

        if (enemyUI != null) enemyUI.SetHP(currentHp);
        if (currentHp <= 0) Die();
        else animationController?.PlayDamaged();
    }

    private void Die()
    {
        if (isDead) return ;

        isDead = true;

        StartCoroutine(DeathRoutine());

    }
    private IEnumerator DeathRoutine()
    {
        animationController?.PlayDeath();

        // 죽은 뒤 더 이상 움직이거나 공격하지 않게 처리
        Temp_MeleeEnemyAI meleeAI = GetComponent<Temp_MeleeEnemyAI>();
        if (meleeAI != null) meleeAI.enabled = false;

        Temp_RangedEnemyAI rangedAI = GetComponent<Temp_RangedEnemyAI>();
        if (rangedAI != null) rangedAI.enabled = false;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(deathDelay);

        if (rewardPrefab != null)
            Instantiate(rewardPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}

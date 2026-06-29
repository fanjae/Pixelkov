using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}
public class Temp_MeleeEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float attackRange = 0.8f;

    [Header("Attack")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.4f;

    [SerializeField] private Temp_EnemyAnimationController animationController;

    private Rigidbody2D rb;
    private EnemyState state;
    private bool isAttacking;
    private bool isFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (animationController == null)
        {
            animationController = GetComponentInChildren<Temp_EnemyAnimationController>();
        }
    }

    private void FixedUpdate()
    {
        if (player == null || isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > detectRange)
        {
            ChangeState(EnemyState.Idle);
            rb.linearVelocity = Vector2.zero;

            if (animationController != null)
                animationController.SetMove(false);

            return;
        }

        if (distance > attackRange)
        {
            ChangeState(EnemyState.Chase);
            ChasePlayer();

            if (animationController != null)
                animationController.SetMove(true);
            return;
        }

        ChangeState(EnemyState.Attack);
        rb.linearVelocity = Vector2.zero;

        if (animationController != null)
            animationController.SetMove(false);

        StartCoroutine(AttackRoutine());
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        SetFacingDirection(direction);

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        if (animationController != null)
            animationController.PlayAttack();

        yield return new WaitForSeconds(attackDelay);

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position,attackRadius,playerLayer);

        if (hit != null)
        {
            PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
    private void ChangeState(EnemyState nextState)
    {
        if (state == nextState) return;
        state = nextState;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRange);

        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    private void SetFacingDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < 0.01f) return;

        bool shouldFaceRight = direction.x > 0f;
        if (isFacingRight == shouldFaceRight) return;

        isFacingRight = shouldFaceRight;

        Vector3 scale = visualRoot.localScale;

        // 기본이 왼쪽을 보는 에셋 기준
        scale.x = shouldFaceRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);

        visualRoot.localScale = scale;
    }
}
using System.Collections;
using UnityEngine;

public class Temp_RangedEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectRange = 7f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float keepDistance = 2.5f;

    [Header("Attack")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackDelay = 0.25f;
    [SerializeField] private float attackCooldown = 1.5f;

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
        Vector2 direction = (player.position - transform.position).normalized;

        if (distance > detectRange)
        {
            ChangeState(EnemyState.Idle);
            rb.linearVelocity = Vector2.zero;
            animationController?.SetMove(false);
            return;
        }

        SetFacingDirection(direction);

        if (distance > attackRange)
        {
            ChangeState(EnemyState.Chase);
            ChasePlayer(direction);
            animationController?.SetMove(true);
            return;
        }

        if (distance < keepDistance)
        {
            ChangeState(EnemyState.Chase);
            MoveAwayFromPlayer(direction);
            animationController?.SetMove(true);
            return;
        }

        ChangeState(EnemyState.Attack);
        rb.linearVelocity = Vector2.zero;
        animationController?.SetMove(false);

        StartCoroutine(AttackRoutine());
    }

    private void ChasePlayer(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void MoveAwayFromPlayer(Vector2 directionToPlayer)
    {
        Vector2 awayDirection = -directionToPlayer;
        rb.MovePosition(rb.position + awayDirection * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        animationController?.PlayAttack();

        yield return new WaitForSeconds(attackDelay);

        ShootArrow();

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void ShootArrow()
    {
        if (arrowPrefab == null || firePoint == null || player == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;

        GameObject arrowObj = Instantiate(
            arrowPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Temp_EnemyArrow arrow = arrowObj.GetComponent<Temp_EnemyArrow>();
        if (arrow != null)
        {
            arrow.SetDirection(direction);
        }
    }

    private void ChangeState(EnemyState nextState)
    {
        if (state == nextState) return;
        state = nextState;
    }

    private void SetFacingDirection(Vector2 direction)
    {
        if (visualRoot == null) return;
        if (Mathf.Abs(direction.x) < 0.01f) return;

        bool shouldFaceRight = direction.x > 0f;
        if (isFacingRight == shouldFaceRight) return;

        isFacingRight = shouldFaceRight;

        Vector3 scale = visualRoot.localScale;

        // 기본이 왼쪽을 보는 에셋 기준
        scale.x = shouldFaceRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);

        visualRoot.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, keepDistance);
    }
}
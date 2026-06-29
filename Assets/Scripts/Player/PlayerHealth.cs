using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour, IEnmeyController
{
    [Header("플레이어 체력")]
    [SerializeField] private int maxHealth = 5;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    [Header("사망 설정")]
    [SerializeField] private float destroyDelay = 3f;

    // 현재 체력
    private int currentHealth;
    private bool isHit;

    public bool IsHit => isHit;
    // 이미 죽었는지 확인
    private bool isDead;

    private Player player;
    private PlayerShooter playerShooter;
    private Rigidbody2D rb;
    private PlayerAnimationController animationController;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        // 게임 시작 시 최대 체력으로 설정
        currentHealth = maxHealth;

        // Player에 붙어 있는 컴포넌트 찾기
        player = GetComponent<Player>();
        playerShooter = GetComponent<PlayerShooter>();
        rb = GetComponent<Rigidbody2D>();

        // 자식 오브젝트에서 애니메이션 제어 스크립트 찾기
        animationController =
            GetComponentInChildren<PlayerAnimationController>();

        // Inspector에서 Animator를 연결하지 않았다면
        // Player의 자식 오브젝트에서 자동으로 찾음
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            Debug.Log(
                "PlayerHealth가 찾은 Animator: " +
                animator.gameObject.name
            );

            if (animator.runtimeAnimatorController != null)
            {
                Debug.Log(
                    "연결된 Controller: " +
                    animator.runtimeAnimatorController.name
                );
            }
        }
        else
        {
            Debug.LogWarning(
                "PlayerHealth에서 Animator를 찾지 못했습니다."
            );
        }
    }
    public void Heal(int amount)
    {
        if (isDead)
        {
            return;
        }

        if (amount <= 0)
        {
            return;
        }

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log(
            $"플레이어 체력 회복: {currentHealth} / {maxHealth}"
        );
    }

    // 플레이어가 데미지를 받을 때 외부에서 호출
    public void TakeDamage(int damage)
    {
        // 이미 죽은 상태라면 추가 데미지를 받지 않음
        if (isDead)
        {
            return;
        }

        // 0 이하의 데미지는 무시
        if (damage <= 0)
        {
            return;
        }

        // 회피 무적 상태라면 데미지를 받지 않음
        if (player != null && player.IsInvincible)
        {
            Debug.Log(
                "회피 무적 상태라 데미지를 받지 않았습니다."
            );

            return;
        }

        // 체력 감소
        currentHealth -= damage;

        // 체력이 음수가 되지 않도록 처리
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(
            $"플레이어 현재 체력: {currentHealth} / {maxHealth}"
        );

        // 체력이 0이 되면 사망
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        if(animator !=null)
        {
            animator.ResetTrigger("3_Damaged");
            animator.SetTrigger("3_Damaged");
        }
        if (animator != null)
        {
            StartCoroutine(HitCoroutine());
        }
    }
    private IEnumerator HitCoroutine()
    {
        isHit = true;

        animator.ResetTrigger("3_Damaged");
        animator.SetTrigger("3_Damaged");

        // 피격 모션 길이에 맞게 조절
        yield return new WaitForSeconds(0.5f);

        isHit = false;
    }
    private void Die()
    {
        // 중복 사망 처리 방지
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("플레이어 사망");

        // 물리 이동 즉시 정지
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 이동과 회피 스크립트 비활성화
        if (player != null)
        {
            player.enabled = false;
        }

        // 발사 스크립트 비활성화
        if (playerShooter != null)
        {
            playerShooter.enabled = false;
        }

        // 이동 애니메이션을 계속 변경하는 스크립트 비활성화
        if (animationController != null)
        {
            animationController.enabled = false;
        }

        // 죽음 애니메이션 실행
        if (animator != null)
        {
            animator.speed = 1f;

            // 기존 이동과 회피 상태를 종료
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsDodging", false);

            // Die Trigger를 다시 실행할 수 있도록 초기화
            animator.ResetTrigger("Die");
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.LogWarning(
                "Animator가 없어서 죽음 애니메이션을 실행할 수 없습니다."
            );
        }

        // 죽음 모션이 재생된 후 Player 오브젝트 삭제
        Destroy(gameObject, destroyDelay);
    }
}
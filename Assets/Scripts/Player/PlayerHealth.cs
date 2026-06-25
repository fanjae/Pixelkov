using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 체력")]
    [SerializeField] private int maxHealth = 5;

    [Header("애니메이터")]
    [SerializeField] private Animator animator;

    // 현재 체력
    private int currentHealth;

    // 이미 죽었는지 확인
    private bool isDead;

    private Player player;

    private void Awake()
    {
        // 게임 시작 시 최대 체력으로 설정
        currentHealth = maxHealth;
        player = GetComponent<Player>();

        // Inspector에서 Animator를 연결하지 않았다면
        // Player의 자식 오브젝트에서 자동으로 찾음
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            Debug.Log("찾은 Animator: " + animator.gameObject.name);

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

    // 플레이어가 데미지를 받을 때 외부에서 호출
    public void TakeDamage(int damage)
    {
        // 이미 죽었다면 추가 데미지를 받지 않음
        if (player != null && player.IsInvincible)
        {
            Debug.Log("회피 무적 상태라 데미지를 받지 않았습니다.");
            return;
        }
        // 체력 감소
        currentHealth -= damage;

        // 체력이 음수가 되지 않도록 처리
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("플레이어 현재 체력: " + currentHealth);

        // 체력이 0이 되면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 중복 사망 처리 방지
        if (isDead)
            return;

        isDead = true;

        Debug.Log("플레이어 사망");

        // 플레이어 이동 즉시 멈추기
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 이동 스크립트 비활성화
        Player player = GetComponent<Player>();

        if (player != null)
        {
            player.enabled = false;
        }

        // 공격 스크립트 비활성화
        PlayerShooter playerShooter =
            GetComponent<PlayerShooter>();

        if (playerShooter != null)
        {
            playerShooter.enabled = false;
        }

        // 평소 걷기·대기 애니메이션을 변경하는 스크립트 비활성화
        PlayerAnimationController animationController =
            GetComponentInChildren<PlayerAnimationController>();

        if (animationController != null)
        {
            animationController.enabled = false;
        }

        // 죽는 애니메이션 실행
        if (animator != null)
        {
            animator.speed = 1f;

            // Animator Parameters에 있는 Die Trigger 실행
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.LogWarning(
                "Animator가 없어서 죽는 애니메이션을 실행할 수 없습니다."
            );
        }

        // 죽는 애니메이션이 재생된 후 플레이어 삭제
        Destroy(gameObject, 3f);
    }
}
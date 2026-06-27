using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform aimPivot;

    [Header("시야 설정")]
    [SerializeField] private float closeVisionRadius = 2f;
    [SerializeField] private float viewDistance = 7f;
    [SerializeField] private float viewAngle = 70f;

    [Header("벽 설정")]
    [SerializeField] private LayerMask wallLayer;

    private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        // Enemy 본체와 자식에 있는 모든 Sprite Renderer를 가져옴
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (player == null || aimPivot == null)
            return;

        Vector2 playerPosition = player.position;
        Vector2 enemyPosition = transform.position;

        Vector2 directionToEnemy = enemyPosition - playerPosition;
        float distanceToEnemy = directionToEnemy.magnitude;

        // 플레이어 주변의 원형 시야 안인지 확인
        bool isInsideCloseVision =
            distanceToEnemy <= closeVisionRadius;

        // 조준 방향과 Enemy 방향의 각도 확인
        float angleToEnemy = Vector2.Angle(
            aimPivot.up,
            directionToEnemy.normalized
        );

        // 부채꼴 시야 안인지 확인
        bool isInsideCone =
            distanceToEnemy <= viewDistance &&
            angleToEnemy <= viewAngle * 0.5f;

        // 플레이어와 Enemy 사이에 벽이 있는지 확인
        bool isBlockedByWall = Physics2D.Linecast(
            playerPosition,
            enemyPosition,
            wallLayer
        );

        bool isVisible =
            (isInsideCloseVision || isInsideCone) &&
            !isBlockedByWall;

        SetVisible(isVisible);
    }

    private void SetVisible(bool visible)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = visible;
        }
    }
}
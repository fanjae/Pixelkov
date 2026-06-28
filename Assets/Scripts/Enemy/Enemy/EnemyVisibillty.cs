using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyVisibility : MonoBehaviour
{
    [Header("플레이어 설정")]
    [SerializeField] private Transform player;

    [Header("시야 조명")]
    [SerializeField] private Transform visionTransform; // 부채꼴 시야 방향 Transform
    [SerializeField] private Light2D coneLight;         // 부채꼴 시야 범위/각도 Light2D
    [SerializeField] private Light2D circleLight;       // 원형 시아 반경 Light2D

    [Header("벽 설정")]
    [SerializeField] private LayerMask wallLayer; // 벽 레이어

    private SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        // 적 오브젝트 및 자식 오브젝트에 있는 Sprite 모두 가져옴
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Inspector에 미 연결시 직접 연결
        if (coneLight == null && visionTransform != null) coneLight = visionTransform.GetComponent<Light2D>();
    }

    private void Update()
    {
        if (player == null || visionTransform == null) return;

        Vector2 playerPosition = player.position;
        Vector2 enemyPosition = transform.position;

        Vector2 directionToEnemy = enemyPosition - playerPosition;
        float distanceToEnemy = directionToEnemy.magnitude;

        // 플레이어와 적 사이에 벽 검사. (있으면 시야가 막힌 것으로 판단)
        bool isBlockedByWall = Physics2D.Linecast(playerPosition,enemyPosition,wallLayer);

        bool hasLineOfSight = !isBlockedByWall;

        // 근거리 원형 시야 체크
        bool isInsideCloseVision = IsInsideCircleVision(distanceToEnemy) && hasLineOfSight;

        // 부채꼴 시야 체크
        bool isInsideCone = IsInsideConeVision(directionToEnemy, distanceToEnemy) &&  hasLineOfSight;

        // 하나라도 만족시 적 보임
        SetVisible(isInsideCloseVision || isInsideCone);
    }
    private bool IsInsideCircleVision(float distanceToEnemy)
    {
        // 원형 시야 light2D 가 없으면, 원형 시야 실패
        if (circleLight == null) return false; 

        // Light2D의 Outer Radius를 근거리 시야 반경으로 사용
        float radius = circleLight.pointLightOuterRadius;

        return distanceToEnemy <= radius;
    }

    private bool IsInsideConeVision(Vector2 directionToEnemy, float distanceToEnemy)
    {
        // 부채꼴 시야 Light2D 가 없으면 부채꼴 시야 실패
        if (coneLight == null) return false;

        // Light2D의 Outer Radius를 거리, Outer Angle을 각도로 처리
        float viewDistance = coneLight.pointLightOuterRadius;
        float viewAngle = coneLight.pointLightOuterAngle;

        // 현재 Vision 오브젝트의 위쪽 방향을 부채꼴 시야의 정면 방향으로 사용
        Vector2 visionDirection = visionTransform.up;

        // 시야 정면 방향과 적 방향 사이의 각 계산
        float angleToEnemy = Vector2.Angle(visionDirection,directionToEnemy.normalized);

        // 거리 조건과 각도 조건 모두 만족시 부채꼴 시야로 판단
        return distanceToEnemy <= viewDistance && angleToEnemy <= viewAngle * 0.5f;
    }

    private void SetVisible(bool visible)
    {
        // 적과 자식 오브젝트의 SpriteRender 모두 켜거나 끔.
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer == null) continue;
            spriteRenderer.enabled = visible;
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("발사 설정")]

    // 투사체가 생성되는 위치
    [SerializeField] private Transform firePoint;

    // 생성할 투사체 프리팹
    [SerializeField] private Projectile projectilePrefab;

    // 한 번 발사한 후 다음 발사가 가능해질 때까지의 시간
    [SerializeField] private float fireDelay = 0.2f;

    // 마우스 화면 좌표를 월드 좌표로 변환할 때 사용할 카메라
    private Camera mainCamera;

    
    private float fireTimer;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 매 프레임마다 발사 대기 시간을 증가시킴
        fireTimer += Time.deltaTime;

        // 마우스 입력 또는 카메라를 찾지 못하면 발사 불가
        if (Mouse.current == null || mainCamera == null)
            return;

        // 마우스 왼쪽 버튼을 누르고 있는 동안 계속 발사 시도
        if (Mouse.current.leftButton.isPressed)
        {
            TryFire();
        }
    }

    // 실제 발사가 가능한지 확인한 후 투사체를 생성하는 메서드
    private void TryFire()
    {
        // 아직 발사 간격이 지나지 않았다면 발사하지 않음
        if (fireTimer < fireDelay)
            return;

        // FirePoint나 투사체 프리팹이 Inspector에 연결되지 않은 경우
        if (firePoint == null || projectilePrefab == null)
        {
            Debug.LogWarning(
                "FirePoint 또는 Projectile Prefab이 연결되지 않았습니다."
            );

            return;
        }

        // 현재 마우스 위치를 화면 좌표에서 월드 좌표로 변환
        Vector3 mouseWorld =
            mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

        // 2D 게임이므로 Z 좌표를 0으로 고정
        mouseWorld.z = 0f;

        // FirePoint 위치에서 마우스 위치로 향하는 방향 계산
        Vector2 direction =
            ((Vector2)mouseWorld - (Vector2)firePoint.position).normalized;

        // FirePoint 위치에 투사체 프리팹 생성
        Projectile projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

        // 생성된 투사체에 발사 방향 전달
        projectile.Launch(direction);

        // 발사했으므로 발사 타이머 초기화
        fireTimer = 0f;
    }
}
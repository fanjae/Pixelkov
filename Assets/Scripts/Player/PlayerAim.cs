using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform aimPivot;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float firePointDistance = 0.8f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera == null || aimPivot == null || firePoint == null)
            return;

        // 마우스 위치를 게임 월드 좌표로 변환
        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mousePosition.z = 0f;

        // 플레이어에서 마우스를 향하는 방향
        Vector2 aimDirection =
            ((Vector2)mousePosition - (Vector2)aimPivot.position).normalized;

        // 마우스 방향으로 조준축 회전
        float angle =
            Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        aimPivot.rotation = Quaternion.Euler(0f, 0f, angle);

        // FirePoint를 마우스 방향의 플레이어 바깥쪽에 배치
        firePoint.position =
            aimPivot.position + (Vector3)(aimDirection * firePointDistance);

        // 총알의 위쪽 방향을 마우스 방향으로 설정
        firePoint.up = aimDirection;
    }
}
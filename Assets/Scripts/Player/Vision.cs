using UnityEngine;
using UnityEngine.InputSystem;

public class VisionAim : MonoBehaviour
{
    // 현재 게임에서 사용 중인 메인 카메라
    private Camera mainCamera;

    private void Awake()
    {
        // Main Camera  가져옴
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 카메라나 마우스 입력을 없으면 멈춤
        if (mainCamera == null || Mouse.current == null)
            return;

        // 현재 마우스의 화면 좌표를 월드 좌표로 변환
        Vector3 mouseWorld =
            mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

        // 2D 게임이므로 Z 좌표는 0
        mouseWorld.z = 0f;

        // Vision 오브젝트 위치에서 마우스 방향으로 향하는 벡터 계산
        Vector2 direction =
            ((Vector2)mouseWorld - (Vector2)transform.position).normalized;

        // 마우스와 Vision 위치가 정확히 같으면 방향 계산을 하지 않음
        if (direction == Vector2.zero)
            return;

        // 방향 벡터를 회전 각도로 변환
        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Spot Light 2D의 기본 방향이 위쪽이므로 90도를 빼서
        // 시야가 마우스 방향을 바라보도록 회전
        transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f);
    }
}
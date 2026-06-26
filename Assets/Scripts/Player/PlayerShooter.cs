using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("발사 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireDelay = 0.2f;
    [SerializeField] private Animator animator;

    [Header("탄창 설정")]
    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private float reloadTime = 1.5f;

    private Camera mainCamera;
    private float fireTimer;
    private PlayerHealth playerHealth;
    private int currentAmmo;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerHealth = GetComponent<PlayerHealth>();
        currentAmmo = maxAmmo;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (Keyboard.current == null ||
            Mouse.current == null ||
            mainCamera == null)
        {
            return;
        }

        // R키를 누르면 재장전
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartCoroutine(Reload());
        }

        // 재장전 중에는 발사 불가
        if (isReloading)
        {
            return;
        }

        // 마우스 왼쪽 버튼을 누르고 있는 동안 발사
        if (Mouse.current.leftButton.isPressed)
        {
            TryFire();
        }
    }

    private void TryFire()
    {
        //피격시 공격불가
        if (playerHealth != null && playerHealth.IsHit)
        {
            return;
        }

        // 발사 간격 확인
        if (fireTimer < fireDelay)
        {
            return;
        }

        // 총알이 없으면 발사하지 않음
        if (currentAmmo <= 0)
        {
            Debug.Log("탄창이 비었습니다. R키로 재장전하세요.");
            return;
        }

        // Inspector 연결 확인
        if (firePoint == null || bulletPrefab == null)
        {
            Debug.LogWarning(
                "Fire Point 또는 Bullet Prefab이 연결되지 않았습니다."
            );
            return;
        }

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

        mouseWorldPosition.z = 0f;

        Vector2 direction =
            ((Vector2)mouseWorldPosition -
             (Vector2)firePoint.position).normalized;

        // 공격 애니메이션
        if (animator != null)
        {
            animator.ResetTrigger("Shoot");
            animator.SetTrigger("Shoot");
        }
        float angle =
    Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -45f;
        // 총알 생성
        Bullet bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.Euler(0f,0f,angle)
        );

        bullet.Launch(direction);

        // 탄약 1발 감소
        currentAmmo--;

        Debug.Log(
            $"남은 탄약: {currentAmmo} / {maxAmmo}"
        );

        fireTimer = 0f;
    }

    private IEnumerator Reload()
    {
        // 이미 재장전 중이면 다시 실행하지 않음
        if (isReloading)
        {
            yield break;
        }

        // 탄창이 이미 가득 차 있으면 재장전하지 않음
        if (currentAmmo >= maxAmmo)
        {
            yield break;
        }

        isReloading = true;

        Debug.Log("재장전 시작");

        // 나중에 재장전 애니메이션을 넣을 자리
        if (animator != null)
        {
           // animator.SetBool("IsReloading", true);
        }

        yield return new WaitForSeconds(reloadTime);

        // 탄창을 가득 채움
        currentAmmo = maxAmmo;
        isReloading = false;

        if (animator != null)
        {
          //  animator.SetBool("IsReloading", false);
        }

        Debug.Log(
            $"재장전 완료: {currentAmmo} / {maxAmmo}"
        );
    }
}
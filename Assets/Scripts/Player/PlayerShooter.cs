using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerShooter : MonoBehaviour
{
    [Header("발사 설정")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireDelay = 0.2f;

    [Header("애니메이터")]
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator weaponAnimator;

    private Camera mainCamera;
    private float fireTimer;
    private PlayerHealth playerHealth;
    private bool isReloading;
    private Player player;

    public bool IsReloading => isReloading;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerHealth = GetComponent<PlayerHealth>();
        player = GetComponent<Player>();       
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (Keyboard.current == null || Mouse.current == null || mainCamera == null)
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


        if(player == null)
        {
            Debug.LogWarning("발사 실패: Player 컴포넌트를 찾지 못했습니다.");
            return;
        }

        if (player.WeaponController == null)
        {
            Debug.LogWarning("발사 실패: WeaponController가 없습니다.");
            return;
        }

        Debug.Log(
            $"발사 시도 - 무기: {player.WeaponController.GetEquippedWeapon()?.ItemName}, " +
            $"탄약: {player.WeaponController.CurrentAmmo} / {player.WeaponController.MaxAmmo}"
        );

        // Inspector 연결 확인
        if (firePoint == null || bulletPrefab == null)
        {
            Debug.LogWarning("Fire Point 또는 Bullet Prefab이 연결되지 않았습니다.");
            return;
        }

        // 총알이 없으면 발사하지 않음
        if (!player.WeaponController.TryConsumeAmmo())
        {
            Debug.Log("탄약이 부족합니다.");
            return;
        }

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mouseWorldPosition.z = 0f;

        Vector2 direction = ((Vector2)mouseWorldPosition - (Vector2)firePoint.position).normalized;

        // 공격 애니메이션
        if (weaponAnimator != null)
        {
            weaponAnimator.ResetTrigger("Shoot");
            weaponAnimator.SetTrigger("Shoot");
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -45f;

        // 총알 생성
        Bullet bullet = Instantiate(bulletPrefab,firePoint.position,Quaternion.Euler(0f,0f,angle));

        Debug.Log("총알 생성됨: " + bullet.name);

        bullet.SetDamage(player.WeaponController.GetAttackDamage());
        bullet.Launch(direction);

        fireTimer = 0f;
    }

    private IEnumerator Reload()
    {
        // 이미 재장전 중이라면 중복 실행하지 않음
        if (isReloading)
        {
            Debug.Log("장전 실패: 이미 장전 중입니다.");
            yield break;
        }

        // 탄창이 이미 가득 차 있다면 재장전하지 않음
        if (player == null || player.WeaponController == null)
        {
            Debug.LogWarning("장전 실패: Player 컴포넌트를 찾지 못했습니다.");
            yield break;
        }

        if (!player.WeaponController.CanReload(player.Inventory))
        {
            Debug.LogWarning("장전 실패: WeaponController가 없습니다.");
            yield break;
        }

        Debug.Log(
            $"장전 시도 - 무기: {player.WeaponController.GetEquippedWeapon()?.ItemName}, " +
            $"탄약: {player.WeaponController.CurrentAmmo} / {player.WeaponController.MaxAmmo}"
        );

        if (!player.WeaponController.CanReload(player.Inventory))
        {
            Debug.Log("장전 실패: 장전 조건을 만족하지 않습니다.");
            yield break;
        }

        isReloading = true;

        Debug.Log("재장전 시작");

        if (characterAnimator != null)
        {
            characterAnimator.ResetTrigger("Reload");
            characterAnimator.SetTrigger("Reload");
        }

        yield return new WaitForSeconds(player.WeaponController.ReloadTime);

        player.WeaponController.ReloadFromInventory(player.Inventory);

        Debug.Log(
            $"재장전 완료: {player.WeaponController.CurrentAmmo} / {player.WeaponController.MaxAmmo}"
        );

        isReloading = false;
    }
}
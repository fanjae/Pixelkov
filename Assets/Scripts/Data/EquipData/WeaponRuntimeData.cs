using System;
using UnityEngine;

[Serializable]
public class WeaponRuntimeData
{
    // 장전 화살 수
    [SerializeField] private int currentAmmo;

    public int CurrentAmmo => currentAmmo;

    // 초기 장전 수 설정
    public WeaponRuntimeData(int initialAmmo = 0)
    {
        currentAmmo = Mathf.Max(0, initialAmmo);
    }

    // 발사 가능 체크
    public bool CanShoot()
    {
        return currentAmmo > 0;
    }

    // 지정한 수 만큼 탄약 소모
    public void ConsumeAmmo(int amount = 1)
    {
        if (amount <= 0) return;
        currentAmmo = Mathf.Max(0, currentAmmo - amount);
    }

    // 탄창을 가득 채우기 위해 필요한 탄약 수 계산
    public int GetNeededAmmoCount(int capacity)
    {
        return Mathf.Max(0, capacity - currentAmmo);
    }

    // 장전 처리
    public void Reload(int amount, int capacity)
    {
        if (amount <= 0) return;

        // 탄창 용량 초과금지
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, capacity);
    }
}
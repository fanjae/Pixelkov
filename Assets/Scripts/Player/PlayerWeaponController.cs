using UnityEngine;

public class PlayerWeaponController
{
    private readonly Inventory inventory;

    // 장착 정보 확인용
    private readonly Equipment equipment;

    // 장비 슬롯에 저장된 ItemID의 WeaponDat 조회 DB
    private readonly ItemDatabase itemDatabase;

    // 무기 장착시 기본 공격력
    private readonly int defaultDamage;

    public int CurrentAmmo
    {
        get
        {
            WeaponRuntimeData runtimeData = GetCurrentWeaponRuntime();
            return runtimeData == null ? 0 : runtimeData.CurrentAmmo;
        }
    }

    // 현재 장착 무기의 탄창 용량
    // 무기 없으면 무한 탄환(0) 반환
    public int MaxAmmo
    {
        get
        {
            WeaponData weapon = GetEquippedWeapon();
            return weapon == null ? 0 : weapon.Capacity;
        }
    }

    // 현재 장착 무기의 장전 시간 반환
    // 무기가 없으면 장전 시간이 없으므로(0) 반환

    public float ReloadTime
    {
        get
        {
            WeaponData weapon = GetEquippedWeapon();
            return weapon == null ? 0f : weapon.ReloadTime;
        }
    }

    public PlayerWeaponController(Inventory inventory, Equipment equipment, ItemDatabase itemDatabase, int defaultDamage)
    {
        this.inventory = inventory;
        this.equipment = equipment;
        this.itemDatabase = itemDatabase;
        this.defaultDamage = defaultDamage;
    }

    // 현재 Weapon 슬롯에 장착된 아이템을 WeaponData로 조회
    public WeaponData GetEquippedWeapon()
    {
        if (equipment == null || itemDatabase == null) return null;

        // 장비 슬롯 중 무기 슬롯 조회
        if (!equipment.TryGetSlot(EquipmentSlotType.Weapon, out EquipmentSlot slot))
            return null;

        // 무기 슬롯 비어 있으면 장착 무기 없음으로 처리
        if (slot.IsEmpty)
            return null;

        // 슬롯에 저장된 ItemId를 조회하여 WeaponData로 캐스팅.
        return itemDatabase.GetItem(slot.ItemId) as WeaponData;
    }

    // 현재 공격력 반환
    // 무기가 없으면 기본 공격력, 있으면 무기 공격력 사용.
    public int GetAttackDamage()
    {
        WeaponData weapon = GetEquippedWeapon();

        if (weapon == null)
            return defaultDamage;

        return weapon.Damage;
    }

    // 발사 가능 여부 체크

    public bool CanShoot()
    {
        WeaponData weapon = GetEquippedWeapon();

        // 무기가 없으면 기본 공격은 탄약 없이 가능하도록 처리
        if (weapon == null) return true;

        WeaponRuntimeData runtimeData = GetCurrentWeaponRuntime();

        // 무기가 있으면 현재 탄약 수 기준으로 발사 가능 여부 판단.
        return runtimeData != null && runtimeData.CanShoot();
    }

    // 공격 시 탄약 소모
    public bool TryConsumeAmmo()
    {
        WeaponData weapon = GetEquippedWeapon();

        // 무기가 없으면 탄약 소모 없이 공격 성공
        if (weapon == null)
            return true;

        WeaponRuntimeData runtimeData = GetCurrentWeaponRuntime();

        if (runtimeData == null)
            return false;

        // 탄약이 없으면 공격 실패.
        if (!runtimeData.CanShoot())
            return false;

        // 탄약 1발 소모
        runtimeData.ConsumeAmmo();
        return true;
    }

    // 현재 장착 무기를 장전할 수 있는지 확인.
    public bool CanReload(Inventory inventory)
    {
        WeaponData weapon = GetEquippedWeapon();
        WeaponRuntimeData runtimeData = GetCurrentWeaponRuntime();

        if (weapon == null) return false;
        if (runtimeData == null) return false;
        if (inventory == null) return false;

        // 탄창을 가득 채우기 위해 필요한 탄약 수 계산
        int needCount = runtimeData.GetNeededAmmoCount(weapon.Capacity);

        // 이미 탄창 가득 찼으면 장전 불가능
        if (needCount <= 0) return false;

        // 인벤토리에 해당 무기가 사용하는 탄약 아이템이 1개 이상 있는지 확인
        return inventory.HasItem(weapon.AmmoItemId, 1);
    }

    // 인벤토리의 탄약 아이템을 소모해서 장전
    public bool ReloadFromInventory(Inventory inventory)
    {
        WeaponData weapon = GetEquippedWeapon();
        WeaponRuntimeData runtimeData = GetCurrentWeaponRuntime();

        if (weapon == null) return false;
        if (runtimeData == null) return false;
        if (inventory == null) return false;

        // 현재 탄창에 필요한 탄약 수 계산
        int needCount = runtimeData.GetNeededAmmoCount(weapon.Capacity);

        if (needCount <= 0) return false;

        int reloadCount = 0;

        // 필요한 수량 만큼, 또는 인벤토리 탄약이 떨어질 때 까지 1개씩 제거
        while (reloadCount < needCount && inventory.RemoveItem(weapon.AmmoItemId, 1))
        {
            reloadCount++;
        }

        // 실제로 소모한 탄약이 없으면 장전 실패
        if (reloadCount <= 0) return false;

        // 소모한 탄약 수만큼 현재 탄창에 반영
        runtimeData.Reload(reloadCount, weapon.Capacity);
        return true;
    }

    private WeaponRuntimeData GetCurrentWeaponRuntime()
    {
        if (equipment == null || inventory == null)
        {
            
            Debug.LogWarning("Runtime 조회 실패: equipment 또는 inventory null");
            return null;
        }

        if (!equipment.TryGetSlot(EquipmentSlotType.Weapon, out EquipmentSlot equipmentSlot))
        {
            Debug.LogWarning("Runtime 조회 실패: Weapon 장비 슬롯 없음");
            return null;
        }

        if (equipmentSlot.IsEmpty)
        {
            Debug.LogWarning("Runtime 조회 실패: Weapon 장비 슬롯 비어있음");
            return null;
        }

        if (!inventory.TryGetSlot(equipmentSlot.InventorySlotIndex, out InventorySlot inventorySlot))
        {
            Debug.LogWarning($"Runtime 조회 실패: 인벤토리 슬롯 인덱스 오류 {equipmentSlot.InventorySlotIndex}");
            return null;
        }

        if (inventorySlot.IsEmpty)
        {
            Debug.LogWarning($"Runtime 조회 실패: 인벤토리 슬롯 비어있음 {equipmentSlot.InventorySlotIndex}");
            return null;
        }

        if (inventorySlot.ItemInstance == null)
        {
            Debug.LogWarning("Runtime 조회 실패: ItemInstance null");
            return null;
        }

        WeaponRuntimeData runtime = inventorySlot.ItemInstance.RuntimeData as WeaponRuntimeData;

        if (runtime == null)
        {
            Debug.LogWarning($"Runtime 조회 실패: WeaponRuntimeData 아님 / ItemId: {inventorySlot.ItemId}");
            return null;
        }

        return runtime;
    }
}
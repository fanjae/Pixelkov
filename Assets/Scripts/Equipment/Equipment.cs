using System;
using System.Collections.Generic;

public class Equipment
{
    // 장비 슬롯을 슬롯 타입 기준 보관
    private readonly Dictionary<EquipmentSlotType, EquipmentSlot> slots;

    // 외부에서 슬롯 목록을 읽기만 가능하게 공개
    public IReadOnlyDictionary<EquipmentSlotType, EquipmentSlot> Slots => slots;

    // 장비 교체에 대한 UI 갱신
    public event Action OnEquipmentChanged;

    // 장비 교체 막는 용도
    private bool isWeaponLocked;

    public Equipment()
    {
        // 장비 시스템 생성 시 사용할 슬롯 미리 생성
        slots = new Dictionary<EquipmentSlotType, EquipmentSlot>
        {
            { EquipmentSlotType.Weapon, new EquipmentSlot(EquipmentSlotType.Weapon) },
            { EquipmentSlotType.Armor, new EquipmentSlot(EquipmentSlotType.Armor) },
            { EquipmentSlotType.Accessory, new EquipmentSlot(EquipmentSlotType.Accessory) }
        };
    }

    public bool EquipItem(ItemData itemData, int inventorySlotIndex)
    {

        // 장착할 아이템 데이터 체크
        if (itemData == null) return false;

        // ItemType을 실제 장비 슬롯 타입 변환 및 무기/방어구/악세서리 여부 체크
        if (!TryGetEquipmentSlotType(itemData.ItemType, out EquipmentSlotType slotType)) return false;

        // 장전 중 무기 장착만 차단
        if (isWeaponLocked && slotType == EquipmentSlotType.Weapon) return false;

        // 실제 슬롯 데이터 변경
        EquipmentSlot slot = slots[slotType];
        slot.SetItem(itemData.ItemId, inventorySlotIndex);

        // 장비 변경 상태 알림
        OnEquipmentChanged?.Invoke();
        return true;
    }

    // 지정한 장비 슬롯 아이템 해제
    public bool UnEquipItem(EquipmentSlotType slotType)
    {
        // 장전 중 무기 해제만 차단
        if (isWeaponLocked && slotType == EquipmentSlotType.Weapon) return false;

        // 미존재하는 슬롯 타입 해제 불가
        if (!slots.TryGetValue(slotType, out EquipmentSlot slot)) return false;

        // 이미 비어있는 슬롯 해제 불가
        if (slot.IsEmpty) return false;

        // 장착 정보 제거
        slot.Clear();

        // 장비 상태 변경 알림
        OnEquipmentChanged?.Invoke();
        return true;
    }

    public bool TryGetSlot(EquipmentSlotType slotType, out EquipmentSlot slot)
    {
        // 외부에서 특정 장비 슬롯 조회
        return slots.TryGetValue(slotType, out slot);
    }

    private bool TryGetEquipmentSlotType(ItemType itemType, out EquipmentSlotType slotType)
    {
        // ItemType을 EquipmentSlotType으로 매핑. 장착 가능한 아이템 체크.
        switch (itemType)
        {
            case ItemType.Weapon:
                slotType = EquipmentSlotType.Weapon;
                return true;

            case ItemType.Armor:
                slotType = EquipmentSlotType.Armor;
                return true;

            case ItemType.Accessory:
                slotType = EquipmentSlotType.Accessory;
                return true;

            default:
                slotType = default;
                return false;
        }
    }


    // 해당 아이템이 장착 가능한 장비 아이템인지 확인(외부 확인용)
    public bool CanEquip(ItemData itemData)
    {
        if (itemData == null) return false;

        return TryGetEquipmentSlotType(itemData.ItemType, out _);
    }


    // 인벤토리 슬롯의 아이템이 다른 ItemID로 교체된 경우,
    // 해당 슬롯을 참조 중인 장비 슬롯의 ItemID도 함께 교체.
    public bool ReplaceEquippedItem(int inventorySlotIndex, int oldItemId, int newItemId)
    {
        foreach (var pair in slots)
        {
            EquipmentSlot slot = pair.Value;

            // 비어있는 장비 슬롯은 갱신 대상에서 제외
            if (slot.IsEmpty) continue;

            // 같은 인벤토리 슬롯을 참조, 기존 ItemId가 일치하는 장비 슬롯을 찾음
            if (slot.InventorySlotIndex == inventorySlotIndex && slot.ItemId == oldItemId)
            {
                // 장비 슬롯의 ItemID를 새 ItemID로 동기화 처리
                slot.ChangeItemId(newItemId);
                OnEquipmentChanged?.Invoke();
                return true;
            }
        }

        return false;
    }

    public void SetWeaponLocked(bool locked)
    {
        isWeaponLocked = locked;
    }
}

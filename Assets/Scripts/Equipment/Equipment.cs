using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    // 장비 슬롯을 슬롯 타입 기준 보관
    private readonly Dictionary<EquipmentSlotType, EquipmentSlot> slots;

    // 외부에서 슬롯 목록을 읽기만 가능하게 공개
    public IReadOnlyDictionary<EquipmentSlotType, EquipmentSlot> Slots => slots;

    // 장비 교체에 대한 UI 갱신
    public event Action OnEquipmentChanged;

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
    
    public bool EquipItem(ItemData itemData, out int previousItemId)
    {
        // 교체된 기존 장비 없으면 0.
        previousItemId = 0;

        // 장착할 아이템 데이터 체크
        if (itemData == null) return false;

        // ItemType을 실제 장비 슬롯 타입 변환 및 무기/방어구/악세서리 여부 체크
        if (!TryGetEquipmentSlotType(itemData.ItemType, out EquipmentSlotType slotType)) return false;

        EquipmentSlot slot = slots[slotType];

        // 해당 슬롯에 이미 장비 존재하면 기존 장비 ItemID를 변환해서 인벤토리로 돌려보냄.
        if (!slot.IsEmpty) previousItemId = slot.ItemId;

        // 실제 슬롯 데이터 변경
        slot.SetItem(itemData.ItemId);

        // 장비 변경 상태 알림
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
}

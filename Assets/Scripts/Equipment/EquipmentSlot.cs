using JetBrains.Annotations;
using UnityEngine;

public class EquipmentSlot
{
    // Weapon, Armor, Accessory
    public EquipmentSlotType SlotType { get; }
    public int ItemId { get; private set; }
    public int InventorySlotIndex { get; private set; } // 0618 추가 (특정 슬롯 아이템이 장착 중 상태 표기 용)

    // 슬롯 비어있는지 체크 (장비는 ID값으로 판단)
    public bool IsEmpty => ItemId <= 0;

    public EquipmentSlot(EquipmentSlotType slotType)
    {
        // 슬롯 생성 시 장비 슬롯 고정
        SlotType = slotType;
        Clear();
    }

    public void ChangeItemId(int itemId) // 강화 추가에 따른 장비 슬롯 Item 교체 방식 추가
    {
        if (itemId <= 0) return;

        ItemId = itemId;
    }

    public void SetItem(int itemId, int inventorySlotIndex)
    {
        // 슬롯에 장착된 아이템 ID 저장
        ItemId = itemId;
        InventorySlotIndex = inventorySlotIndex;
    }
    
    public void Clear()
    {
        // 해제 상태
        ItemId = 0;
        InventorySlotIndex = -1;
    }
}

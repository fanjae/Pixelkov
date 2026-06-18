using JetBrains.Annotations;
using UnityEngine;

public class EquipmentSlot
{
    // Weapon, Armor, Accessory
    public EquipmentSlotType SlotType { get; }
    public int ItemId { get; private set; }

    // 슬롯 비어있는지 체크 (장비는 ID값으로 판단)
    public bool IsEmpty => ItemId <= 0;

    public EquipmentSlot(EquipmentSlotType slotType)
    {
        // 슬롯 생성 시 장비 슬롯 고정
        SlotType = slotType;
        Clear();
    }
    public void SetItem(int itemId)
    {
        // 슬롯에 장착된 아이템 ID 저장
        ItemId = itemId;
    }
    
    public void Clear()
    {
        // 해제 상태
        ItemId = 0;
    }
}

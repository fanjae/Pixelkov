using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventoryController
{
    private readonly Inventory inventory;
    private readonly Equipment equipment;
    private readonly ItemDatabase itemDatabase;

    public PlayerInventoryController(Inventory inventory, Equipment equipment, ItemDatabase itemDatabase)
    {
        // 인벤토리, 장비, 아이템 DB 주입 받아 사용
        this.inventory = inventory;
        this.equipment = equipment;
        this.itemDatabase = itemDatabase;
    }

    // 인벤토리 슬롯의 장비 아이템 장착
    public bool EquipFromInventory(int slotIndex)
    {
        // 유효하지 않은 슬롯 장착 불가
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot)) return false;

        // 빈 슬롯 장착 불가
        if (inventorySlot.IsEmpty) return false;

        if (IsEquippedSlot(slotIndex)) return false;

        // 슬롯 ItemId로 실제 ItemData 조회
        ItemData itemData = itemDatabase.GetItem(inventorySlot.ItemId);
        if (itemData == null) return false;

        // 장착 요청
        return equipment.EquipItem(itemData, slotIndex);
    }

    public bool UnEquip(EquipmentSlotType slotType)
    {
        return equipment.UnEquipItem(slotType);
    }

    // 장착 여부 체크
    public bool IsEquippedSlot(int slotIndex)
    {
        // 슬롯이 없거나 비어있으면 장착 상태 아님
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot)) return false;
        if (inventorySlot.IsEmpty) return false;

        foreach (var pair in equipment.Slots)
        {
            EquipmentSlot equipmentSlot = pair.Value;

            // 슬롯 인덱스 및 ItemID가 모두 일치시 실제 장착 중인 아이템으로 판단
            if (!equipmentSlot.IsEmpty && equipmentSlot.InventorySlotIndex == slotIndex && equipmentSlot.ItemId == inventorySlot.ItemId) return true;
        }
        return false;
    }

    // 인벤토리 특정 슬롯 아이템의 장착 가능 여부 체크
    public bool CanEquipFromInventory(int slotIndex)
    {
        // 유효하지 않은 슬롯이나 빈 슬롯 장착 불가
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot)) return false;
        if (inventorySlot.IsEmpty) return false;

        // ItemId로 ItemData를 조회한 뒤 장착 가능 여부 확인
        ItemData itemData = itemDatabase.GetItem(inventorySlot.ItemId);
        if (itemData == null) return false;

        return equipment.CanEquip(itemData);
    }

    // 인벤토리 특정 슬롯 아이템 제거 가능 여부 확인
    public bool CanRemoveFromInventory(int slotIndex, int count = 1)
    {
        if (count <= 0) return false;

        // 잘못된 슬롯 제거 불가
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot inventorySlot)) return false;

        // 빈 슬롯 제거 불가
        if (inventorySlot.IsEmpty) return false;

        // 장착 중인 아이템 제거 불가
        if (IsEquippedSlot(slotIndex)) return false;

        // 슬롯에 제거하려는 수량 이상 보유 여부 체크
        return inventorySlot.Count >= count;
    }
}

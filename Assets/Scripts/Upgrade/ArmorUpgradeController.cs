using UnityEngine;

public class ArmorUpgradeController
{
    private readonly Inventory inventory;
    private readonly ItemDatabase itemDatabase;
    private readonly PlayerGoldController goldController;
    private readonly PlayerInventoryController inventoryController;
    private readonly Equipment equipment;

    public ArmorUpgradeController(Inventory inventory, ItemDatabase itemDatabase, PlayerGoldController goldController, PlayerInventoryController inventoryController, Equipment equipment)
    {
        this.inventory = inventory;
        this.itemDatabase = itemDatabase;
        this.goldController = goldController;
        this.inventoryController = inventoryController;
        this.equipment = equipment;
    }

    // 지정한 인벤토리 슬롯의 방어구 강화 가능한 상태인지 확인
    public bool CanUpgradeArmotAt(int slotIndex)
    {
        // 잘못된 슬롯이거나 빈 슬롯이면 강화 불가
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot slot)) return false;
        if (slot.IsEmpty) return false;

        // 슬롯의 ItemId로 실제 아이템 데이터를 조회하고, 방어구 인지 체크
        ItemData itemData = itemDatabase.GetItem(slot.ItemId);
        if (itemData is not ArmorData armorData) return false;

        // 다음 강화 단계가 없으면 강화 불가
        if (!armorData.CanUpgrade) return false;

        // nextUpgradeItemId가 실제 방어구 아이템인지 가리킴
        ItemData nextItemData = itemDatabase.GetItem(armorData.NextUpgradeItemId);
        if (nextItemData is not ArmorData) return false;

        // 강화 비용을 지불 가능한지 확인
        return goldController.CanSpendGold(armorData.UpgradeCost);
    }

    // 지정한 인벤토리 슬롯의 방어구를 다음 강화 단계로 교체
    public bool UpgradeArmorAt(int slotIndex)
    {
        // 강화 가능 조건을 검증
        if (!CanUpgradeArmotAt(slotIndex)) return false;

        InventorySlot slot = inventory.Slots[slotIndex];

        ArmorData currentArmor = itemDatabase.GetItem(slot.ItemId) as ArmorData;
        ArmorData nextArmor = itemDatabase.GetItem(currentArmor.NextUpgradeItemId) as ArmorData;

        bool wasEquipped = inventoryController.IsEquippedSlot(slotIndex);
        int oldItemId = currentArmor.ItemId;
        int newItemId = nextArmor.ItemId;

        // 강화 비용 지불
        if (!goldController.SpendGold(currentArmor.UpgradeCost)) return false;

        // 기존 방어구 제거
        if (!inventory.RemoveItemAt(slotIndex, 1))
        {
            goldController.AddGold(currentArmor.UpgradeCost);
            return false;
        }

        // 강화된 방어구 지급
        if (!inventory.AddItem(nextArmor, 1))
        {
            // 실패 시 기존 방어구, 골드 복구
            inventory.AddItem(currentArmor, 1);
            goldController.AddGold(currentArmor.UpgradeCost);
            return false;
        }

        if (wasEquipped)
        {
            if (!equipment.ReplaceEquippedItem(slotIndex, oldItemId, newItemId))
            {
                return false;
            }
        }

        return true;
    }
}

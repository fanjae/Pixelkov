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
    public bool CanUpgradeArmorAt(int slotIndex)
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
        if (!CanUpgradeArmorAt(slotIndex)) return false;

        InventorySlot slot = inventory.Slots[slotIndex];

        ArmorData currentArmor = itemDatabase.GetItem(slot.ItemId) as ArmorData;
        ArmorData nextArmor = itemDatabase.GetItem(currentArmor.NextUpgradeItemId) as ArmorData;

        bool wasEquipped = inventoryController.IsEquippedSlot(slotIndex);
        int oldItemId = currentArmor.ItemId;
        int newItemId = nextArmor.ItemId;

        // 강화 비용 지불
        if (!goldController.SpendGold(currentArmor.UpgradeCost)) return false;

        // 기존 슬롯의 아이템을 강화된 방어구로 교체
        if (!inventory.ReplaceItemAt(slotIndex, nextArmor))
        {
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

    // 인벤토리 슬롯에 있는 강화 가능한 방어구 데이터 조회
    public ArmorData GetUpgradeableArmorAt(int slotIndex)
    {
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot slot)) return null;
        if (slot.IsEmpty) return null;

        return itemDatabase.GetItem(slot.ItemId) as ArmorData;
    }

    // 현재 방어구의 다음 강화 단계 방어구 데이터 조회
    public ArmorData GetNextArmorAt(int slotIndex)
    {
        ArmorData currentArmor = GetUpgradeableArmorAt(slotIndex);
        if (currentArmor == null) return null;
        if (!currentArmor.CanUpgrade) return null;

        return itemDatabase.GetItem(currentArmor.NextUpgradeItemId) as ArmorData;
    }

}

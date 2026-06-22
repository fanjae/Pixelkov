using UnityEngine;

public class ShopController
{
    private readonly Inventory inventory;
    private readonly ItemDatabase itemDatabase;
    private readonly PlayerGoldController goldController; // 있음을 전제(팀원 구현 예정)
    private readonly PlayerInventoryController inventoryController;
    private readonly ShopData shopData; 
    
    public ShopController(Inventory inventory, ItemDatabase itemDatabase, PlayerGoldController goldController, PlayerInventoryController inventoryController, ShopData shopData)
    {
        this.inventory = inventory;
        this.itemDatabase = itemDatabase;
        this.goldController = goldController;
        this.inventoryController = inventoryController;
        this.shopData = shopData;
    }

    public bool BuyItem(int itemId, int count = 1)
    {
        // 잘못된 ItemId나 구매 횟수 방지
        if (itemId <= 0 || count <= 0) return false;

        // 현재 상점에 판매하는 아이템 체크
        ShopItemData shopItemData = GetShopItemData(itemId);
        if (shopItemData == null) return false;

        // ItemId로 실제 아이템 데이터 조회
        ItemData itemData = itemDatabase.GetItem(itemId);
        if (itemData == null) return false;

        // 총 구매 가격 계산
        int totalPrice = itemData.Price * count;

        // if (!goldController.CanSpendGold(totalPrice)) return false;

        if (!inventory.CanAddItem(itemData, count)) return false;

        // if (!goldController.SpendGold(totalPrice)) return false;

        if (!inventory.AddItem(itemData, count))
        {
            // goldController.AddGold(totalPrice);
            return false;
        }

        return true;
    }

    // 인벤토리 슬롯의 아이템 판매
    public bool SellItemAt(int slotIndex, int count = 1)
    {
        if (count <= 0) return false;

        // 빈 슬롯, 잘못된 슬롯 등을 차단
        if (!inventoryController.CanRemoveFromInventory(slotIndex, count)) return false;

        // 슬롯 정보 조회
        if (!inventory.TryGetSlot(slotIndex, out InventorySlot slot)) return false;

        // ItemId로 데이터 조회
        ItemData itemData = itemDatabase.GetItem(slot.ItemId);
        if (itemData == null) return false;

        // 판매 가격 계산
        int sellPrice = GetSellPrice(itemData, count);

        // 인벤토리에서 해당 아이템 제거
        if (!inventory.RemoveItemAt(slotIndex, count)) return false;

        // goldController.AddGold(sellPrice);
        return true;
    }



    // 현재 상점에서 특정 ItemId 상품을 판매 중인지 체크
    private ShopItemData GetShopItemData(int itemId)
    {
        if (shopData == null || shopData.ShopItems == null) return null;

        // 현재 상점에서 특정 Item 상품을 판매 중인지 비교하여 맞으면 shopItemData 반환.
        foreach (ShopItemData shopItemData in shopData.ShopItems)
        {
            if (shopItemData == null) continue;

            if (shopItemData.ItemId == itemId) return shopItemData;
        }
        return null;
    }

    // 판매 금액 처리(원가의 50% 느낌으로)
    private int GetSellPrice(ItemData itemData, int count)
    {
        return (itemData.Price / 2) * count;
    }
    
}

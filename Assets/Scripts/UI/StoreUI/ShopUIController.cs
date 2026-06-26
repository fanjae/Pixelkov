using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShopUIController : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public static ItemDatabase Database { get; private set; }

    [Header("외부 참조 컴포넌트")]
    [SerializeField] private ItemDatabase database;
    [SerializeField] private ShopData shopData;
    [SerializeField] private Player player;
    [SerializeField] private PlayerGoldController goldController;

    [Header("캔버스 내 참조 컴포넌트")]
    [SerializeField] private BuyPanel buyPanel;
    [SerializeField] private ArmorUpgradePanel upgradePanel;
    [SerializeField] private InventoryUIController invenUIController;

    private Inventory inventory;
    private Equipment equipment;

    private ShopController shopController;
    private PlayerInventoryController inventoryController;
    private ArmorUpgradeController upgradeController;

    private HashSet<int> Duplicate = new HashSet<int>();
    private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        Database = database;
        InitShopList();
        if (player == null) FindAnyObjectByType<Player>();
        if (invenUIController == null) FindAnyObjectByType<InventoryUIController>();

        if(player != null)
        {
            // 플레이어의 인벤토리, 장비 받을 예정
            inventory = player.Inventory;
            equipment = player.Equipment;
        }
        if (inventory != null && equipment != null && database != null)
        {
            inventoryController = new PlayerInventoryController(inventory, equipment, database);
        }
        if (goldController != null && inventoryController != null)
        {
            upgradeController = new ArmorUpgradeController(inventory, database, goldController, inventoryController, equipment);
            if(shopData != null)
                shopController = new ShopController(inventory, database, goldController, inventoryController, shopData);
        }
    }
    private void OnEnable()
    {
        // 활성화 시 인벤토리 슬롯의 판매 기능 활성화
        if (invenUIController != null && shopController != null)
        {
            invenUIController.AllocateShop(shopController.SellItemAt);
        }
        if (upgradePanel != null)
        {
            upgradePanel.OnUpgrade += Upgrade;
        }
    }

    private void OnDisable()
    {
        // 인벤토리 슬롯 판매 기능 해제
        if (invenUIController != null && shopController != null)
        {
            invenUIController.ReleaseShop(shopController.SellItemAt);
        }
        if (upgradePanel != null)
        {
            upgradePanel.OnUpgrade -= Upgrade;
        }
    }

    private void OnDestroy()
    {
        AllocateBuy();
    }

    private void InitShopList()
    {
        // ShopData에 들어있는 리스트를 통해 상점에 아이템 추가
        foreach (var data in shopData.ShopItems)
        {
            // 중복 아이템 검사
            if (Duplicate.Contains(data.ItemId)) continue;

            buyPanel.AddProduct(data.ItemId, Buy, goldController);
            Duplicate.Add(data.ItemId);
        }
    }
    private void AllocateBuy()
    {
        buyPanel.AllocEvent(Buy);
    }
    private bool Buy(int itemId, int count)
    {
        return shopController.BuyItem(itemId, count);
    }
    public bool Upgrade(int slotIndex)
    {
        return upgradeController.UpgradeArmorAt(slotIndex);
    }
    // 창 움직이는 기능 관련 메서드
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = (Vector2)transform.position - eventData.position;
    }
}

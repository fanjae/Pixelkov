using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShopUIController : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public static ItemDatabase Database { get; private set; }

    [SerializeField] private ItemDatabase database;
    [SerializeField] private PlayerGoldController goldController; // 플레이어 받으면 수정 가능성 존재
    [SerializeField] private ShopData shopData;
    [SerializeField] private BuyPanel buyPanel;

    [SerializeField] private InventoryUIController invenUIController;

    private Inventory inventory;
    private Equipment equipment;
    private PlayerInventoryController inventoryController;
    private ShopController shopController;
    private HashSet<int> Duplicate = new HashSet<int>();
    private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        Database = database;
        InitShopList();
    }

    private void OnEnable()
    {
        if(shopController == null)
        {
            inventory = invenUIController.Inventory;
            equipment = invenUIController.Equipment;
            inventoryController = invenUIController.InventoryController;
            shopController = new ShopController(inventory, database, goldController, inventoryController, shopData);
        }
        // 활성화 시 인벤토리 슬롯의 판매 기능 활성화
        invenUIController?.AllocateShop(shopController);
    }

    private void OnDisable()
    {
        // 인벤토리 슬롯 판매 기능 해제
        invenUIController?.ReleaseShop();
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
            if (Duplicate.Contains(data.ItemId)) return;

            buyPanel.AddProduct(data.ItemId, Buy);
            Duplicate.Add(data.ItemId);
        }
    }
    private void AllocateBuy()
    {
        buyPanel.AllocEvent(Buy);
    }
    private void Buy(int itemId)
    {
        shopController.BuyItem(itemId);
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

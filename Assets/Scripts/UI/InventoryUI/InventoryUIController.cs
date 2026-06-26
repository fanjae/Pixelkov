using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 인벤토리의 주요 정보를 담고, 패널들의 흐름을 관리합니다.
/// </summary>
public class InventoryUIController : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public static ItemDatabase Database { get; private set; }
    [SerializeField] private ItemDatabase database;

    [SerializeField] private GuidePanel guidePanel;
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private EquipmentPanel equipmentPanel;

    [SerializeField] private Player player;

    private Inventory inventory;
    private Equipment equipment;

    private PlayerInventoryController inventoryController;

    // 마우스 드래그 오프셋
    private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        inventoryPanel?.AllocateSlotEvent(OpenGuidePanel, CloseGuidePanel, Equip); // GuidePanel의 온/오프 메서드 할당
        equipmentPanel?.AllocateSlotEvent(UnEquip);

        if (database != null)
        {
            Database = database;
        }
        if(player == null) player = FindAnyObjectByType<Player>();
    }
    private void Start()
    {
        if(player != null)
        {
            // 플레이어의 인벤토리, 장비 받을 예정
            inventory = player.Inventory;
            equipment = player.Equipment;
        }
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventory;
            inventoryPanel?.AllocateInventory(inventory);
        }
        if (equipment != null)
        {
            equipment.OnEquipmentChanged += UpdateEquipment;
            equipmentPanel?.AllocateEquipment(equipment);
        }
        if (inventory != null && equipment != null && database != null)
        {
            inventoryController = new PlayerInventoryController(inventory, equipment, database);
        }
    }
    private void OnDestroy()
    {
        inventoryPanel?.ReleaseSlotEvent(OpenGuidePanel, CloseGuidePanel, Equip); // GuidePanel의 온/오프 메서드 할당
        equipmentPanel?.ReleaseSlotEvent(UnEquip);

        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateInventory;
        }
        if(equipment != null)
        {
            equipment.OnEquipmentChanged -= UpdateEquipment;
        }
    }

    private void OnEnable()
    {
        UpdateInventory();
        UpdateEquipment();
    }
    
    /// <summary>
    /// GuidePanel을 활성화 하는 메서드. itemId를 기반으로 내용을 재구성 합니다.
    /// </summary>
    /// <param name="itemId"></param>
    private void OpenGuidePanel(int itemId)
    {
        bool? result = guidePanel?.PaintGuide(itemId);
        if(result != null && result == true)
            guidePanel?.gameObject.SetActive(true);
    }
    /// <summary>
    /// GuidePanel을 비활성화 하는 메서드.
    /// </summary>
    private void CloseGuidePanel()
    {
        guidePanel?.gameObject.SetActive(false);
    }
    /// <summary>
    /// InventoryPanel을 업데이트하는 메서드
    /// </summary>
    private void UpdateInventory()
    {
        inventoryPanel?.PaintInventoryAll();
    }
    /// <summary>
    /// EquipmentPanel을 업데이트하는 메서드
    /// </summary>
    private void UpdateEquipment()
    {
        equipmentPanel?.PaintEquipmentAll();
    }
    /// <summary>
    /// index 번째의 슬롯의 장비를 장착합니다.
    /// </summary>
    private void Equip(int index)
    {
        inventoryController?.EquipFromInventory(index);
    }
    /// <summary>
    /// 슬롯 타입을 기준으로 장비를 해제합니다.
    /// </summary>
    public void UnEquip(EquipmentSlotType slotType)
    {
        inventoryController?.UnEquip(slotType);
    }
    
    public void AllocateShop(Func<int, int, bool> sellAction)
    {
        inventoryPanel.AllocateSell(sellAction);
    }
    public void ReleaseShop(Func<int, int, bool> sellAction)
    {
        inventoryPanel.ReleaseSell(sellAction);
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

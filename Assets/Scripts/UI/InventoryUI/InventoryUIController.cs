using UnityEngine;

/// <summary>
/// 인벤토리의 주요 정보를 담고, 패널들의 흐름을 관리합니다.
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    public static ItemDatabase Database { get; private set; }

    [SerializeField] private GuidePanel guidePanel;
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private EquipmentPanel equipmentPanel;

    #region test Fields
    // 해당 region에 있는 필드들은 추후 플레이어 데이터와 연동해야하는 필드 입니다.
    [SerializeField] private ItemDatabase database; // 테스트용 데이터베이스
    private Inventory inventory = new Inventory(12);
    private Equipment equipment = new Equipment();
    #endregion
    private PlayerInventoryController inventoryController;

    private void Awake()
    {
        if (database != null)
        {
            Database = database;
            inventoryController = new PlayerInventoryController(inventory, equipment, database);
        }
        if(inventoryPanel != null)
        {
            inventoryPanel.AllocateSlotEvent(OpenGuidePanel, CloseGuidePanel, Equip); // GuidePanel의 온/오프 메서드 할당
            inventoryPanel.AllocateInventory(inventory);    // 임시로 생성된 인벤토리
        }
        if(equipmentPanel != null)
        {
            equipmentPanel.AllocateEquipment(equipment);
        }
        if(inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventory;
        }
        if(equipment != null)
        {
            equipment.OnEquipmentChanged += UpdateEquipment;
        }
        inventory.AddItem(database.GetItem(1));
        inventory.AddItem(database.GetItem(2));
    }
    private void OnDestroy()
    {
        if(inventoryPanel != null)
        {
            inventoryPanel.ReleaseSlotEvent(OpenGuidePanel, CloseGuidePanel, Equip); // GuidePanel의 온/오프 메서드 할당
        }
        if(inventory != null)
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
        if(guidePanel != null)
        {
            guidePanel.gameObject.SetActive(true);
            guidePanel.PaintGuide(itemId);
        }
    }
    /// <summary>
    /// GuidePanel을 비활성화 하는 메서드.
    /// </summary>
    private void CloseGuidePanel()
    {
        if(guidePanel != null)
            guidePanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// InventoryPanel을 업데이트하는 메서드
    /// </summary>
    private void UpdateInventory()
    {
        if(inventoryPanel != null)
            inventoryPanel.PaintInventoryAll();
    }
    /// <summary>
    /// EquipmentPanel을 업데이트하는 메서드
    /// </summary>
    private void UpdateEquipment()
    {
        if (equipmentPanel != null)
            equipmentPanel.PaintEquipmentAll();
    }
    /// <summary>
    /// index 번째의 슬롯의 장비를 장착합니다.
    /// </summary>
    private void Equip(int index)
    {
        inventoryController.EquipFromInventory(index);
    }
    public void UnEquip(EquipmentSlotType slotType)
    {
        inventoryController.UnEquip(slotType);
    }
}

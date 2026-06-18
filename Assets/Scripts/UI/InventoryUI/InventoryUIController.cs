using UnityEngine;

/// <summary>
/// 인벤토리의 주요 정보를 담고, 패널들의 흐름을 관리합니다.
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    public static ItemDatabase Database { get; private set; }

    [SerializeField] private GuidePanel guidePanel;
    [SerializeField] private InventoryPanel inventoryPanel;

    #region test Fields
    // 해당 region에 있는 필드들은 추후 플레이어 데이터와 연동해야하는 필드 입니다.
    [SerializeField] private ItemDatabase database; // 테스트용 데이터베이스
    private Inventory inventory = new Inventory(12);
    #endregion

    private void Awake()
    {
        if (database != null)
            Database = database;
        if(inventoryPanel != null)
        {
            inventoryPanel.AllocateSlotEvent(OpenGuidePanel, CloseGuidePanel); // GuidePanel의 온/오프 메서드 할당
            inventoryPanel.AllocateInventory(inventory);    // 임시로 생성된 인벤토리
        }
    }

    private void OnEnable()
    {
        InventoryPanelUpdate();
        // 다른 패널들도 기능 완성되면 추가 예정
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
    /// InventoryPanel을 업데이트하는 메서드. Action에 할당하는 용도로 사용할 예정입니다.
    /// </summary>
    private void InventoryPanelUpdate()
    {
        if(inventoryPanel != null)
            inventoryPanel.PaintInventoryAll();
    }
}

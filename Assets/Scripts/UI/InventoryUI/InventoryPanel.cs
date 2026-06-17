using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private ItemDatabase database; // 아이템 데이터 베이스
    [Header("아이템 슬롯")]
    [SerializeField] private SlotUI[] slotUIs;  // 인벤토리 각 슬롯에 아이템을 그리기 위한 배열
    private Inventory inventory = new Inventory(12);    // 임시 구현용으로 사용하는 인벤토리(추후 실제 사용하는 인벤토리로 변경 예정)

    private void OnEnable()
    {
        InventoryPaintAll();
    }

    /// <summary>
    /// 모든 슬롯을 다시 그리는 메서드
    /// </summary>
    private void InventoryPaintAll()
    {
        for(int i = 0; i < inventory.Slots.Count; i++)
        {
            InitSlotUI(i);
        }
    }
    /// <summary>
    /// 주어진 index번째 슬롯만 다시 그리는 메서드
    /// </summary>
    /// <param name="index">다시 그릴 슬롯의 인덱스</param>
    public void InventoryPaintIndex(int index)
    {
        InitSlotUI(index);
    }
    private void InitSlotUI(int index)
    {
        // 데이터 베이스가 할당되지 않은 경우
        if (database == null) return;
        // index가 UI상의 슬롯의 개수보다 크거나 인벤토리의 슬롯의 개수보다 큰 경우
        if (index >= slotUIs.Length || index >= inventory.Slots.Count) return;
        // 선택한 인벤토리의 슬롯이 할당되지 않은 경우
        if (inventory.Slots[index] == null) return;

        // index 번째의 슬롯 정보를 받아서 데이터를 얻기
        InventorySlot curSlot = inventory.Slots[index];
        int getId = curSlot.ItemId;
        ItemData itemInfo = database.GetItem(getId);
        if (itemInfo == null) return;

        // 얻은 데이터를 기반으로 슬롯 그리기
        Sprite icon = itemInfo.Icon;
        slotUIs[index].PaintSlot(icon, curSlot.Count);
    }
}

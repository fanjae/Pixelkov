using UnityEngine;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private Image iconImage;
    public EquipmentSlotType SlotType => slotType;
    public ItemData CurItem { get; private set; }
    
    /// <summary>
    /// 슬롯의 데이터를 가져와 UI에 적용하는 메서드
    /// </summary>
    public void SetSlotInfo(EquipmentSlot slot)
    {
        ItemDatabase database = InventoryUIController.Database;
        if (database == null) return;

        int getId = slot.ItemId;

        ItemData itemInfo = database.GetItem(getId);
        if (itemInfo == null) return;

        CurItem = itemInfo;
        iconImage.sprite = CurItem.Icon;
    }
}

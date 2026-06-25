using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private EquipmentSlotType slotType;
    [SerializeField] private Image iconImage;

    public event Action<EquipmentSlotType> OnUnEquip;
    public EquipmentSlotType SlotType => slotType;
    public ItemData CurItem { get; private set; }
    public int Index { get; private set; }

    private Vector3 originPos;
    private void Awake()
    {
        originPos = iconImage.transform.localPosition;
    }
    /// <summary>
    /// 슬롯의 데이터를 가져와 UI에 적용하는 메서드
    /// </summary>
    public void SetSlotInfo(EquipmentSlot slot)
    {
        ItemDatabase database = InventoryUIController.Database;
        if (database == null) return;

        int getId = slot.ItemId;

        Index = slot.InventorySlotIndex;

        ItemData itemInfo = database.GetItem(getId);
        // ItemData가 존재하지 않으면 UI 초기화
        if(itemInfo == null)
        {
            CurItem = null;
            iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            iconImage.sprite = null;
            return;
        }
        CurItem = itemInfo;
        iconImage.sprite = CurItem.Icon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right || eventData.clickCount >= 2)
        {
            OnUnEquip?.Invoke(slotType);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 슬롯에 아이템이 있을때만 아이콘 드래그
        if(CurItem == null) return;
        iconImage.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        iconImage.transform.localPosition = originPos;

        GameObject hoverObjects = eventData.pointerCurrentRaycast.gameObject;
        if (hoverObjects == null) return;

        ArmorUpgradePanel upgradePanel = hoverObjects.GetComponentInParent<ArmorUpgradePanel>();
        if(upgradePanel != null)
        {
            upgradePanel.PaintUpgradeUI(CurItem as ArmorData, Index);
            return;
        }
    }
}

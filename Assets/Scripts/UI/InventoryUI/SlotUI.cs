using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject subButton;

    public Action<int> OnSlotEnter;
    public Action OnSlotExit;
    public int Index { get; private set; }

    private Vector3 originScale;
    ItemData curItem = null;

    private void Awake()
    {
        originScale = transform.localScale;
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    /// <summary>
    /// 슬롯의 데이터를 가져와 UI에 적용하는 메서드
    /// </summary>
    public void SetSlotInfo(InventorySlot slot)
    {
        ItemDatabase database = InventoryUIController.Database;
        if (database == null) return;

        int getId = slot.ItemId;

        ItemData itemInfo = database.GetItem(getId);
        if (itemInfo == null) return;

        curItem = itemInfo;
        iconImage.sprite = curItem.Icon;
        if (slot.Count <= 1) countText.text = "";
        else countText.text = slot.Count.ToString();
    }

    // 슬롯에 마우스 올리면 확대
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale * 1.1f, 0.15f).SetEase(Ease.OutQuad);
        if(curItem != null)
        {
            OnSlotEnter?.Invoke(curItem.ItemId);
        }
    }
    // 슬롯에서 마우스를 내리면 복구
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale, 0.15f).SetEase(Ease.OutQuad);
        OnSlotExit?.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 시 서브 버튼 활성화/비활성화
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (subButton == null) return;
            subButton.SetActive(!subButton.activeSelf);
        }
    }
}

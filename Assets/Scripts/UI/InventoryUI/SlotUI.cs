using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, 
    IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject subButton;
    [SerializeField] private CounterSelector counterSelector;

    public event Action<int> OnSlotEnter;
    public event Action OnSlotExit;
    public event Action<int> OnEquip;
    public event Func<int, int, bool> OnSell;

    public int Index { get; private set; }
    public ItemData CurItem { get; private set; }

    private Vector3 originScale;
    private Vector3 originPos;
    private ArmorUpgradePanel upgradePanel;
    private int slotCount = 0;

    private void Awake()
    {
        originScale = transform.localScale;
        originPos = iconImage.transform.localPosition;
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    /// <summary>
    /// 버튼에 할당하기 위한 메서드
    /// </summary>
    public void OnClickEquipButton()
    {
        OnEquip?.Invoke(Index);
    }
    public void OnClickSellButton()
    {
        // 스택형 아이템이면 개수 정하는 UI 활성화
        if(CurItem != null && CurItem.IsStackable)
        {
            if(counterSelector != null && OnSell != null)
            {
                counterSelector.gameObject.SetActive(true);
            }
        }
        // 아니라면 한개만 있다는 의미이므로 한개 판매
        else
        {
            OnSell?.Invoke(Index, 1);
        }
    }
    /// <summary>
    /// Stackable Item 판매처리
    /// </summary>
    public void OnClickSellStackable()
    {
        OnSell?.Invoke(Index, counterSelector.Count);
    }
    /// <summary>
    /// subButton에 있는 Upgrade버튼에 할당하는 메서드로 업그레이드 패널에 데이터를 전달하는 기능
    /// </summary>
    public void OnClickUpgradeButton()
    {
        if(upgradePanel == null)
        {
            upgradePanel = FindAnyObjectByType<ArmorUpgradePanel>();
            if (upgradePanel == null) return;
        }
        upgradePanel.PaintUpgradeUI(CurItem as ArmorData, Index);
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
        // 데이터가 없으면 비워줍니다.
        if (itemInfo == null)
        {
            CurItem = null;
            iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            iconImage.sprite = null;
            countText.text = "";
            return;
        }
        // 데이터가 있다면 해당 데이터를 기반으로 그리기 진행
        CurItem = itemInfo;
        iconImage.sprite = CurItem.Icon;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        slotCount = slot.Count;
        if (slotCount <= 1) countText.text = "";
        else countText.text = slot.Count.ToString();

        counterSelector?.Init(0, slotCount);
    }
    // 슬롯에 마우스 올리면 슬롯 크기가 커집니다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale * 1.1f, 0.15f).SetEase(Ease.OutQuad);
        if(CurItem != null)
        {
            OnSlotEnter?.Invoke(CurItem.ItemId);
        }
    }
    // 슬롯에서 마우스를 내리면 원본 크기로 돌아옵니다.
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(originScale, 0.15f).SetEase(Ease.OutQuad);
        subButton.SetActive(false);
        OnSlotExit?.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 우클릭 시 서브 버튼 활성화/비활성화
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (subButton == null) return;

            if (CurItem == null) return;

            // 해당 슬롯이 재료 아이템이라면 상호작용 X
            if (CurItem.ItemType == ItemType.Material) return;

            subButton.SetActive(!subButton.activeSelf);
            return;
        }
        else
            subButton.SetActive(false);

        if (eventData.clickCount >= 2)
        {
            // 더블 클릭시 장착
            OnEquip?.Invoke(Index);
            eventData.clickCount = 0;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // 현재 슬롯에 아이템이 없을 시 드래그 하지 않음
        if (CurItem == null) return;

        iconImage.transform.position = eventData.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        subButton.SetActive(false);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        iconImage.transform.localPosition = originPos;
        // 드래그를 놓은 시점에서 마우스 위치로 Raycast시 맞은 대상의 GameObject를 가져오기
        GameObject hoverObjects = eventData.pointerCurrentRaycast.gameObject;
        if (hoverObjects == null) return;

        // 맞은 대상은 하위 UI들이고 원하는 슬롯 정보는 부모 오브젝트가 갖고 있으므로 InParent 사용
        EquipSlotUI equipInfo = hoverObjects.GetComponentInParent<EquipSlotUI>();
        if(equipInfo != null)
        {
            // 장착할 슬롯의 장비 타입과 현재 선택한 슬롯의 장비 타입이 일치하면 장착
            var slotType = ConvertToEquipType(CurItem.ItemType);
            if (equipInfo.SlotType == slotType)
                OnEquip?.Invoke(Index);
            return;
        }
        
        // 드래그가 끝난 시점(드롭)에 마우스가 올려진 UI의 이름으로 판매창에 드롭했음을 정합니다.
        if(hoverObjects.name == "SellCover")
        {
            // 스택형 아이템이면 CounterSelector 활성화
            if(CurItem.IsStackable)
            {
                // OnSell이 null이면 상점UI가 열리지 않았음을 의미하므로 예외처리
                if(counterSelector != null && OnSell != null)
                {
                    counterSelector.gameObject.SetActive(true);
                }
            }
            // 비스택형이면 바로 판매 진행
            else
            {
                OnSell?.Invoke(Index, 1);
            }
            return;
        }
        
        // 방어구 강화탭에 드롭한 경우 데이터 전달
        ArmorUpgradePanel upgradePanel = hoverObjects.GetComponentInParent<ArmorUpgradePanel>();
        if(upgradePanel != null)
        {
            upgradePanel.PaintUpgradeUI(CurItem as ArmorData, Index);
            return;
        }
    }
    private EquipmentSlotType ConvertToEquipType(ItemType type)
    {
        switch(type)
        {
            case ItemType.Weapon:
                return EquipmentSlotType.Weapon;
                case ItemType.Armor:
                return EquipmentSlotType.Armor;
            case ItemType.Accessory:
                return EquipmentSlotType.Accessory;
            default:
                return default;
        }
    }

}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private CounterSelector counterSelector;

    public int ItemId { get; private set; }
    public event Func<int, int, bool> OnBuyEvent;
    public ItemData curItem { get; private set; }

    private PlayerGoldController goldController;

    private void OnEnable()
    {
        // PlayerGoldController가 있다면 초기화 진행 + 컨트롤러에 
        if(goldController != null)
        {
            goldController.OnGoldChanged += UpdateCounterSelector;
            UpdateCounterSelector(goldController.Gold);
        }
    }
    private void OnDisable()
    {
        if(goldController != null)
        {
            goldController.OnGoldChanged -= UpdateCounterSelector;
        }
    }

    private void UpdateCounterSelector(int amount)
    {
        // 아이템이 비어있거나 스택형 아이템이 아닌 경우 업데이트 X
        if (curItem == null || !curItem.IsStackable) return;

        // DivideByZeroException 예외처리
        if(curItem.Price > 0)
        {
            int count = Mathf.Clamp(amount / curItem.Price, 0, curItem.MaxStackCount);
            counterSelector?.Init(0, count);
        }
        else
        {
            counterSelector?.Init(0, curItem.MaxStackCount);
        }
    }

    /// <summary>
    /// UI와 필드 초기화 메서드
    /// </summary>
    public void Init(int itemId, PlayerGoldController goldController)
    {
        curItem = ShopUIController.Database.GetItem(itemId);

        if (curItem == null || ItemId == itemId) return;
        ItemId = itemId;
        icon.sprite = curItem.Icon;
        itemName.text = curItem.ItemName;
        icon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        if(this.goldController == null && goldController != null)
        {
            this.goldController = goldController;
        }
    }
    /// <summary>
    /// 아이템 구매 버튼 할당 메서드
    /// </summary>
    public void OnClickButton()
    {
        if (curItem == null) return;
        
        // 스택형 아이템이고 구매 가능하면 CounterSelector 활성화
        if(curItem.IsStackable)
        {
            if(goldController.CanSpendGold(curItem.Price))
                counterSelector?.gameObject.SetActive(true);
        }
        // 비스택형 아이템의 경우 구매 이벤트
        else
        {
            OnBuyEvent?.Invoke(ItemId, 1);
        }
    }
    /// <summary>
    /// CounterSelector의 버튼에 할당하는 메서드로 스택형 아이템을 여러개씩 구매하는 기능
    /// </summary>
    public void OnClickStackable()
    {
        if (counterSelector == null) return;
        int count = counterSelector.Count;

        OnBuyEvent?.Invoke(ItemId, count);
    }
}

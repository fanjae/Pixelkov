using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProductUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    public int ItemId { get; private set; }
    public event Action<int> OnBuyAction;
    
    public void Init(int itemId)
    {
        ItemData data = ShopUIController.Database.GetItem(itemId);

        if (data == null || ItemId == itemId) return;

        ItemId = itemId;
        icon.sprite = data.Icon;
        itemName.text = data.ItemName;
    }
    public void OnClickButton()
    {
        OnBuyAction?.Invoke(ItemId);
    }
}

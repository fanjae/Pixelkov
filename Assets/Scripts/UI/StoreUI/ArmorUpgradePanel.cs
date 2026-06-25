using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUpgradePanel : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    public int SlotIndex { get; private set; } = -1;

    public Func<int, bool> OnUpgrade;

    private void OnEnable()
    {
        // 패널 활성화 시마다 UI 초기화
        InitUI();
    }
    public void UpgradeEvent()
    {
        bool? result = OnUpgrade?.Invoke(SlotIndex);
        // 강화 결과가 실패했다면 return
        if (result == null || result == false) return;
        // 성공이면 UI 초기화
        InitUI();
    }

    public void PaintUpgradeUI(ArmorData data, int slotIndex)
    {
        // 데이터가 없으면 초기화
        if(data == null)
        {
            InitUI();
            return;
        }
        // 같은 인덱스면 다시 그릴 필요X
        if (SlotIndex == slotIndex) return;

        iconImage.sprite = data.Icon;
        itemName.text = data.ItemName;
        description.text = data.Description;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        SlotIndex = slotIndex;
    }

    private void InitUI()
    {
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        iconImage.sprite = null;
        itemName.text = null;
        description.text = null;
        SlotIndex = -1;
    }
}

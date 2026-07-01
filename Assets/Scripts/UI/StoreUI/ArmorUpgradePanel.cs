using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUpgradePanel : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private UpgradeSlot upgradeSlot;

    public int SlotIndex { get; private set; } = -1;

    public Func<int, bool> OnUpgrade;

    private void OnEnable()
    {
        // 패널 활성화 시마다 UI 초기화
        InitUI();
        upgradeSlot.ReleaseEvent += InitUI;
    }
    private void OnDisable()
    {
        upgradeSlot.ReleaseEvent -= InitUI;
    }

    public void UpgradeEvent()
    {
        if(OnUpgrade != null)
        {
            bool isUpgrade = OnUpgrade.Invoke(SlotIndex);
            if (isUpgrade)
            {
                // 성공이면 UI 초기화
                InitUI();
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Play(SFXType.Upgrade);
                }
            }
        }
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
        itemPrice.text = data.UpgradeCost.ToString() + "G";
        description.text = data.Description;
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        SlotIndex = slotIndex;
    }

    private void InitUI()
    {
        iconImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        iconImage.sprite = null;
        itemName.text = null;
        itemPrice.text = "0G";
        description.text = null;
        SlotIndex = -1;
    }
}

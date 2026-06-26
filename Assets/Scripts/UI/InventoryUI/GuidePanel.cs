using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    public bool PaintGuide(int itemId)
    {
        // 할당되지 않은 필드가 존재하면 return
        if (icon == null) return false;
        if (itemName == null) return false;
        if (description == null) return false;

        ItemData data = InventoryUIController.Database.GetItem(itemId);

        if (data == null) return false;

        icon.sprite = data.Icon;
        itemName.text = data.ItemName;
        description.text = data.Description;
        icon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        return true;
    }
}

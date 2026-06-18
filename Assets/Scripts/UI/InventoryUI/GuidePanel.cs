using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    [SerializeField] private ItemDatabase database;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    public void PaintGuide(int itemId)
    {
        // 할당되지 않은 필드가 존재하면 return
        if (database == null) return;
        if (icon == null) return;
        if (itemName == null) return;
        if (description == null) return;

        ItemData data = database.GetItem(itemId);

        if (data == null) return;

        icon.sprite = data.Icon;
        itemName.text = data.ItemName;
        description.text = data.Description;
    }
}

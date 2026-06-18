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

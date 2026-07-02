using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private CounterSelector counterSelector;
    [SerializeField] private Image resultIcon;
    [SerializeField] private Image firstMaterial;
    [SerializeField] private Image secondMaterial;

    [SerializeField] private TextMeshProUGUI resultCountText;
    [SerializeField] private TextMeshProUGUI mat1CountText;
    [SerializeField] private TextMeshProUGUI mat2CountText;

    public Action<int, int> OnCraft;

    public ItemData CurItem { get; private set; }
    public int RecipeId { get; private set; } = -1;
    public int MaxCount { get; private set; }

    public void RecvRecipeData(CraftRecipeData recipeData, bool firstMatActive, bool secondMatActive, int maxCount)
    {
        if (CraftUIController.Database == null) return;
        if (recipeData == null) return;

        RecipeId = recipeData.RecipeId;
        MaxCount = maxCount;

        ItemData resultItem = CraftUIController.Database.GetItem(recipeData.ResultItemId);
        if (resultItem != null)
        {
            PaintUI(resultIcon, resultCountText, resultItem, recipeData.ResultCount);
            CurItem = resultItem;
        }

        ItemData firstMatItem = CraftUIController.Database.GetItem(recipeData.Material1ItemId);
        if (firstMatItem != null)
            PaintUI(firstMaterial, mat1CountText, firstMatItem, recipeData.Material1Count, firstMatActive);

        ItemData secondMatItem = CraftUIController.Database.GetItem(recipeData.Material2ItemId);
        if (secondMatItem != null)
            PaintUI(secondMaterial, mat2CountText, secondMatItem, recipeData.Material2Count, secondMatActive);

        if (counterSelector != null)
            counterSelector.Init(0, maxCount);
    }
    public void OnClickCraftButton()
    {
        if(MaxCount <= 0) return;

        if(CurItem != null && CurItem.IsStackable)
        {
            if(counterSelector != null &&  OnCraft != null)
            {
                counterSelector.gameObject.SetActive(true);
            }
        }
        else
        {
            OnCraft?.Invoke(RecipeId, 1);
        }
    }
    public void OnClickCraftStackable()
    {
        OnCraft?.Invoke(RecipeId, counterSelector.Count);
    }
    private void PaintUI(Image icon, TextMeshProUGUI text, ItemData itemData, int count, bool active = true)
    {
        if (icon == null || text == null) return;

        if(itemData == null)
        {
            icon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            text.text = "";
            return;
        }

        icon.sprite = itemData.Icon;
        if(active)
            icon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        else
            icon.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);

        if (count <= 1)
            text.text = "";
        else
            text.text = count.ToString();
        Debug.Log(count);
    }
}

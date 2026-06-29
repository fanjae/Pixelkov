using System;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private CounterSelector counterSelector;
    [SerializeField] private Image resultIcon;
    [SerializeField] private Image firstMaterial;
    [SerializeField] private Image secondMaterial;

    public Action<int, int> OnCraft;

    public ItemData CurItem { get; private set; }
    public int RecipeId { get; private set; } = -1;

    public void RecvRecipeData(CraftRecipeData recipeData, bool firstMatActive, bool secondMatActive, int maxCount)
    {
        if (CraftUIController.Database == null) return;
        if (recipeData == null) return;

        RecipeId = recipeData.RecipeId;

        ItemData resultItem = CraftUIController.Database.GetItem(recipeData.ResultItemId);
        if (resultItem != null)
        {
            PaintUI(resultIcon, resultItem);
            CurItem = resultItem;
        }

        ItemData firstMatItem = CraftUIController.Database.GetItem(recipeData.Material1ItemId);
        if (firstMatItem != null)
            PaintUI(firstMaterial, firstMatItem, firstMatActive);

        ItemData secondMatItem = CraftUIController.Database.GetItem(recipeData.Material2ItemId);
        if (secondMatItem != null)
            PaintUI(secondMaterial, secondMatItem, secondMatActive);

        if (counterSelector != null)
            counterSelector.Init(0, maxCount);
    }
    public void UpdateCounterSelector(int maxCount)
    {
        counterSelector.Init(0, maxCount);
    }
    public void OnClickCraftButton()
    {
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
    public void OnClickSellStackable()
    {
        OnCraft?.Invoke(RecipeId, counterSelector.Count);
    }
    private void PaintUI(Image icon, ItemData itemData, bool active = true)
    {
        if (icon == null) return;

        if(itemData == null)
        {
            icon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            return;
        }

        icon.sprite = itemData.Icon;
        if(active)
            icon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        else
            icon.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeListPanel : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RecipeUI recipePrefab;

    private List<RecipeUI> RecipeUIList = new List<RecipeUI>();

    public void AddRecipe(CraftRecipeData recipeData, List<CraftMaterialStatus> materialStatus, Action<int, int> Craft, int maxCount)
    {
        // CombData에서 결과 아이템, 재료1, 재료2 뽑아서 전달
        if (content == null || recipePrefab == null || recipeData == null || materialStatus == null) return;

        RecipeUI recipeUI = Instantiate(recipePrefab, content);
        if (recipeUI != null && materialStatus.Count >= 2)
        {
            RecipeUIList.Add(recipeUI);
            if (materialStatus[0] == null) return;
            bool mat1Status = materialStatus[0].OwnedCount >= materialStatus[0].RequiredCount;

            if (materialStatus[1] == null) return;
            bool mat2Status = materialStatus[1].OwnedCount >= materialStatus[1].RequiredCount;

            recipeUI.RecvRecipeData(recipeData, mat1Status, mat2Status, maxCount);
            recipeUI.OnCraft += Craft;
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, RecipeUIList.Count * 130);
    }
    public void UpdateUI(CraftRecipeData recipeData, List<CraftMaterialStatus> materialStatus, int maxCount)
    {
        if (content == null || recipePrefab == null || recipeData == null || materialStatus == null) return;

        foreach(RecipeUI recipeUI in RecipeUIList)
        {
            if (recipeUI != null && materialStatus.Count >= 2)
            {
                if (materialStatus[0] == null) continue;
                bool mat1Status = materialStatus[0].OwnedCount >= materialStatus[0].RequiredCount;

                if (materialStatus[1] == null) continue;
                bool mat2Status = materialStatus[1].OwnedCount >= materialStatus[1].RequiredCount;

                recipeUI.RecvRecipeData(recipeData, mat1Status, mat2Status, maxCount);
            }
        }
    }
    public void ReleaseRecipe(Action<int, int> Craft)
    {
        foreach(RecipeUI recipeUI in RecipeUIList)
        {
            if(recipeUI != null)
                recipeUI.OnCraft -= Craft;
        }
    }
}

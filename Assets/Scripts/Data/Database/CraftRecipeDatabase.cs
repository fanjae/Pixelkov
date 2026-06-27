using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft/Recipe Database")]
public class CraftRecipeDatabase : ScriptableObject
{
    // 전체 제작 레시피 목록
    [SerializeField] private List<CraftRecipeData> recipes = new();

    // RecipeId로 찾기 위한 Dictionary
    private Dictionary<int, CraftRecipeData> recipeMap;

    // 외부에서 레시피 목록 조회
    public IReadOnlyList<CraftRecipeData> Recipes => recipes;

    // Recipes 리스트를 RecipeId 기준 Dictionary로 변환
    public void Initialize()
    {
        recipeMap = new Dictionary<int, CraftRecipeData>();

        foreach (CraftRecipeData recipe in recipes)
        {
            if (recipe == null) continue;

            recipeMap[recipe.RecipeId] = recipe;
        }
    }

    // RecipeId에 해당하는 CraftRecipeData 반환
    public CraftRecipeData GetRecipe(int recipeId)
    {
        // 아직 초기화 되지 않았다면 1회 초기화 진행
        if (recipeMap == null) Initialize();

        recipeMap.TryGetValue(recipeId, out CraftRecipeData recipe);
        return recipe;
    }
}
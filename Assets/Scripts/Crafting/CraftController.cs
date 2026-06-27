using System;

public class CraftController
{
    private readonly Inventory inventory;
    private readonly ItemDatabase itemDatabase;
    private readonly CraftRecipeDatabase recipeDatabase;

    public CraftController(Inventory inventory,ItemDatabase itemDatabase,CraftRecipeDatabase recipeDatabase)
    {
        // 인벤토리, 아이템 DB, 레시피 DB 주입 받아 사용
        this.inventory = inventory;
        this.itemDatabase = itemDatabase;
        this.recipeDatabase = recipeDatabase;
    }

    // 해당 레시피로 제작 가능한지 확인
    public bool CanCraft(int recipeId)
    {
        // RecipeId로 레시피 조회
        CraftRecipeData recipe = recipeDatabase.GetRecipe(recipeId);

        // 레시피 데이터가 유효하지 않으면 제작 불가
        if (!IsValidRecipe(recipe)) return false;

        // 결과 아이템 데이터 조회
        ItemData resultItem = itemDatabase.GetItem(recipe.ResultItemId);
        if (resultItem == null) return false;

        // 필요한 재료를 보유하고 있는지 확인
        if (!inventory.HasItem(recipe.Material1ItemId, recipe.Material1Count)) return false;
        if (!inventory.HasItem(recipe.Material2ItemId, recipe.Material2Count)) return false;

        // 결과 아이템을 인벤토리에 추가할 공간이 있는지 확인
        return inventory.CanAddItem(resultItem, recipe.ResultCount);
    }

    // 레시피 기준으로 아이템 제작
    public bool Craft(int recipeId)
    {
        // 제작 조건을 만족하지 않으면 제작 불가
        if (!CanCraft(recipeId)) return false;

        // 제작에 사용할 레시피와 결과 아이템 조회
        CraftRecipeData recipe = recipeDatabase.GetRecipe(recipeId);
        ItemData resultItem = itemDatabase.GetItem(recipe.ResultItemId);
        if (resultItem == null) return false;

        // 재료 아이템 소모
        if (!inventory.RemoveItem(recipe.Material1ItemId, recipe.Material1Count)) return false;
        if (!inventory.RemoveItem(recipe.Material2ItemId, recipe.Material2Count)) return false;

        // 결과 아이템 추가
        if (!inventory.AddItem(resultItem, recipe.ResultCount)) return false;

        return true;
    }

    // 레시피 데이터가 제작에 사용할 수 있는 상태인지 검증
    private bool IsValidRecipe(CraftRecipeData recipe)
    {
        if (recipe == null) return false;

        // 결과 아이템 ID와 결과 수량 검증
        if (recipe.ResultItemId <= 0 || recipe.ResultCount <= 0) return false;

        // 재료 아이템 ID와 필요 수량 검증
        if (recipe.Material1ItemId <= 0 || recipe.Material1Count <= 0) return false;
        if (recipe.Material2ItemId <= 0 || recipe.Material2Count <= 0) return false;

        return true;
    }
}
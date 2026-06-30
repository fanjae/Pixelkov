using System;
using System.Collections.Generic;

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
    public bool CanCraft(int recipeId, int craftCount = 1)
    {
        if (craftCount <= 0) return false;

        CraftRecipeData recipe = recipeDatabase.GetRecipe(recipeId);
        if (!IsValidRecipe(recipe)) return false;

        ItemData resultItem = itemDatabase.GetItem(recipe.ResultItemId);
        if (resultItem == null) return false;

        // 스택 불가능한 결과 아이템은 여러 개 제작 불가
        if (!resultItem.IsStackable && craftCount > 1) return false;

        if (!inventory.HasItem(recipe.Material1ItemId, recipe.Material1Count * craftCount)) return false;
        if (!inventory.HasItem(recipe.Material2ItemId, recipe.Material2Count * craftCount)) return false;

        return inventory.CanAddItem(resultItem, recipe.ResultCount * craftCount);
    }

    // 레시피 기준으로 아이템 제작
    public bool Craft(int recipeId, int craftCount = 1)
    {
        if (!CanCraft(recipeId, craftCount)) return false;

        CraftRecipeData recipe = recipeDatabase.GetRecipe(recipeId);
        ItemData resultItem = itemDatabase.GetItem(recipe.ResultItemId);
        if (resultItem == null) return false;

        if (!inventory.RemoveItem(recipe.Material1ItemId, recipe.Material1Count * craftCount)) return false;
        if (!inventory.RemoveItem(recipe.Material2ItemId, recipe.Material2Count * craftCount)) return false;

        if (!inventory.AddItem(resultItem, recipe.ResultCount * craftCount)) return false;

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

    // 레시피에 필요한 재료들의 보유 상태 목록을 조회
    public List<CraftMaterialStatus> GetMaterialStatuses(int recipeId, int craftCount = 1)
    {
        List<CraftMaterialStatus> result = new();

        if (craftCount <= 0) return result;

        CraftRecipeData recipe = recipeDatabase.GetRecipe(recipeId);
        if (!IsValidRecipe(recipe)) return result;

        AddMaterialStatus(result, recipe.Material1ItemId, recipe.Material1Count * craftCount);
        AddMaterialStatus(result, recipe.Material2ItemId, recipe.Material2Count * craftCount);

        return result;
    }

    // 단일 재료의 필요 개수와 현재 보유 개수를 상태 목록에 추가
    private void AddMaterialStatus(List<CraftMaterialStatus> result,int itemId,int requiredCount)
    {
        ItemData itemData = itemDatabase.GetItem(itemId);

        // 현재 인벤토리에 보유 중인 재료 개수 조회
        int ownedCount = inventory.GetItemCount(itemId);

        result.Add(new CraftMaterialStatus(itemId,itemData,requiredCount,ownedCount));
    }

    // 해당 레시피로 최대로 제작 가능한 횟수 조회
    public int GetMaxCraftCount(int recipeId)
    {
        CraftRecipeData recipe = recipeDatabase.GetRecipe(recipeId);
        if (!IsValidRecipe(recipe)) return 0;

        ItemData resultItem = itemDatabase.GetItem(recipe.ResultItemId);
        if (resultItem == null) return 0;

        // 결과 아이템이 스택 불가능하면 1회 제작까지만 허용
        if (!resultItem.IsStackable)
        {
            return CanCraft(recipeId) ? 1 : 0;
        }

        // 각 재료의 보유 수량
        int material1OwnedCount = inventory.GetItemCount(recipe.Material1ItemId);
        int material2OwnedCount = inventory.GetItemCount(recipe.Material2ItemId);

        // 각 재료 기준으로 제작 가능한 횟수
        int material1CraftableCount = material1OwnedCount / recipe.Material1Count;
        int material2CraftableCount = material2OwnedCount / recipe.Material2Count;

        // 두 재료 중 더 적게 만들 수 있는 횟수가 최종 제작 가능 횟수.
        return Math.Min(material1CraftableCount, material2CraftableCount);
    }
}
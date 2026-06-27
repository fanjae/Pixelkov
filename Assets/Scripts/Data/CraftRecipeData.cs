using UnityEngine;

[CreateAssetMenu(menuName = "Craft/Recipe")]
public class CraftRecipeData : ScriptableObject
{
    [Header("레시피 ID")]
    [SerializeField] private int recipeId; // 고유 ID

    [Header("결과물")] // 완성품
    [SerializeField] private int resultItemId; 
    [SerializeField] private int resultCount = 1; 

    [Header("재료 1")] // 첫번째 재료
    [SerializeField] private int material1ItemId; 
    [SerializeField] private int material1Count = 1;

    [Header("재료 2")] // 두번째 재료
    [SerializeField] private int material2ItemId;
    [SerializeField] private int material2Count = 1;

    public int RecipeId => recipeId;

    public int ResultItemId => resultItemId;
    public int ResultCount => resultCount;

    public int Material1ItemId => material1ItemId;
    public int Material1Count => material1Count;

    public int Material2ItemId => material2ItemId;
    public int Material2Count => material2Count;
}


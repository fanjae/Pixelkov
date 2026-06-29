using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftUIController : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public static ItemDatabase Database {  get; private set; }

    [Header("외부 참조 컴포넌트")]
    [SerializeField] private ItemDatabase database;
    [SerializeField] private CraftRecipeDatabase recipeDatabase;
    [SerializeField] private Player player;

    [Header("캔버스 내 참조 컴포넌트")]
    [SerializeField] private RecipeListPanel recipeListPanel;

    private CraftController craftController;
    private Vector2 offset = Vector2.zero;

    void Awake()
    {
        if (player == null) player = FindAnyObjectByType<Player>();

        if(database != null)
        {
            Database = database;
        }
        if(player != null && database != null && recipeDatabase != null)
        {
            craftController = new CraftController(player.Inventory, database, recipeDatabase);
        }
        InitRecipe();
    }
    private void OnEnable()
    {
        player.Inventory.OnInventoryChanged += UpdateCounter;
    }
    private void OnDisable()
    {
        player.Inventory.OnInventoryChanged -= UpdateCounter;
    }
    private void OnDestroy()
    {
        recipeListPanel.ReleaseRecipe(Craft);
    }
    private void UpdateCounter()
    {
        if (recipeListPanel == null || craftController == null) return;

        foreach(CraftRecipeData recipeData in recipeDatabase.Recipes)
        {
            if (recipeData == null) continue;

            recipeListPanel.UpdateUI(recipeData, craftController.GetMaterialStatuses(recipeData.RecipeId), craftController.GetMaxCraftCount(recipeData.RecipeId));
        }
    }
    private void InitRecipe()
    {
        if (recipeListPanel == null || craftController == null) return;

        foreach(CraftRecipeData recipeData in recipeDatabase.Recipes)
        {
            if (recipeData == null) continue;

            List<CraftMaterialStatus> materialStatus = craftController.GetMaterialStatuses(recipeData.RecipeId);
            if(materialStatus == null) continue;

            recipeListPanel.AddRecipe(recipeData, materialStatus, Craft, craftController.GetMaxCraftCount(recipeData.RecipeId));
        }
    }
    public void Craft(int recipeId, int count)
    {
        if(craftController == null) return;
        craftController.Craft(recipeId, count);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = (Vector2)transform.position - eventData.position;
    }
}

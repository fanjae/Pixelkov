using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("아이디 정보")]
    [SerializeField] private int itemId;

    [Header("기본 정보")]
    [SerializeField] private string itemName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private Sprite icon;

    [Header("분류")]
    [SerializeField] private ItemType itemType;

    [Header("스택 여부")]
    [SerializeField] private bool isStackable = true;
    [SerializeField] private int maxStackCount = 99;

    public int ItemId => itemId;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public int Price => price;
    public ItemType ItemType => itemType;

    public bool IsStackable => isStackable;
    public int MaxStackCount => isStackable ? maxStackCount : 1;

    public abstract void Use(Player player);

#if UNITY_EDITOR
    public void InitForTest(int itemId, string itemName, string description, int price, bool isStackable, int maxStackCount, ItemType itemType = ItemType.Material)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.description = description;
        this.price = price;
        this.isStackable = isStackable;
        this.maxStackCount = maxStackCount;
        this.itemType = itemType;
    }
#endif
}


public class ItemInstance
{
    public int ItemId { get; }

    public ItemRuntimeData RuntimeData { get; }

    public ItemInstance(ItemData itemData)
    {
        ItemId = itemData.ItemId;

        switch (itemData)
        {
            case WeaponData weapon:
                RuntimeData = new WeaponRuntimeData(0);
                break;
        }
    }
    public bool TryGetRuntime<T>(out T runtime) where T : ItemRuntimeData
    {
        runtime = RuntimeData as T;
        return runtime != null;
    }
}
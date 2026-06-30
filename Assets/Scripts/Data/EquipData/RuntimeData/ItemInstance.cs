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
                RuntimeData = new WeaponRuntimeData(weapon.Capacity);
                break;
        }
    }
    public bool TryGetRuntime<T>(out T runtime) where T : ItemRuntimeData
    {
        runtime = RuntimeData as T;
        return runtime != null;
    }
}
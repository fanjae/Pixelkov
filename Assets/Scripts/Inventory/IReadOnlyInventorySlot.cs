public interface IReadOnlyInventorySlot
{
    ItemInstance ItemInstance { get; }
    int Count { get; }
    int ItemId { get; }
    bool IsEmpty { get; }
}
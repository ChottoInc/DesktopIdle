

public abstract class AbstractConsumableHandler<T> : IConsumableHandler where T : ItemSO
{
    public bool Use(ItemSO item)
    {
        if (item is T typedItem)
           return UseItem(typedItem);
        return false;
    }

    protected abstract bool UseItem(T item);
}

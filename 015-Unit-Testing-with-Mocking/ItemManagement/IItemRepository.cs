namespace ItemManagement;

public interface IItemRepository
{
    void AddItem(Item item);
    Item? GetItemById(int id);
    IEnumerable<Item> GetAllItems();
    void UpdateItem(Item item);
    void DeleteItem(int id);
}

public class InMemoryItemRepository : IItemRepository
{
    private readonly Dictionary<int, Item> _items = [];
    private int _nextId = 1;

    public void AddItem(Item item)
    {
        item.Id = _nextId++;
        _items[item.Id] = item;
    }

    public Item? GetItemById(int id) => _items.GetValueOrDefault(id);

    public IEnumerable<Item> GetAllItems() => _items.Values;

    public void UpdateItem(Item item) => _items[item.Id] = item;

    public void DeleteItem(int id) => _items.Remove(id);
}

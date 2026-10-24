namespace ItemManagement;

public class ItemService(IItemRepository repository)
{
    public const int MaxNameLength = 10;

    public void AddItem(string name)
    {
        // The course version validated names only in ValidateItemName and never called it, so invalid items were saved.
        EnsureValidName(name);
        repository.AddItem(new Item { Name = name });
    }

    public IEnumerable<Item> GetAllItems() => repository.GetAllItems();

    public Item? GetItemById(int id) => repository.GetItemById(id);

    public void UpdateItem(int id, string newName)
    {
        EnsureValidName(newName);
        var item = repository.GetItemById(id);
        if (item != null)
        {
            item.Name = newName;
            repository.UpdateItem(item);
        }
    }

    public void DeleteItem(int id) => repository.DeleteItem(id);

    public bool ValidateItemName(string? name) => !string.IsNullOrEmpty(name) && name.Length <= MaxNameLength;

    private void EnsureValidName(string name)
    {
        if (!ValidateItemName(name))
        {
            throw new ArgumentException($"Item name must be 1 to {MaxNameLength} characters long.", nameof(name));
        }
    }
}

using ItemManagement;

var service = new ItemService(new InMemoryItemRepository());
service.AddItem("Laptop");
service.AddItem("Mouse");
service.UpdateItem(2, "Keyboard");

foreach (var item in service.GetAllItems())
{
    Console.WriteLine($"{item.Id}: {item.Name}");
}

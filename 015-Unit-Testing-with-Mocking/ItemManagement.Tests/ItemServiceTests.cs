using Moq;

namespace ItemManagement.Tests;

public class ItemServiceTests
{
    private Mock<IItemRepository> _repository = null!;
    private ItemService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new Mock<IItemRepository>();
        _service = new ItemService(_repository.Object);
    }

    [Test]
    public void AddItem_ValidName_CallsRepository()
    {
        _service.AddItem("Laptop");

        _repository.Verify(r => r.AddItem(It.Is<Item>(i => i.Name == "Laptop")), Times.Once);
    }

    [TestCase("")]
    [TestCase(null)]
    [TestCase("ElevenChars")]
    public void AddItem_InvalidName_ThrowsAndDoesNotSave(string? name)
    {
        Assert.Throws<ArgumentException>(() => _service.AddItem(name!));
        _repository.Verify(r => r.AddItem(It.IsAny<Item>()), Times.Never);
    }

    [Test]
    public void GetAllItems_ReturnsRepositoryItems()
    {
        var items = new List<Item> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } };
        _repository.Setup(r => r.GetAllItems()).Returns(items);

        Assert.That(_service.GetAllItems(), Is.EqualTo(items));
    }

    [Test]
    public void GetAllItems_EmptyRepository_ReturnsEmpty()
    {
        _repository.Setup(r => r.GetAllItems()).Returns([]);

        Assert.That(_service.GetAllItems(), Is.Empty);
    }

    [Test]
    public void GetItemById_ExistingId_ReturnsItem()
    {
        var item = new Item { Id = 1, Name = "Laptop" };
        _repository.Setup(r => r.GetItemById(1)).Returns(item);

        Assert.That(_service.GetItemById(1), Is.SameAs(item));
    }

    [Test]
    public void GetItemById_MissingId_ReturnsNull()
    {
        _repository.Setup(r => r.GetItemById(It.IsAny<int>())).Returns((Item?)null);

        Assert.That(_service.GetItemById(42), Is.Null);
    }

    [Test]
    public void UpdateItem_ExistingItem_UpdatesName()
    {
        var item = new Item { Id = 1, Name = "Old" };
        _repository.Setup(r => r.GetItemById(1)).Returns(item);

        _service.UpdateItem(1, "New");

        Assert.That(item.Name, Is.EqualTo("New"));
        _repository.Verify(r => r.UpdateItem(item), Times.Once);
    }

    [Test]
    public void UpdateItem_MissingItem_DoesNotUpdate()
    {
        _repository.Setup(r => r.GetItemById(1)).Returns((Item?)null);

        _service.UpdateItem(1, "New");

        _repository.Verify(r => r.UpdateItem(It.IsAny<Item>()), Times.Never);
    }

    [Test]
    public void UpdateItem_InvalidName_Throws()
    {
        Assert.Throws<ArgumentException>(() => _service.UpdateItem(1, ""));
        _repository.Verify(r => r.GetItemById(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public void DeleteItem_CallsRepository()
    {
        _service.DeleteItem(7);

        _repository.Verify(r => r.DeleteItem(7), Times.Once);
    }

    [TestCase("Valid", true)]
    [TestCase("TenCharsOk", true)]
    [TestCase("ElevenChars", false)]
    [TestCase("", false)]
    [TestCase(null, false)]
    public void ValidateItemName_ChecksLength(string? name, bool expected)
    {
        Assert.That(_service.ValidateItemName(name), Is.EqualTo(expected));
    }
}

public class InMemoryItemRepositoryTests
{
    [Test]
    public void Repository_SupportsCrud()
    {
        var repository = new InMemoryItemRepository();
        repository.AddItem(new Item { Name = "A" });
        repository.AddItem(new Item { Name = "B" });

        repository.UpdateItem(new Item { Id = 1, Name = "A2" });
        repository.DeleteItem(2);

        Assert.That(repository.GetAllItems().Select(i => i.Name), Is.EqualTo(new[] { "A2" }));
        Assert.That(repository.GetItemById(2), Is.Null);
    }
}

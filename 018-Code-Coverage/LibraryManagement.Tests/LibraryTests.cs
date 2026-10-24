namespace LibraryManagement.Tests;

public class LibraryTests
{
    private Library _library = null!;

    [SetUp]
    public void SetUp()
    {
        _library = new Library();
        _library.AddBook(new Book { Id = 1, Title = "1984", Author = "George Orwell" });
    }

    [Test]
    public void AddBook_AddsToCollection()
    {
        _library.AddBook(new Book { Id = 2, Title = "Dune", Author = "Frank Herbert" });

        Assert.That(_library.GetAllBooks().Select(b => b.Title), Is.EqualTo(new[] { "1984", "Dune" }));
    }

    [Test]
    public void AddBook_Null_Throws() => Assert.Throws<ArgumentNullException>(() => _library.AddBook(null!));

    [Test]
    public void AddBook_DuplicateId_Throws() =>
        Assert.Throws<InvalidOperationException>(() => _library.AddBook(new Book { Id = 1, Title = "Copy" }));

    [Test]
    public void CheckOutBook_AvailableBook_ReturnsTrueAndMarksIt()
    {
        Assert.That(_library.CheckOutBook(1), Is.True);
        Assert.That(_library.GetAllBooks()[0].IsCheckedOut, Is.True);
    }

    [Test]
    public void CheckOutBook_AlreadyCheckedOut_ReturnsFalse()
    {
        _library.CheckOutBook(1);

        Assert.That(_library.CheckOutBook(1), Is.False);
    }

    [Test]
    public void CheckOutBook_MissingBook_ReturnsFalse() => Assert.That(_library.CheckOutBook(99), Is.False);

    [Test]
    public void ReturnBook_CheckedOutBook_ReturnsTrueAndMarksAvailable()
    {
        _library.CheckOutBook(1);

        Assert.That(_library.ReturnBook(1), Is.True);
        Assert.That(_library.GetAllBooks()[0].IsCheckedOut, Is.False);
    }

    [Test]
    public void ReturnBook_NotCheckedOut_ReturnsFalse() => Assert.That(_library.ReturnBook(1), Is.False);

    [Test]
    public void ReturnBook_MissingBook_ReturnsFalse() => Assert.That(_library.ReturnBook(99), Is.False);

    [Test]
    public void GetAllBooks_IsReadOnly() =>
        Assert.That(_library.GetAllBooks(), Is.InstanceOf<System.Collections.ObjectModel.ReadOnlyCollection<Book>>());
}

namespace MyArrayExample.Tests;

public class MyArrayTests
{
    [Test]
    public void Constructor_FillsWithIndexes() => Assert.That(new MyArray(4).Items, Is.EqualTo(new[] { 0, 1, 2, 3 }));

    [Test]
    public void Constructor_NegativeSize_Throws() => Assert.Throws<ArgumentOutOfRangeException>(() => new MyArray(-1));

    [TestCase(0)]
    [TestCase(4)]
    public void Replace_ValidPosition_ReplacesValue(int position)
    {
        var array = new MyArray(5);

        Assert.That(array.Replace(position, 42), Is.True);
        Assert.That(array.Items[position], Is.EqualTo(42));
    }

    [TestCase(-1)]
    [TestCase(5)]
    [TestCase(6)]
    public void Replace_InvalidPosition_Throws(int position) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new MyArray(5).Replace(position, 1));

    [Test]
    public void FindMax_ReturnsLargestValue()
    {
        var array = new MyArray(3);
        array.Replace(1, 100);

        Assert.That(array.FindMax(), Is.EqualTo(100));
    }

    [Test]
    public void FindMax_AllNegative_ReturnsLargestValue()
    {
        var array = new MyArray(2);
        array.Replace(0, -5);
        array.Replace(1, -3);

        Assert.That(array.FindMax(), Is.EqualTo(-3));
    }

    [Test]
    public void FindMax_EmptyArray_Throws() => Assert.Throws<InvalidOperationException>(() => new MyArray(0).FindMax());
}

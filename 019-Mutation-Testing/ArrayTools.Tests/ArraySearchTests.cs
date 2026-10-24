namespace ArrayTools.Tests;

// Boundary cases (first, last, duplicates, missing) are what kill Stryker's loop and comparison mutants.
public class ArraySearchTests
{
    [TestCase(new[] { 7, 2, 3 }, 7, 0)]
    [TestCase(new[] { 1, 2, 3 }, 2, 1)]
    [TestCase(new[] { 1, 2, 3 }, 3, 2)]
    [TestCase(new[] { 5, 9, 5 }, 5, 0)]
    [TestCase(new[] { -4, 0, 4 }, 0, 1)]
    public void FindIndex_ExistingElement_ReturnsFirstIndex(int[] array, int element, int expected) =>
        Assert.That(ArraySearch.FindIndex(array, element), Is.EqualTo(expected));

    [TestCase(new[] { 1, 2, 3 }, 4)]
    [TestCase(new int[0], 1)]
    public void FindIndex_MissingElement_ReturnsMinusOne(int[] array, int element) =>
        Assert.That(ArraySearch.FindIndex(array, element), Is.EqualTo(-1));

    [Test]
    public void FindIndex_NullArray_Throws() =>
        Assert.Throws<ArgumentNullException>(() => ArraySearch.FindIndex(null!, 1));
}

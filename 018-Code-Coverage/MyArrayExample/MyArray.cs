namespace MyArrayExample;

public class MyArray
{
    private readonly int[] _items;

    public MyArray(int size)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(size);
        _items = Enumerable.Range(0, size).ToArray();
    }

    public IReadOnlyList<int> Items => _items;

    public bool Replace(int position, int newValue)
    {
        // The course version checked "position > Length", letting position == Length through to an IndexOutOfRangeException.
        if (position < 0 || position >= _items.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position must be a valid index.");
        }
        _items[position] = newValue;
        return true;
    }

    public int FindMax() => _items.Length == 0 ? throw new InvalidOperationException("Array is empty") : _items.Max();
}

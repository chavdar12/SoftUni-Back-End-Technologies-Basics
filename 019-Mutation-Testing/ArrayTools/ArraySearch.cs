namespace ArrayTools;

public static class ArraySearch
{
    public static int FindIndex(int[] array, int element)
    {
        ArgumentNullException.ThrowIfNull(array);
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] == element)
            {
                return i;
            }
        }
        return -1;
    }
}

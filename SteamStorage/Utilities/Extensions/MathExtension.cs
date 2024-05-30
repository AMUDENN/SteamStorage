using System;

namespace SteamStorage.Utilities.Extensions;

public static class MathExtension
{
    public static bool IsBetweenInclusive<T>(this T item, T start, T end) where T : IComparable
    {
        return item.CompareTo(start) >= 0 && item.CompareTo(end) <= 0;
    }
}

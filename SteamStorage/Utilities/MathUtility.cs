using System;

namespace SteamStorage.Utilities;

public static class MathUtility
{
    public static decimal Clamp(decimal val, decimal min, decimal max)
    {
        if (min > max)
            throw new ArgumentException($"{min} cannot be greater than {max}.");
        return Math.Min(Math.Max(val, min), max);
    }
}

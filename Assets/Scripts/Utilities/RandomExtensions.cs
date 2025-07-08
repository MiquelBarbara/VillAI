using System;

public static class RandomExtensions
{
    public static bool NextBool(this Random rng)
    {
        return rng.Next(2) == 0;
    }
}
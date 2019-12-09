using UnityEngine;

public static class Approximation
{
    /// <summary>Determines if a value is approximately equal to another in a given range.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <param name="tolerance">The tolerant range in which a value is equal to another.</param>
    /// <returns>True if the value is approximately equal, false if not.</returns>
    public static bool Float (float a, float b, float tolerance)
    {
        return ( Mathf.Abs (a - b) < tolerance );
    }

    /// <summary>Determines if a value is approximately equal to another in a given range.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <param name="tolerance">The tolerant range in which a value is equal to another.</param>
    /// <returns>True if the value is approximately equal, false if not.</returns>
    public static bool Vector2 (Vector2 a, Vector2 b, float tolerance)
    {
        return ( Mathf.Abs (a.x - b.x) < tolerance && Mathf.Abs (a.y - b.y) < tolerance && Mathf.Abs (a.y - b.y) < tolerance );
    }

    /// <summary>Determines if a value is approximately equal to another in a given range.</summary>
    /// <param name="a">The first value to compare.</param>
    /// <param name="b">The second value to compare.</param>
    /// <param name="tolerance">The tolerant range in which a value is equal to another.</param>
    /// <returns>True if the value is approximately equal, false if not.</returns>
    public static bool Vector3 (Vector3 a, Vector3 b, float tolerance)
    {
        return ( Mathf.Abs (a.x - b.x) < tolerance && Mathf.Abs (a.y - b.y) < tolerance && Mathf.Abs (a.y - b.y) < tolerance );
    }
}

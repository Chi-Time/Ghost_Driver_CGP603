using UnityEngine;

static class Approximation
{
    public static bool Float (float a, float b, float tolerance)
    {
        return ( Mathf.Abs (a - b) < tolerance );
    }

    public static bool Vector2 (Vector2 a, Vector2 b, float tolerance)
    {
        return ( Mathf.Abs (a.x - b.x) < tolerance && Mathf.Abs (a.y - b.y) < tolerance && Mathf.Abs (a.y - b.y) < tolerance );
    }

    public static bool Vector3 (Vector3 a, Vector3 b, float tolerance)
    {
        return ( Mathf.Abs (a.x - b.x) < tolerance && Mathf.Abs (a.y - b.y) < tolerance && Mathf.Abs (a.y - b.y) < tolerance );
    }
}

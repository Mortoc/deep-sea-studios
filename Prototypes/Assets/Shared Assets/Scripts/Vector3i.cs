using UnityEngine;
using System.Collections;

public struct Vector3i
{
    public static readonly Vector3i zero = new Vector3i(0, 0, 0);

    public static readonly Vector3i up = new Vector3i(0, 1, 0);
    public static readonly Vector3i down = new Vector3i(0, -1, 0);

    public static readonly Vector3i left = new Vector3i(-1, 0, 0);
    public static readonly Vector3i right = new Vector3i(1, 0, 0);

    public static readonly Vector3i forward = new Vector3i(0, 0, 1);
    public static readonly Vector3i backward = new Vector3i(0, 0, -1);

    public int x;
    public int y;
    public int z;

    public Vector3i(int x_, int y_, int z_)
    {
        x = x_;
        y = y_;
        z = z_;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1}, {2})", x, y, z);
    }

    public static Vector3i operator +(Vector3i a, Vector3i b)
    {
        return new Vector3i()
        {
            x = a.x + b.x,
            y = a.y + b.y,
            z = a.z + b.z
        };
    }
    public static Vector3i operator -(Vector3i a, Vector3i b)
    {
        return new Vector3i()
        {
            x = a.x - b.x,
            y = a.y - b.y,
            z = a.z - b.z
        };
    }
}
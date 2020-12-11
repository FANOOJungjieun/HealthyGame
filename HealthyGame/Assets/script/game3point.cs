using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class game3point
{
    public int x;
    public int y;

    public game3point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    public void mult(int m)
    {
        x *= m;
        y *= m;
    }

    public void add(game3point p)
    {
        x += p.x;
        y += p.y;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    public bool Equals(game3point p)
    {
        return (x == p.x && y == p.y);
    }

    public static game3point fromVector(Vector2 v)
    {
        return new game3point((int)v.x, (int)v.y);
    }

    public static game3point fromVector(Vector3 v)
    {
        return new game3point((int)v.x, (int)v.y);
    }

    public static game3point mult(game3point p, int m)
    {
        return new game3point(p.x * m, p.y * m);
    }

    public static game3point add(game3point p, game3point o)
    {
        return new game3point(p.x + o.x, p.y +o.y);
    }

    public static game3point clone(game3point p)
    {
        return new game3point(p.x, p.y);
    }

    public static game3point zero
    {
        get { return new game3point(0, 0); }
    }

    public static game3point one
    {
        get { return new game3point(1, 1); }
    }

    public static game3point up
    {
        get { return new game3point(0, 1); }
    }

    public static game3point down
    {
        get { return new game3point(0, -1); }
    }

    public static game3point right
    {
        get { return new game3point(1, 0); }
    }

    public static game3point left
    {
        get { return new game3point(-1, 0); }
    }
}

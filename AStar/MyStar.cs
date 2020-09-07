using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStar : AStar<MyMapPoint>
{
    protected override List<MyMapPoint> GetAroundFreePoint(MyMapPoint point)
    {
        List<Vector2> arounds = TestMyStar.instance.GetAroundFreeTerrain(point.x, point.y);
        List<MyMapPoint> list = new List<MyMapPoint>();
        for (int i = 0; i < arounds.Count; i++)
        {
            MyMapPoint newPoint = new MyMapPoint((int)arounds[i].x, (int)arounds[i].y);
            list.Add(newPoint);
        }
        return list;
    }
}

public class MyMapPoint : AStarPoint
{
    public AStarPoint parent { get; set; }
    public AStarPoint end { get; set; }

    public int x;
    public int y;

    public float G { get; private set; }

    public float H { get; private set; }

    public float F { get { return G + H; } }

    public bool IsEqual(AStarPoint other)
    {
        MyMapPoint point = other as MyMapPoint;
        return this.x == point.x && this.y == point.y;
    }

    public void UpdateGH()
    {
        MyMapPoint p = parent as MyMapPoint;
        MyMapPoint e = end as MyMapPoint;

        float g = 1;
        if (p.x == x || p.y == y) g = 0.7f;

        G = parent.G + g;

        H = Mathf.Abs(e.x - this.x) + Mathf.Abs(e.y - this.y);
    }

    public MyMapPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
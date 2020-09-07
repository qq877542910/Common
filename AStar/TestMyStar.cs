using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMyStar : MonoBehaviour
{
    public static TestMyStar instance;

    public int width;
    public int height;

    public int startX;
    public int startY;

    public int endX;
    public int endY;

    public bool[,] map;

    private List<MyMapPoint> findPaths = new List<MyMapPoint>();
    private MyStar myStar = new MyStar();

    private void Awake()
    {
        instance = this;

        map = new bool[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                bool free = UnityEngine.Random.Range(0f, 1f) < 0.7f;
                map[i, j] = free;
            }
        }
    }

    [ContextMenu("Find Path")]
    public void Find()
    {
        findPaths.Clear();
        MyMapPoint start = new MyMapPoint(startX ,startY);
        MyMapPoint end = new MyMapPoint(endX, endY);
        findPaths = myStar.Search(start , end);
    }

    private void OnDrawGizmos()
    {
        if (instance == null) return;

        DrawMap();
        DrawStartAndEnd();
        DrawPath();
    }

    void DrawMap()
    {
        Gizmos.color = Color.black;
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                Vector2 start = new Vector2(0, y);
                Vector2 end = new Vector2(width, y);
                Gizmos.DrawLine(start, end);

                start = new Vector2(x, 0);
                end = new Vector2(x, height);
                Gizmos.DrawLine(start, end);

                if (!IsFree(x, y))
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(new Vector3(x, y), Vector3.one);
                }
            }
        }
    }
    void DrawStartAndEnd()
    {
        bool freeStart = IsFree(startX, startY);
        Gizmos.color = freeStart ? Color.black : Color.red;
        Gizmos.DrawSphere(new Vector3(startX, startY), 0.5f);

        bool freeEnd = IsFree(endX, endY);
        Gizmos.color = freeEnd ? Color.black : Color.red;
        Gizmos.DrawSphere(new Vector3(endX, endY), 0.5f);
    }
    void DrawPath()
    {
        Gizmos.color = Color.green;
        if (findPaths != null && findPaths.Count > 0)
        {
            for (int i = 0; i < findPaths.Count; i++)
            {
                Gizmos.DrawCube(new Vector3(findPaths[i].x, findPaths[i].y), Vector3.one * 0.3f);

                if (i > 0)
                {
                    Vector3 start = new Vector3(findPaths[i - 1].x, findPaths[i - 1].y);
                    Vector3 end = new Vector3(findPaths[i].x, findPaths[i].y);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
    }

    public bool IsFree(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return false;

        return map[x, y];
    }

    public List<Vector2> GetAroundFreeTerrain(int x, int y)
    {
        List<Vector2> arounds = new List<Vector2>();

        bool left = IsFree(x - 1, y);
        bool up = IsFree(x, y + 1);
        bool right = IsFree(x + 1, y);
        bool down = IsFree(x, y - 1);

        if (left) arounds.Add(new Vector2(x - 1, y));
        if (up) arounds.Add(new Vector2(x, y + 1));
        if (right) arounds.Add(new Vector2(x + 1, y));
        if (down) arounds.Add(new Vector2(x, y - 1));

        if (left && up && IsFree(x - 1, y + 1)) arounds.Add(new Vector2(x - 1, y + 1));
        if (right && up && IsFree(x + 1, y + 1)) arounds.Add(new Vector2(x + 1, y + 1));
        if (right && down && IsFree(x + 1, y - 1)) arounds.Add(new Vector2(x + 1, y - 1));
        if (left && down && IsFree(x - 1, y - 1)) arounds.Add(new Vector2(x - 1, y - 1));

        return arounds;
    }
}

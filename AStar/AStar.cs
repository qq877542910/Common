using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStar<T> where T : AStarPoint
{
    protected List<T> openList = new List<T>();
    protected List<T> closeList = new List<T>();

    protected abstract List<T> GetAroundFreePoint(T point);

    public virtual List<T> Search(T start, T end)
    {
        openList.Clear();
        closeList.Clear();

        List<T> paths = new List<T>();
        openList.Add(start);

        Search(end, ref paths);
        return paths;
    }

    protected virtual void Search( T end, ref List<T> findPath)
    {
        if (openList.Count == 0) return;
        openList.Sort(Sort);

        T target =  openList[0];
        openList.Remove(target);
        closeList.Add(target);

        if (target.IsEqual(end))
        {
            GetPath(target, ref findPath);
            findPath.Reverse();
            return;
        }

        List<T> aroundPoints = GetAroundFreePoint(target);
        if (aroundPoints != null && aroundPoints.Count > 0)
        {
            for (int i = aroundPoints.Count - 1; i >= 0; i--)
            {
                if (IsOpenOrCloseHave(aroundPoints[i]))
                {
                    aroundPoints.RemoveAt(i);
                    continue;
                }
                aroundPoints[i].parent = target;
                aroundPoints[i].end = end;
                aroundPoints[i].UpdateGH();
            }
            openList.AddRange(aroundPoints);
        }
        Search(end, ref findPath);
    }

    void GetPath(T point, ref List<T> path)
    {
        path.Add(point);
        if (point.parent != null)
        {
            GetPath((T)point.parent, ref path);
        }
    }

    protected int Sort(T a, T b)
    {
        return a.F.CompareTo(b.F);
    }

    protected bool IsOpenOrCloseHave(T target)
    {
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].IsEqual(target)) return true;
        }
        for (int i = 0; i < closeList.Count; i++)
        {
            if (closeList[i].IsEqual(target)) return true;
        }
        return false;
    }
}

public interface AStarPoint
{
    AStarPoint parent { get; set; }
    AStarPoint end { get; set; }
    
    float G { get;}
    float H { get;}
    float F { get;}

    void UpdateGH();
    bool IsEqual(AStarPoint other);
}
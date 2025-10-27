using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AmoebaCoordinator : MonoBehaviour
{
    public List<AmoebaPoint> controlPoints = new();

    public AmoebaCenter centerPoint;
    public List<AmoebaSDFPoint> sdfPoints = new();

    public UnityEvent onPointsRefreshed = new();

    [ContextMenu("Refresh Points")]
    public void RefreshPoints()
    {
        controlPoints = GetComponentsInChildren<AmoebaPoint>().ToList();
        sdfPoints = GetComponentsInChildren<AmoebaSDFPoint>().ToList();
        centerPoint = GetComponentInChildren<AmoebaCenter>();
        onPointsRefreshed.Invoke();
    }

    void Awake()
    {
        RefreshPoints();
    }

    void FixedUpdate()
    {
        RefreshPoints();
    }

    public List<Vector2> controlPointPositions
    {
        get
        {
            List<Vector2> colliderPointsLS = new();
            for (int i = 0; i < controlPoints.Count; i++)
            {
                colliderPointsLS.Add(controlPoints[i].transform.localPosition);
            }
            return colliderPointsLS;
        }
    }
    public Vector2 GetCentroid()
    {
        return GetCentroid(controlPointPositions);
    }
    public List<Vector2> SortClockwiseControlPoints()
    {
        return SortClockwise(controlPointPositions, GetCentroid(controlPointPositions));
    }

    public static Vector2 GetCentroid(List<Vector2> points)
    {
        if (points == null || points.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 sum = Vector2.zero;
        foreach (Vector2 p in points)
        {
            sum += p;
        }

        return sum / points.Count;
    }
    // standard angular point sort
    public static List<Vector2> SortClockwise(List<Vector2> points, Vector2 centroid)
    {
        if (points == null || points.Count == 0)
            return new List<Vector2>();

        List<Vector2> sortedPoints = points.OrderBy(point =>
        {
            float angle = Mathf.Atan2(point.y - centroid.y, point.x - centroid.x);
            return -angle;
        }).ToList();

        return sortedPoints;
    }
}
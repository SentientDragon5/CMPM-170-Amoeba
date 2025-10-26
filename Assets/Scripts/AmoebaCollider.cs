using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class AmoebaCollider : MonoBehaviour
{
    public List<AmoebaPoint> points;
    private EdgeCollider2D col;
    void Start()
    {
        col = GetComponent<EdgeCollider2D>();
    }

    void FixedUpdate()
    {
        List<Vector2> colliderPointsLS = new ();
        for (int i = 0; i < points.Count; i++)
        {
            colliderPointsLS.Add(points[i].transform.localPosition);
        }
        colliderPointsLS = SortClockwise(colliderPointsLS);
        colliderPointsLS.Add(colliderPointsLS[0]); // close the circle
        col.points = colliderPointsLS.ToArray();
    }

    // standard angular point sort
    public static List<Vector2> SortClockwise(List<Vector2> points)
    {
        if (points == null || points.Count == 0)
            return new List<Vector2>();

        Vector2 centroid = GetCentroid(points);

        List<Vector2> sortedPoints = points.OrderBy(point =>
        {
            float angle = Mathf.Atan2(point.y - centroid.y, point.x - centroid.x);
            return -angle;
        }).ToList();

        return sortedPoints;
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
}

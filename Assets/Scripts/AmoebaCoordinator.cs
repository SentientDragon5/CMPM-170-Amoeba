using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmoebaCoordinator : MonoBehaviour
{
    public List<AmoebaPoint> controlPoints = new();
    public List<AmoebaSDFPoint> sdfPoints = new();

    public AmoebaCenter centerPoint;
    public float deathRadius = 13.22f;

    private void OnEnable()
    {
        AmoebaEater.OnPointAdd += UpdateSDFPoints;
        AmoebaPoint.OnPointRemove += UpdateSDFPoints;
    }

    private void OnDisable()
    {
        AmoebaEater.OnPointAdd -= UpdateSDFPoints;
        AmoebaPoint.OnPointRemove -= UpdateSDFPoints;
    }


    void Awake()
    {
        controlPoints = GetComponentsInChildren<AmoebaPoint>().ToList();
        centerPoint = GetComponentInChildren<AmoebaCenter>();
    }

    private void Start()
    {
        UpdateSDFPoints();
    }

    private void UpdateSDFPoints()
    {
        sdfPoints.Clear();
        sdfPoints.Add(centerPoint.GetComponent<AmoebaSDFPoint>());
        foreach (AmoebaPoint point in controlPoints)
        {
            sdfPoints.Add(point.GetComponent<AmoebaSDFPoint>());
        }
    }

    void FixedUpdate()
    {
        centerPoint.transform.localPosition = GetCentroid();
    }

    public List<Vector2> GetControlPointPositions()
    {
        List<Vector2> colliderPointsLS = new ();
        for (int i = 0; i<controlPoints.Count; i++)
            {
                colliderPointsLS.Add(controlPoints[i].transform.localPosition);
            }
        return colliderPointsLS;
    }

    public Vector2 GetCentroid()
    {
        return CaculateCentroid(GetControlPointPositions());
    }
    public List<Vector2> SortClockwiseControlPoints()
    {
        return SortClockwise(GetControlPointPositions(), GetCentroid());
    }

    public static Vector2 CaculateCentroid(List<Vector2> points)
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
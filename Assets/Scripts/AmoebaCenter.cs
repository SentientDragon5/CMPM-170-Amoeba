using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AmoebaCenter : MonoBehaviour
{
    public int lineResolution = 20;
    public int raycastResolution = 80;
    public bool drawLines = false;
    public LayerMask amoebaPointLayer;
    public EdgeCollider2D edgeCollider;

    public List<Transform> points = new List<Transform>();
    private LineRenderer lr;

    private List<Vector2> colliderPoints = new List<Vector2>();

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Start()
    {
        ScanPoints();
        lr.positionCount = lineResolution * points.Count;
        CreateLine();
    }

    private void Update()
    {
        transform.position = CalculateCentroid();
        CreateLine();

    }

    private Vector2 CalculateCentroid()
    {
        float xSum = 0f;
        float ySum = 0f;
        for (int i = 0; i < points.Count; i++)
        {
            xSum += points[i].transform.position.x;
            ySum += points[i].transform.position.y;
        }

        return new Vector2(xSum/ points.Count, ySum/ points.Count);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (drawLines)
        {
            for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
            {
                Gizmos.DrawRay(transform.position, Vector3.Lerp(new Vector3(1, 0, 0), new Vector3(0, 1, 0), t) * 100f);
            }
            for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
            {
                Gizmos.DrawRay(transform.position, Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(-1, 0, 0), t) * 100f);
            }
            for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
            {
                Gizmos.DrawRay(transform.position, Vector3.Lerp(new Vector3(-1, 0, 0), new Vector3(0, -1, 0), t) * 100f);
            }
            for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
            {
                Gizmos.DrawRay(transform.position, Vector3.Lerp(new Vector3(0, -1, 0), new Vector3(1, 0, 0), t) * 100f);
            }
        }
    }

    // yes i know this is bad
    // no i will not try to optimize it
    private void ScanPoints()
    {
        for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Lerp(new Vector3(1, 0, 0), new Vector3(0, 1, 0), t), 100f, amoebaPointLayer);
           
            if (hit && hit.collider.CompareTag("Draggable") && !points.Contains(hit.transform))
            {
                points.Add(hit.transform);
            }
        }
        for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Lerp(new Vector3(0, 1, 0), new Vector3(-1, 0, 0), t), 100f, amoebaPointLayer);
            if (hit && hit.collider.CompareTag("Draggable") && !points.Contains(hit.transform))
            {
                points.Add(hit.transform);
            }
        }
        for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Lerp(new Vector3(-1, 0, 0), new Vector3(0, -1, 0), t), 100f, amoebaPointLayer);
            if (hit && hit.collider.CompareTag("Draggable") && !points.Contains(hit.transform))
            {
                points.Add(hit.transform);
            }
        }
        for (float t = 0; t <= 1; t += 1f / (raycastResolution / 4))
        {
           RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.Lerp(new Vector3(0, -1, 0), new Vector3(1, 0, 0), t), 100f, amoebaPointLayer);
            if (hit && hit.collider.CompareTag("Draggable") && !points.Contains(hit.transform))
            {
                points.Add(hit.transform);
            }
        }
    }

    private void CreateLine()
    {
        int count = 0;
        for (int i = 0; i < points.Count; i++)
        {
            if (i == points.Count - 1)
            {
                for (float t = 0; t < 1; t += 1f / lineResolution)
                {
                    Vector2 point = QuadraticBezierCurve(points[i].transform, transform, points[0], t);
                    lr.SetPosition(count, point);
                    colliderPoints.Add(point);
                    count++;
                }
            }
            else
            {
                for (float t = 0; t < 1; t += 1f / lineResolution)
                {
                    Vector2 point = QuadraticBezierCurve(points[i].transform, transform, points[i + 1], t);
                    lr.SetPosition(count, point);
                    colliderPoints.Add(point);
                    count++;
                }
            }
        }
    }

    private void CreateCollider()
    {
        edgeCollider.points = colliderPoints.ToArray();
    }

    // quadratic bezier curve formula from wiki
    // P1 + (1-t)^2 * (P0 - P1) + t^2(P2 - P1), 0 <= t <= 1
    private Vector2 QuadraticBezierCurve(Transform p0, Transform p1, Transform p2, float t)
    {
        return p1.position + Mathf.Pow(1 - t, 2) * (p0.position - p1.position) + Mathf.Pow(t, 2) * (p2.position - p1.position);
    }

}

using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class AmoebaCenter : MonoBehaviour
{
    public int lineResolution = 20;
    public int raycastResolution = 80;
    public bool drawLines = false;
    public LayerMask amoebaPointLayer;

    public List<Transform> points = new List<Transform>();
    public LineRenderer lr;
    public EdgeCollider2D _collider;

    private void Awake()
    {
        if (lr == null) lr = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        ScanPoints();
        lr.positionCount = lineResolution * points.Count;
    }

    private void Update()
    {
        transform.position = CalculateCentroid();
        CreateLine();
        CreateCollider();
        _collider.transform.localPosition = -transform.position;
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
                    count++;
                }
            }
            else
            {
                for (float t = 0; t < 1; t += 1f / lineResolution)
                {
                    Vector2 point = QuadraticBezierCurve(points[i].transform, transform, points[i + 1], t);
                    lr.SetPosition(count, point);
                    count++;
                }
            }
        }
    }

    readonly List<Vector3> v3 = new List<Vector3>();
    readonly List<Vector2> v2 = new List<Vector2>();


    private void CreateCollider()
    {
        v3.Clear();
        v2.Clear();
        for (int i = 0;i < lr.positionCount ;i++) 
        {
            Vector2 asdf = lr.GetPosition(i);
            v3.Add(new Vector2(asdf.x, asdf.y));
        }

        foreach (var v in v3)
        {
            v2.Add(v);
        }
        _collider.points = v2.ToArray();
    }

    // quadratic bezier curve formula from wiki
    // P1 + (1-t)^2 * (P0 - P1) + t^2(P2 - P1), 0 <= t <= 1
    private Vector2 QuadraticBezierCurve(Transform p0, Transform p1, Transform p2, float t)
    {
        return p1.position + Mathf.Pow(1 - t, 2) * (p0.position - p1.position) + Mathf.Pow(t, 2) * (p2.position - p1.position);
    }

}

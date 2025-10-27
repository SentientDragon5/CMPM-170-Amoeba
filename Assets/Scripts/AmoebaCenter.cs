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

    private AmoebaCoordinator coordinator;
#if UNITY_EDITOR
    void OnValidate()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }
#endif
    
    public List<AmoebaPoint> points => coordinator.controlPoints;
    void Start()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }

    private void Awake()
    {
    }

    private void Update()
    {
        transform.position = CalculateCentroid();
        // CreateCollider();
        // _collider.transform.localPosition = -transform.position;
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

        return new Vector2(xSum / points.Count, ySum / points.Count);
    }

    // quadratic bezier curve formula from wiki
    // P1 + (1-t)^2 * (P0 - P1) + t^2(P2 - P1), 0 <= t <= 1
    private Vector2 QuadraticBezierCurve(Transform p0, Transform p1, Transform p2, float t)
    {
        return p1.position + Mathf.Pow(1 - t, 2) * (p0.position - p1.position) + Mathf.Pow(t, 2) * (p2.position - p1.position);
    }

}

using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class AmoebaCenter : MonoBehaviour
{
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

   

    // quadratic bezier curve formula from wiki
    // P1 + (1-t)^2 * (P0 - P1) + t^2(P2 - P1), 0 <= t <= 1
    private Vector2 QuadraticBezierCurve(Transform p0, Transform p1, Transform p2, float t)
    {
        return p1.position + Mathf.Pow(1 - t, 2) * (p0.position - p1.position) + Mathf.Pow(t, 2) * (p2.position - p1.position);
    }

}

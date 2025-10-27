using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class AmoebaCollider : MonoBehaviour
{
    private AmoebaCoordinator coordinator;
#if UNITY_EDITOR
    void OnValidate()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }
#endif
    
    public List<AmoebaPoint> points => coordinator.controlPoints;
    private EdgeCollider2D col;
    void Start()
    {
        col = GetComponent<EdgeCollider2D>();
        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }

    void FixedUpdate()
    {
        var colliderPointsLS = coordinator.SortClockwiseControlPoints();
        colliderPointsLS.Add(colliderPointsLS[0]); // close the circle
        col.points = colliderPointsLS.ToArray();
    }

    
}

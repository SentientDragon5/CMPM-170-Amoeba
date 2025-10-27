using System;
using UnityEngine;

[RequireComponent(typeof(AmoebaCollider))]
public class AmoebaEater : MonoBehaviour
{
    public static event Action OnPointAdd;

    private AmoebaCoordinator coordinator;

    private EdgeCollider2D col;
    private AmoebaCollider amoebaCollider;

    public AmoebaPoint pointPrefab;
    public Transform pointParent;

    public GameObject foodPrefab;
    public Transform foodParent;

    public float dieRadius;
    public AmoebaCenter center;

    void Start()
    {
        col = GetComponent<EdgeCollider2D>();
        col.isTrigger = true;

        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Food food))
        {
            GainPoint(food);
        }
    }

    void GainPoint(Food food)
    {
        AmoebaPoint newPoint = Instantiate(pointPrefab, food.transform.position, Quaternion.identity, pointParent);
        coordinator.controlPoints.Add(newPoint);
        OnPointAdd?.Invoke();
        Destroy(food.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.transform.position, dieRadius);
    }





}
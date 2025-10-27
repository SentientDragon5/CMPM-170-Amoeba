using System;
using UnityEngine;

[RequireComponent(typeof(AmoebaCollider))]
public class AmoebaEater : MonoBehaviour
{
    public static event Action OnPointAdd;

    private AmoebaCoordinator coordinator;
    private EdgeCollider2D col;

    [Header("Points")]
    public AmoebaPoint pointPrefab;
    public Transform pointParent;

    [Header("Food")]
    public Food foodPrefab;

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
        if (food.Invincible())
            return;
        AmoebaPoint newPoint = Instantiate(pointPrefab, food.transform.position, Quaternion.identity, pointParent);
        coordinator.controlPoints.Add(newPoint);
        OnPointAdd?.Invoke();
        Destroy(food.gameObject);
    }
}
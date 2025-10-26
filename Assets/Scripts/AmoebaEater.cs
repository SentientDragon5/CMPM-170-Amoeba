using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EdgeCollider2D))]
public class AmoebaEater : MonoBehaviour
{
    private EdgeCollider2D col;
    public UnityEvent<Food> onEat;
    void Start()
    {
        col = GetComponent<EdgeCollider2D>();
        col.isTrigger = true;

        onEat.AddListener(GainPoint);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Food food))
        {
            print("Ate");
            onEat.Invoke(food);
            ConsumeFood(food);
        }
    }

    public GameObject pointPrefab;
    public Transform pointParent;
    void GainPoint(Food food)
    {
        Instantiate(pointPrefab, food.transform.position, Quaternion.identity, pointParent);
        
    }
    void ConsumeFood(Food food)
    {
        Destroy(food.gameObject);
    }
}
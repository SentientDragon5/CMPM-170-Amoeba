using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AmoebaCollider))]
public class AmoebaEater : MonoBehaviour
{
    private EdgeCollider2D col;
    private AmoebaCollider amoebaCollider;
    public UnityEvent refreshPoints;
    void Start()
    {
        col = GetComponent<EdgeCollider2D>();
        col.isTrigger = true;
        amoebaCollider = GetComponent<AmoebaCollider>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Food food))
        {
            print("Ate");
            GainPoint(food);
            refreshPoints.Invoke(); // callbacks to renderer and collider (inspector)
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

    public float dieRadius;
    public AmoebaCenter center;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.transform.position, dieRadius);
    }


    void Update()
    {
        var points = amoebaCollider.points;
        var dirty = false;
        for (int i = points.Count - 1; i >= 0; i--)
        {
            if (Vector2.Distance(center.transform.position, points[i].transform.position) > dieRadius)
            {
                PointDie(points[i]);
                dirty = true;
            }
        }
        if (dirty)
        {
            refreshPoints.Invoke();
        }
    }
    public GameObject foodPrefab;
    public Transform foodParent;
    public void PointDie(AmoebaPoint point)
    {
        var g = Instantiate(foodPrefab, point.transform.position, Quaternion.identity, foodParent);
        g.GetComponent<Rigidbody2D>().linearVelocity = point.GetComponent<Rigidbody2D>().linearVelocity;
        Destroy(point.gameObject);
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class AmoebaPoint : MonoBehaviour
{
    public static event Action OnPointRemove;

    public AmoebaCenter center;
    public List<GameObject> neighbors = new List<GameObject>();

    public float distanceCenter = 4;
    public float distanceNeighbors = 4;
    public float speedAwayFromCenter = 2f;
    public float speedTowardsEachOther = 0.2f;
    public bool isBeingDragged = false;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public float controlPointAlphaDragged = 0.8f;
    public float controlPointAlphaNormal = 0.2f;

    private AmoebaCoordinator coordinator;
    public AmoebaEater eater;
    public Food foodPrefab;
    
    
    private void Awake()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (center == null)
        {
            GameObject _ = GameObject.Find("Amoeba Center");
            center = _.GetComponent<AmoebaCenter>();
        }
        
    }

    private void Start()
    {
        if (eater == null)
        {
            eater = GameObject.Find("Amoeba Collider").GetComponent<AmoebaEater>();
        }
        GetClosestTwoPoints();

    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, center.transform.position) > coordinator.deathRadius)
        {
            RemovePoint();
        }
    }

    public void RemovePoint()
    {
        var g = Instantiate(foodPrefab, transform.position, Quaternion.identity);
        g.GetComponent<Rigidbody2D>().linearVelocity = GetComponent<Rigidbody2D>().linearVelocity;
        coordinator.controlPoints.Remove(this);
        OnPointRemove?.Invoke();
        Destroy(gameObject);
    }


    // movement stuff
    private void FixedUpdate()
    {
        // moves the point away from the center
        if (Vector2.Distance(transform.position, center.transform.position) < distanceCenter)
        {
            Vector3 direction = transform.position - center.transform.position;
            rb.AddForce(direction * speedAwayFromCenter);
        }
        //if (isBeingDragged) return;
        GetClosestTwoPoints();
        // moves this point towards other points
        foreach (GameObject point in neighbors)
        {
            if (Vector2.Distance(point.transform.position, transform.position) > distanceNeighbors)
            {
                Vector3 direction = point.transform.position - transform.position;
                rb.AddForce(direction * speedTowardsEachOther);
            }
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, isBeingDragged ? controlPointAlphaDragged : controlPointAlphaNormal);
    }

    private void GetClosestTwoPoints()
    {
        neighbors.Clear();
        GameObject[] list = GameObject.FindGameObjectsWithTag("Draggable");

        // remove self from list
        list = list.Where(p => p.name != gameObject.name).ToArray();

        // sorts by distance
        list = list.OrderBy((p) => (p.transform.position - transform.position).sqrMagnitude).ToArray();

        if (list.Length > 1)
        {
            neighbors.Add(list[0]);
            neighbors.Add(list[1]);
        }
        else
        {
            RemovePoint();
        }

    }

}

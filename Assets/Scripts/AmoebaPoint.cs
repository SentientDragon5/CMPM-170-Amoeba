using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class AmoebaPoint : MonoBehaviour
{
    public static event Action OnPointRemove;

    public List<GameObject> neighbors = new List<GameObject>();

    [Header("Distances to Maintain")]
    public float distanceCenter = 4;
    public float distanceNeighbors = 4;
    [Header("Speed")]
    public float speedAwayFromCenter = 2f;
    public float speedTowardsEachOther = 0.2f;


    [Header("Colors")]
    public float controlPointAlphaDragged = 0.8f;
    public float controlPointAlphaNormal = 0.2f;

    [HideInInspector] public bool isBeingDragged = false;
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private AmoebaCoordinator coordinator;

    public Color defaultColor;
    public Color warningColor;
    private void Awake()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        defaultColor = sr.color;
        GetClosestTwoPoints();

    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, coordinator.centerPoint.transform.position) > coordinator.DeathRadius * .80f)
        {
            sr.color = warningColor;
        }
        else 
        {
            sr.color = defaultColor;
        }

        if (Vector2.Distance(transform.position, coordinator.centerPoint.transform.position) > coordinator.DeathRadius)
        {
            RemovePoint();
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, isBeingDragged ? controlPointAlphaDragged : controlPointAlphaNormal);

    }

    public void RemovePoint()
    {
        var g = coordinator.CreateFood(transform.position);
        g.GetComponent<Rigidbody2D>().linearVelocity = GetComponent<Rigidbody2D>().linearVelocity;
        coordinator.controlPoints.Remove(this);
        OnPointRemove?.Invoke();
        Destroy(gameObject);
    }


    // movement stuff
    private void FixedUpdate()
    {
        // moves the point away from the center
        if (Vector2.Distance(transform.position, coordinator.centerPoint.transform.position) < distanceCenter)
        {
            Vector3 direction = transform.position - coordinator.centerPoint.transform.position;
            rb.AddForce(direction * speedAwayFromCenter);
        }


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

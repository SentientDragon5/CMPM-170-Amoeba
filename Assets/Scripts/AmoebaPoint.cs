using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmoebaPoint : MonoBehaviour
{
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
#if UNITY_EDITOR
    void OnValidate()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }
#endif
    
    private void Awake()
    {
        coordinator = GetComponentInParent<AmoebaCoordinator>();
        coordinator.onPointsRefreshed.AddListener(GetClosestTwoPoints);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GetClosestTwoPoints();
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

        neighbors.Add(list[0]);
        neighbors.Add(list[1]);
    }

}

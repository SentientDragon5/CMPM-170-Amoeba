using System.Collections.Generic;
using System.Drawing;
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


    private void OnValidate()
    {
        if (center == null)
        {
            GameObject _ = GameObject.Find("Amoeba Center");
            center = _.GetComponent<AmoebaCenter>();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        neighbors.Clear();
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
        GameObject[] list = GameObject.FindGameObjectsWithTag("Draggable");

        // remove self from list
        list = list.Where(p => p.name != gameObject.name).ToArray();

        // sorts by distance
        list = list.OrderBy((p) => (p.transform.position - transform.position).sqrMagnitude).ToArray();

        neighbors.Add(list[0]);
        neighbors.Add(list[1]);
    }

}

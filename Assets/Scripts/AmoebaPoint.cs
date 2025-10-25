using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AmoebaPoint : MonoBehaviour
{
    public AmoebaCenter center;
    public List<GameObject> neighbors = new List<GameObject>();

    public float distance = 4;
    public float speed = 2f;
    public bool isBeingDragged = false;

    private Rigidbody2D rb;
    private LineRenderer lr;

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
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {

        lr.positionCount = 3;
        neighbors.Clear();
        GetClosestTwoPoints();

    }

    private void Update()
    {
        lr.SetPosition(0, neighbors[0].transform.position);
        lr.SetPosition(1, transform.position);
        lr.SetPosition(2, neighbors[1].transform.position);
        //for (int i = 0; i < lr.positionCount/2; i++)
        //{
        //    lr.SetPosition(i, Vector3.Lerp(neighbors[0].transform.position, transform.position, (1/(float)lr.positionCount)*i));
        //}
        //int count = 0;
        //for (int i = lr.positionCount / 2; i < lr.positionCount; i++)
        //{
        //    lr.SetPosition(i, Vector3.Slerp(transform.position, neighbors[1].transform.position, (1 / (float)lr.positionCount) * count));
        //    count++;
        //}
    }

    // movement stuff
    private void FixedUpdate()
    {
        if (isBeingDragged) return;
        foreach (GameObject point in neighbors)
        {
            if (Vector2.Distance(point.transform.position, transform.position) > distance)
            {
                Vector3 direction = point.transform.position - transform.position;
                rb.AddForce(direction * speed);
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

    private void PrintArray(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i].name);
        }
        Debug.Log("----------------");
    }
}

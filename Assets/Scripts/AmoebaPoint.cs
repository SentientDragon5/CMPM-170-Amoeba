using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AmoebaPoint : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject center;
    public List<GameObject> neighbors = new List<GameObject>();

    public float distance = 4;

    public float speed = 2f;

    public bool isBeingDragged = false;

    private void OnValidate()
    {
        if (center == null)
        {
            center = GameObject.Find("Amoeba Center");
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (neighbors.Count != 2)
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

    private void Update()
    {
        
    }

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

    private void PrintArray(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(array[i].name);
        }
        Debug.Log("----------------");
    }
}

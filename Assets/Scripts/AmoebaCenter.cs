using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class AmoebaCenter : MonoBehaviour
{
    public int resolution = 5;
    
    public GameObject[] amoebePoints;
    private List<Transform> points = new List<Transform>();

    private void OnValidate()
    {
        amoebePoints = GameObject.FindGameObjectsWithTag("Draggable");
    }

    private void Awake()
    {
    }

    private void Start()
    {
        foreach (GameObject obj in amoebePoints)
        {
            points.Add(obj.transform);
        }
    }

    private void Update()
    {
        Vector2 centroid = CalculateCentroid();
        transform.position = centroid;
    }

    private Vector2 CalculateCentroid()
    {
        float xSum = 0f;
        float ySum = 0f;
        for (int i = 0; i < amoebePoints.Length; i++)
        {
            xSum += points[i].transform.position.x;
            ySum += points[i].transform.position.y;
        }

        return new Vector2(xSum/ amoebePoints.Length, ySum/ amoebePoints.Length);
    }

}

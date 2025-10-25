using UnityEngine;

public class AmoebaCenter : MonoBehaviour
{
    public GameObject[] points;

    private void OnValidate()
    {
        points = GameObject.FindGameObjectsWithTag("Draggable");
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
        for (int i = 0; i < points.Length; i++)
        {
            xSum += points[i].transform.position.x;
            ySum += points[i].transform.position.y;
        }

        return new Vector2(xSum/points.Length, ySum/points.Length);
    }

}

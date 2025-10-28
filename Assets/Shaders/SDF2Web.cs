using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class SDF2Web : MonoBehaviour, ISDFRenderer
{
    private const int MAX_POINTS = 128;

    private Material mat;
    private Vector4[] pointData;

    void OnEnable()
    {
        mat = GetComponent<Renderer>().sharedMaterial;
        
        if (pointData == null || pointData.Length != MAX_POINTS)
        {
            pointData = new Vector4[MAX_POINTS];
        }
    }
    public void SetPoints(Vector4[] newPoints)
    {
        if (newPoints == null)
        {
            newPoints = new Vector4[0];
        }

        int count = newPoints.Length;
        int clampedCount = count;

        if (count > MAX_POINTS)
        {
            clampedCount = MAX_POINTS;
        }

        for (int i = 0; i < clampedCount; i++)
        {
            pointData[i] = newPoints[i];
        }
        
        mat.SetVectorArray("_Points", pointData);
        mat.SetInt("_PointCount", clampedCount);
    }
}

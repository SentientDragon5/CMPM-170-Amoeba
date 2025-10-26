using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class SDF2 : MonoBehaviour
{
    // x, y = position (0-1 UV space)
    // z = radius
    // w = blend strength
    public Vector4[] points = new Vector4[1];

    private Material mat;
    private ComputeBuffer pointsBuffer;

    void OnEnable()
    {
        mat = GetComponent<Renderer>().sharedMaterial;
    }

    void Update()
    {
        UpdateBuffer();
    }

    void UpdateBuffer()
    {
        if (points == null) return;
        int count = points.Length;
        
        if (pointsBuffer == null || pointsBuffer.count != count)
        {
            if (pointsBuffer != null)
            {
                pointsBuffer.Release();
            }
            pointsBuffer = new ComputeBuffer(Mathf.Max(1, count), sizeof(float) * 4);
        }

        if (count > 0)
        {
            pointsBuffer.SetData(points);
        }

        mat.SetBuffer("_Points", pointsBuffer);
        mat.SetInt("_PointCount", count);
    }

    void OnDisable()
    {
        if (pointsBuffer != null)
        {
            pointsBuffer.Release();
            pointsBuffer = null;
        }
    }
}

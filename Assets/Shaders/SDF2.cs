using UnityEngine;

public interface ISDFRenderer
{
    void SetPoints(Vector4[] points);
}

[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class SDF2 : MonoBehaviour, ISDFRenderer
{
    // x, y = position (0-1 UV space)
    // z = radius
    // w = blend strength

    private Material mat;
    private ComputeBuffer pointsBuffer;

    void OnEnable()
    {
        mat = GetComponent<Renderer>().sharedMaterial;
    }


    public void SetPoints(Vector4[] newPoints)
    {
        if (newPoints == null)
        {
            newPoints = new Vector4[0];
        }
        
        int count = newPoints.Length;
        
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
            pointsBuffer.SetData(newPoints);
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
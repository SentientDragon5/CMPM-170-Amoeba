using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class AmoebaSDFRenderer : MonoBehaviour
{
    
    private AmoebaCoordinator coordinator;
    // x, y = position (0-1 UV space)
    // z = radius
    // w = blend strength
    public AmoebaSDFPoint[] amoebaPoints => coordinator.sdfPoints.ToArray();

    public float margin = 2f;
    private Vector4[] points;

    private SpriteRenderer spriteRenderer;
    public ISDFRenderer SDFRenderer;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SDFRenderer = GetComponent<ISDFRenderer>();
        coordinator = GetComponentInParent<AmoebaCoordinator>();
    }
    void OnValidate()
    {
        OnEnable();
    }


    void Update()
    {
        UpdateSpriteSizeAndPoints();
        SDFRenderer.SetPoints(points);
    }

    void UpdateSpriteSizeAndPoints()
    {
        if (amoebaPoints == null || amoebaPoints.Length == 0)
        {
            if (points == null || points.Length != 0)
                points = new Vector4[0];
            return;
        }

        AmoebaSDFPoint firstPoint = amoebaPoints[0];
        Vector2 pos = firstPoint.transform.position;
        float r = firstPoint.radius;
        Vector2 min = new Vector2(pos.x - r, pos.y - r);
        Vector2 max = new Vector2(pos.x + r, pos.y + r);

        for (int i = 1; i < amoebaPoints.Length; i++)
        {
            AmoebaSDFPoint point = amoebaPoints[i];
            pos = point.transform.position;
            r = point.radius;

            min.x = Mathf.Min(min.x, pos.x - r);
            min.y = Mathf.Min(min.y, pos.y - r);
            max.x = Mathf.Max(max.x, pos.x + r);
            max.y = Mathf.Max(max.y, pos.y + r);
        }

        min.x -= margin;
        min.y -= margin;
        max.x += margin;
        max.y += margin;

        Vector2 center = (min + max) / 2f;
        Vector2 size = max - min;

        float maxDimension = Mathf.Max(size.x, size.y);
        size = new Vector2(maxDimension, maxDimension);

        if (size.x <= 0)
        {
            size.x = 0.1f;
            size.y = 0.1f;
        }

        transform.position = new Vector3(center.x, center.y, transform.position.z);
        transform.localScale = new Vector3(size.x, size.y, 1f);

        min = center - size / 2f;

        Rect worldRect = new Rect(min, size);

        if (points == null || points.Length != amoebaPoints.Length)
        {
            points = new Vector4[amoebaPoints.Length];
        }

        for (int i = 0; i < amoebaPoints.Length; i++)
        {
            points[i] = amoebaPoints[i].PackData(worldRect);
        }
    }

    
    void OnDrawGizmos()
    {   
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}

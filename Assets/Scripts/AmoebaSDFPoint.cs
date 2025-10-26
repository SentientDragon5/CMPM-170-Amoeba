using UnityEngine;

public class AmoebaSDFPoint : MonoBehaviour
{
    [Range(0f,4f)]
    public float radius = 1f;
    [Range(0f,1f)]
    public float blend = 2f;

    public Vector4 PackData(Rect worldRect)
    {
        Vector2 uv = new Vector2(
            Mathf.InverseLerp(worldRect.xMin, worldRect.xMax, transform.position.x),
            Mathf.InverseLerp(worldRect.yMin, worldRect.yMax, transform.position.y)
            );
        float uvRadius = (worldRect.height > 0) ? radius / worldRect.height : 0;

        return new Vector4(
            uv.x, uv.y,
            uvRadius,
            blend
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius + blend*2);
    }
}
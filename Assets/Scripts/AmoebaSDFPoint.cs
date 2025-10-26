using UnityEngine;

public class AmoebaSDFPoint : MonoBehaviour
{
    public float radius = 1f;
    public float blend = 2f;

    public Vector4 PackData()
    {
        Vector2 uv = new Vector2(
            transform.position.x, // transform from world space to uv space
            transform.position.y
            );

        return new Vector4(
            uv.x,uv.y,
            radius,
            blend
        );
    }
}
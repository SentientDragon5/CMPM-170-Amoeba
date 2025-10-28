using UnityEngine;

[ExecuteAlways]
public class SDFDirectRenderer : MonoBehaviour
{

    public Vector4[] points = new Vector4[1];


    private Material mat;

    public ISDFRenderer r;

    void OnEnable()
    {
        r = GetComponent<ISDFRenderer>();
        mat = GetComponent<Renderer>().sharedMaterial;
    }
    void Update()
    {
        if (mat == null)
        {
            OnEnable();
            if (mat == null) return;
        }

        r.SetPoints(points);
    }
}
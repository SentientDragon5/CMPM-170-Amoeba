using UnityEngine;

public class FoodRenderer : MonoBehaviour
{
    SpriteRenderer sdf_rend;
    public Material sdf_mat;
    SpriteRenderer nucleus_rend;

    void Start()
    {
        sdf_rend = GetComponent<SpriteRenderer>();
        sdf_mat = Instantiate(sdf_rend.sharedMaterial);
        sdf_rend.material = sdf_mat;
        float hueOffset = UnityEngine.Random.value;
        
        nucleus_rend = transform.GetChild(0).GetComponent<SpriteRenderer>();

        Color color;
        float h, s, v, a;


        color = sdf_mat.GetColor("_CircleColor");
        a = color.a;
        Color.RGBToHSV(color, out h, out s, out v);
        color = Color.HSVToRGB(h += hueOffset, s, v);
        color.a = a;
        sdf_mat.SetColor("_CircleColor", color);

        color = sdf_mat.GetColor("_OutlineColor");
        a = color.a;
        Color.RGBToHSV(color, out h, out s, out v);
        color = Color.HSVToRGB(h += hueOffset, s, v);
        color.a = a;
        sdf_mat.SetColor("_OutlineColor", color);

        Color.RGBToHSV(nucleus_rend.color, out h, out s, out v);
        nucleus_rend.color = Color.HSVToRGB(h += hueOffset, s, v);

        
        color = nucleus_rend.color;
        a = color.a;
        Color.RGBToHSV(color, out h, out s, out v);
        color = Color.HSVToRGB(h += hueOffset, s, v);
        color.a = a;
        nucleus_rend.color = color;
    }
    
}

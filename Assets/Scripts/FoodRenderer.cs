using System.Collections.Generic;
using UnityEngine;

public class FoodRenderer : MonoBehaviour
{
    SpriteRenderer sdf_rend;
    public List<Material> materials;
    public List<Color> nucleusColors = new();

    SpriteRenderer nucleus_rend;
    public int presetIndex;

    [ContextMenu("Add base color")]
    void AddBaseColor()
    {
        nucleusColors.Add(new Color(0.8117647f, 0.5150502f, 0.4705882f, 0.5529412f));
    }
    [ContextMenu("Randomize Preset")]
    void Start()
    {
        sdf_rend = GetComponent<SpriteRenderer>();
        nucleus_rend = transform.GetChild(0).GetComponent<SpriteRenderer>();
        presetIndex = Mathf.FloorToInt(UnityEngine.Random.value * materials.Count);

        sdf_rend.material = materials[presetIndex];
        nucleus_rend.color = nucleusColors[presetIndex];
    }
    
}

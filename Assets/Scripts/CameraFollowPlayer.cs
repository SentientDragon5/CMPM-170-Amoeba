using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    Camera cam;
    public GameObject target;
    public AmoebaSDFRenderer rend;

    public float zoomMultiplier = 1;
    public float multiplierIncrease = 0.15f;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        cam.orthographicSize = rend.transform.lossyScale.x * zoomMultiplier;
        transform.position = new Vector3(target.transform.position.x , target.transform.position.y, -10f);
    }

    public void IncreaseZoomMultiplier()
    {
        zoomMultiplier += multiplierIncrease;
    }
}

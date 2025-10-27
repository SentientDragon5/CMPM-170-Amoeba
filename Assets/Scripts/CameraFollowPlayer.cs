using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    Camera cam;
    public GameObject target;
    public AmoebaSDFRenderer rend;

    public float zoomMultiplier = 1;
    public float multiplierIncrease = 0.15f;
    public float smoothing = 1;
    public float zoomSmoothing = 1;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, rend.transform.lossyScale.x * zoomMultiplier, zoomSmoothing * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x , target.transform.position.y, -10f),smoothing * Time.deltaTime);
    }

    public void IncreaseZoomMultiplier()
    {
        zoomMultiplier += multiplierIncrease;
    }
}

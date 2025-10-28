using System.Collections;
using System.Timers;
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

    private void OnEnable()
    {
        AmoebaEater.OnPointAdd += IncreaseZoomMultiplier;
        AmoebaPoint.OnPointRemove += DecreaseZoomMultiplier;
    }

    private void OnDisable()
    {
        AmoebaEater.OnPointAdd -= IncreaseZoomMultiplier;
        AmoebaPoint.OnPointRemove -= DecreaseZoomMultiplier;
    }


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
        StartCoroutine(LerpTo(zoomMultiplier, zoomMultiplier + multiplierIncrease, 1f));
    }

    public void DecreaseZoomMultiplier()
    {
        StartCoroutine(LerpTo(zoomMultiplier, zoomMultiplier - multiplierIncrease, 1f));
    }

    private IEnumerator LerpTo(float start, float end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            zoomMultiplier = Mathf.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
        }
        zoomMultiplier = end;
        yield return null;
    }
}

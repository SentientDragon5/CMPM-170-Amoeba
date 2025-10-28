using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    public float controlsDelay = 3f;

    float draggingTime = 0;
    public float draggingThreshold = 1f;

    public Dragging dragging;

    public TextMeshProUGUI controlsUI;
    void Start()
    {
        controlsUI.alpha = 0;
        StartCoroutine(CheckShowControls());
    }

    void Update()
    {
        float delta = Time.deltaTime;
        if (dragging.draggedPoints.Count > 0)
        {
            draggingTime += delta;
        }
    }

    IEnumerator CheckShowControls()
    {
        Debug.Log("Controls waiting for " + controlsDelay);
        yield return new WaitForSeconds(controlsDelay);
        Debug.Log("" + draggingTime + " < " + draggingThreshold);
        
        float start = Time.time;
        float duration = 0.4f;
        if (draggingTime < draggingThreshold)
        {
            while(Time.time - start < duration)
            {
                controlsUI.alpha = Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0, duration, Time.time));
                yield return new WaitForFixedUpdate();
            }
        }
        while (draggingTime < draggingThreshold)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        start = Time.time;
        duration = 0.2f;
        while(Time.time - start < duration)
        {
            controlsUI.alpha = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(0, duration, Time.time));
            yield return new WaitForFixedUpdate();
        }
    }
}
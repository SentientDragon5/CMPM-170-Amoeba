using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class ScreenshotTaker : MonoBehaviour
{
    public InputAction screenshotAction;

    public string filenamePrefix = "amoeba-screenshot";
    public TextMeshProUGUI uiComponent;
    public string uiMessagePrefix = "Screenshot saved to Persistant Data Path";

    private void OnEnable()
    {
        if (screenshotAction != null)
        {
            screenshotAction.Enable();
            screenshotAction.performed += OnScreenshotPerformed;
        }
    }

    private void OnDisable()
    {
        if (screenshotAction != null)
        {
            screenshotAction.performed -= OnScreenshotPerformed;
            screenshotAction.Disable();
        }
    }

    [ContextMenu("Take Screenshot")]
    public void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshotCoroutine());
    }

    private void OnScreenshotPerformed(InputAction.CallbackContext context)
    {
        TakeScreenshot();
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        Debug.Log("Starting Screenshot");
        yield return new WaitForEndOfFrame();

        string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"{filenamePrefix}_{timeStamp}.png";

        string filePath;

#if UNITY_EDITOR
        string directoryPath = Path.Combine(Application.dataPath, "..", "Recordings");
        Directory.CreateDirectory(directoryPath);
        filePath = Path.Combine(directoryPath, fileName);
#else
        filePath = Path.Combine(Application.persistentDataPath, fileName);
#endif

        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        byte[] imageBytes = screenTexture.EncodeToPNG();

#if UNITY_EDITOR
        Destroy(screenTexture);
#else
            DestroyImmediate(screenTexture);
#endif

        try
        {
            File.WriteAllBytes(filePath, imageBytes);
            Debug.Log($"Screenshot saved to: {filePath}");
            StartCoroutine(ShowPathUI(filePath));
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save screenshot: {e.Message}");
        }
    }

    void Start()
    {
        if (uiComponent != null)
            uiComponent.alpha = 0;
    }
    IEnumerator ShowPathUI(string path)
    {
        if (uiComponent != null)
        {
            uiComponent.text = "<noparse>"+uiMessagePrefix + path.Replace('\\','/') +"</noparse>";
            Debug.Log(uiComponent.text + " " + uiMessagePrefix + path);
            float start = Time.time;
            float duration = 0.2f;
            while(Time.time - start < duration)
            {
                uiComponent.alpha = Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0, duration, Time.time));
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.4f);
            start = Time.time;
            duration = 1f;
            while (Time.time - start < duration)
            {
                uiComponent.alpha = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(0, duration, Time.time));
                yield return new WaitForFixedUpdate();
            }
            uiComponent.alpha = 0f;
        }
    }
}




using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private InputAction mouseClick; // i didn't even know you could do this
    public float mouseDragSpeed = 10f;

    private Camera _mainCam;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable()
    {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(_mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (hit.collider != null && (hit.collider.gameObject.CompareTag("Draggable")))
        {
            StartCoroutine(DragUpdate(hit.collider.gameObject));
        }
    }

    private IEnumerator DragUpdate(GameObject selectedObject)
    {
        float initialDistance = Vector3.Distance(selectedObject.transform.position, _mainCam.transform.position);
        Rigidbody2D rb = selectedObject.GetComponent<Rigidbody2D>();

        // updates the velocity of the object based on mouse position while it is held every physics update
        while (mouseClick.ReadValue<float>() != 0f)
        {
            Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 direction = ray.GetPoint(initialDistance) - selectedObject.transform.position;
            rb.linearVelocity = direction * mouseDragSpeed;
            yield return waitForFixedUpdate;
        }
    }
}

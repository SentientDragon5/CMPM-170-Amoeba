using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dragging : MonoBehaviour
{
    [SerializeField] private InputAction mouseClick; // i didn't even know you could do this
    [SerializeField] private InputAction mouseRelease;
    public float mouseDragSpeed = 10f;

    public float dragRadius = 1f;

    private Camera _mainCam;
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public List<AmoebaPoint> draggedPoints;

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
        Vector2 mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(mousePos, dragRadius);

        foreach (Collider2D c in overlaps)
        {
            if (c != null && c.gameObject.CompareTag("Draggable"))
            {
                var attenuation = Vector2.Distance(c.transform.position, mousePos);
                var draggedPoint = c.gameObject.GetComponent<AmoebaPoint>();
                draggedPoint.isBeingDragged = true;
                draggedPoints.Add(draggedPoint);
                StartCoroutine(DragUpdate(c.gameObject));
            }
        }
    }

    private void Update()
    {
        for (int i = draggedPoints.Count - 1; i >= 0; i--)
        {
            AmoebaPoint draggedPoint = draggedPoints[i];

            if (draggedPoint != null && mouseClick.ReadValue<float>() == 0f)
            {
                draggedPoint.isBeingDragged = false;
                draggedPoints.RemoveAt(i);
            }
        }
    }

    private IEnumerator DragUpdate(GameObject selectedObject)
    {
        float initialDistance = Vector3.Distance(selectedObject.transform.position, _mainCam.transform.position);
        Rigidbody2D rb = selectedObject.GetComponent<Rigidbody2D>();

        // updates the velocity of the object based on mouse position while it is held every physics update
        while (mouseClick.ReadValue<float>() != 0f)
        {
            if (selectedObject != null)
            {
                Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
                Vector3 direction = ray.GetPoint(initialDistance) - selectedObject.transform.position;
                rb.linearVelocity = direction * mouseDragSpeed;
            }
            yield return waitForFixedUpdate;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, dragRadius);

        if (_mainCam)
        {
            Vector2 mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Gizmos.DrawWireSphere(mousePos, dragRadius);
        }
    }
}

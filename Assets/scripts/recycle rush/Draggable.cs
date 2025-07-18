using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPos;
    private Canvas canvas;
    private RectTransform panelRectTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        canvas = GameObject.FindObjectOfType<Canvas>();
        panelRectTransform = GetComponentInParent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert screen position to world position directly for smoother tracking
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            panelRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out worldPoint
        );

        // Update position to follow cursor smoothly
        transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rb.isKinematic = false;
    }
}
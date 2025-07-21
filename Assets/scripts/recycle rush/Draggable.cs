using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

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
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            panelRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out worldPoint
        );

        transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rb.isKinematic = false;
    }

    public void ReturnToStart()
    {
        StartCoroutine(BounceBack());
    }

    private IEnumerator BounceBack()
    {
        float duration = 0.25f;
        float elapsed = 0f;
        Vector3 originalPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(originalPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
    }
}

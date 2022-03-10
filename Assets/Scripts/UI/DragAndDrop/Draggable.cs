using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Event method that handles behaviours when a draggable object begins to be dragged.
    /// Updates the selected object's parent and visual opacity. 
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.SetParent(_canvas.transform);
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.75f;
    }

    /// <summary>
    /// Event method that handles behaviours when a draggable object continues to be dragged.
    /// Ensures the dragged object remains centered on the mouse cursor.
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }

    /// <summary>
    /// Event method that handles behaviours when a draggable object is no longer being dragged.
    /// Restores the object's original opacity and decides if it has been correctly dropped.
    /// If not, the object is destroyed.
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;

        // Destroy the dragged item if its parent is the UI canvas
        if (eventData.pointerDrag.transform.parent == _canvas.transform)
        {
            Destroy(eventData.pointerDrag);
        }
    }
}

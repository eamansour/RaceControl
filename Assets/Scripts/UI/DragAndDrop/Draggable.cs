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

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.SetParent(_canvas.transform);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
    
        // Destroy the dragged item if its parent is the UI canvas
        if (eventData.pointerDrag.transform.parent == _canvas.transform)
        {
            Destroy(eventData.pointerDrag);
        }
    }
}

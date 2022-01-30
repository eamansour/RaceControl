using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSpawn : MonoBehaviour, IDragHandler, IBeginDragHandler
{

    [SerializeField]
    private GameObject _objectToSpawn;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    // Dragging a "spawner" object will spawn a draggable object and use the object's drag-and-drop behaviour 
    public void OnBeginDrag(PointerEventData eventData)
    {
        Draggable spawned = Instantiate(_objectToSpawn, _canvas.transform).GetComponent<Draggable>();
        eventData.pointerDrag = spawned.gameObject;
        spawned.OnBeginDrag(eventData);
    }

    // OnDrag must be implemented for OnBeginDrag to be called
    public void OnDrag(PointerEventData eventData) {}
}

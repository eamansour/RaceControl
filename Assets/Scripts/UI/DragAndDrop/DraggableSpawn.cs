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

    /// <summary>
    /// Spawns a draggable object and switch to the draggable object's drag-and-drop behaviour
    /// when the user drags their cursor from the spawner.
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Draggable spawned = Instantiate(_objectToSpawn, _canvas.transform).GetComponent<Draggable>();
        eventData.pointerDrag = spawned.gameObject;
        spawned.OnBeginDrag(eventData);
    }

    // OnDrag must be implemented for OnBeginDrag to be called
    public void OnDrag(PointerEventData eventData) {}
}

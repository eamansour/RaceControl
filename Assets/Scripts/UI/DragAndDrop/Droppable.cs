using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Droppable : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private RectTransform _targetRect;

    [SerializeField]
    private bool _reserveFirstIndex = false;

    // Event method that fires when something is dropped onto the current object
    public void OnDrop(PointerEventData eventData)
    {
        // Ensure the dropped object exists and is draggable
        if (eventData.pointerDrag && eventData.pointerDrag.GetComponent<Draggable>())
        {
            eventData.pointerDrag.transform.SetParent(_targetRect);
            float droppedPosY = eventData.pointerDrag.transform.position.y;

            // Re-order the existing program statements to account for the newly dropped statement
            for (int i = 0; i < _targetRect.childCount; i++)
            {
                Transform sibling = _targetRect.GetChild(i);
                if (sibling.gameObject != eventData.pointerDrag)
                {
                    if (droppedPosY >= sibling.position.y)
                    {
                        if (i == 0 && _reserveFirstIndex) return;

                        eventData.pointerDrag.transform.SetSiblingIndex(i);
                        break;
                    }
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_targetRect);
        }
    }
}

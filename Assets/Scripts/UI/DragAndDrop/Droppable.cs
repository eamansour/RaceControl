using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Droppable : MonoBehaviour, IDropHandler
{
    public static Action OnLayoutUpdated;

    [SerializeField]
    private RectTransform _targetRect;

    [SerializeField]
    private bool _reserveFirstIndex = false;

    private void Start()
    {
        OnLayoutUpdated += RefreshLayout;
    }

    private void OnDestroy()
    {
        OnLayoutUpdated -= RefreshLayout;
    }

    /// <summary>
    /// Invokes the OnLayoutUpdated event.
    /// </summary>
    public static void UpdateLayout()
    {
        if (OnLayoutUpdated != null)
        {
            OnLayoutUpdated();
        }        
    }

    /// <summary>
    /// Event method that handles behaviours when another object has been dropped on this object.
    /// Sets a dropped object to be a child of a target transform, and sorts it into an appropriate child index.
    /// </summary>
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
                        if (i == 0 && _reserveFirstIndex)
                        {
                            eventData.pointerDrag.transform.SetSiblingIndex(1);
                            break;
                        }

                        eventData.pointerDrag.transform.SetSiblingIndex(i);
                        break;
                    }
                }
            }
            UpdateLayout();
        }
    }

    /// <summary>
    /// Rebuilds the target's layout group.
    /// </summary>
    private void RefreshLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_targetRect);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        if (true)
        {

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}

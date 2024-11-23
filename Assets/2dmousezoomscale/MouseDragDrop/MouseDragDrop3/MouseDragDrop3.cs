using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragDrop3 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector3 startDragPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - startDragPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
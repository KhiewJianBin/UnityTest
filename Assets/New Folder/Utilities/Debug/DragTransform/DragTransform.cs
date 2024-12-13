using System.Collections;
using UnityEngine;
/// From Unity Procedural sample
/// <summary>
/// 
/// Info:
/// Script that highlights object on mouse over and allows draging object on screen
/// 
/// How To Use:
/// Attach this script to GameObject.
/// Click and Drag
/// </summary>
public class DragTransform : MonoBehaviour
{
    public Color MouseOverColor = Color.blue;
    bool isdragging = false;
    Color originalColor;

    void Start()
    {
        originalColor = GetComponent<Renderer>().sharedMaterial.color;
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = MouseOverColor;
    }

    void OnMouseExit()
    {
        if (!isdragging)
            GetComponent<Renderer>().material.color = originalColor;
    }

    public IEnumerator OnMouseDown()
    {
        Vector3 screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position -
                         Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                             screenSpace.z));

        while (Input.GetMouseButton(0))
        {
            isdragging = true;
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
            transform.position = curPosition;
            yield return null;
        }
    }

    public void OnMouseUp()
    {
        isdragging = false;
    }
}
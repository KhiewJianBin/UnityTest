
// Simple Script to have click drag of 3d game objects in scene,
// This method moves the object left/right/up/down based on the objects perspective to the camera
// Usage: Add this script to 3d gameobject that has a collider component
using UnityEngine;

public class MouseDragDrop : MonoBehaviour
{
    bool dragging = false;
    Vector3 startDragPos;

    void Update()
    {
        if(dragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - startDragPos);
        }
    }
    void OnMouseDown()
    {
        startDragPos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        dragging = true;
    }
    void OnMouseUp()
    {
        dragging = false;    
    }
}
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] RectTransform scrollarea;

    // Zoom speed factor
    public float zoomSpeed = 0.1f;

    // Minimum and maximum scale limits
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    void Update()
    {
        //if(Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        //{
        //    var scale = rt.localScale.x;

        //    //mousePosition contains position of mouse inside scaled area in percentages
        //    var mousePosition = (Vector2)(Input.mousePosition - rt.position) - rt.rect.position * scale;
        //    mousePosition.x /= rt.rect.width * scale;
        //    mousePosition.y /= rt.rect.height * scale;

        //    var contentSize = scrollarea.content.rect;
        //    var shiftX = -scaleDelta * contentSize.width * (mousePosition.x - 0.5f);
        //    var shiftY = -scaleDelta * contentSize.height * (mousePosition.y - 0.5f);
        //    var currPos = scrollarea.content.localPosition;
        //    scrollarea.content.localPosition = new Vector3(currPos.x + shiftX, currPos.y + shiftY, currPos.z);
        //}

        Zoom3();


        Pan();
    }

    private Vector3 previousMousePosition;
    void Zoom3()
    {
        // Get scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // If there is scroll input, zoom in or out
        if (scrollInput != 0)
        {
            // Calculate the new scale
            Vector3 scaleChange = new Vector3(scrollInput, scrollInput, scrollInput) * zoomSpeed;
            Vector3 newScale = canvasRectTransform.localScale + scaleChange;

            // Clamp the scale between min and max limits
            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
                Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
                Mathf.Clamp(newScale.z, minScale.z, maxScale.z)
            );

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, Camera.main, out localPoint);

            // Apply the new scale to the canvas
            canvasRectTransform.localScale = newScale;

            // Move the UI back to the mouse position after zooming
            Vector3 offset = new Vector3(localPoint.x, localPoint.y, 0) - canvasRectTransform.localPosition;
            canvasRectTransform.localPosition = new Vector3(localPoint.x, localPoint.y, 0) - offset;

            print(localPoint);
            print(canvasRectTransform.localPosition);
            print(offset);


        }
    }

    void Zoom2()
    {
        // Get scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // If there's scroll input, zoom in or out
        if (scrollInput != 0)
        {
            // Change scale based on scroll input
            Vector3 newScale = canvasRectTransform.localScale + new Vector3(scrollInput, scrollInput, scrollInput) * zoomSpeed;

            // Clamp the scale between the min and max limits
            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
                Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
                Mathf.Clamp(newScale.z, minScale.z, maxScale.z)
            );

            // Apply the new scale to the UI element
            canvasRectTransform.localScale = newScale;
        }
    }


    void ZoomScale(RectTransform rt)
    {

    }


    void Zoom()
    {
        //https://stackoverflow.com/questions/33433872/zoom-to-mouse-position-like-3ds-max-in-unity3d?rq=3
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            RaycastHit hit;
            Ray ray = this.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Vector3 desiredPosition;

            if (Physics.Raycast(ray, out hit))
            {
                desiredPosition = hit.point;
            }
            else
            {
                desiredPosition = transform.position;
            }
            float distance = Vector3.Distance(desiredPosition, transform.position);
            Vector3 direction = Vector3.Normalize(desiredPosition - transform.position) * (distance * Input.GetAxis("Mouse ScrollWheel"));

            transform.position += direction;
        }
    }


    public float panSpeed = 0.1f;
    Vector3 lastMousePosition;
    void Pan()
    {
        // Check if the middle mouse button is pressed
        if (Input.GetMouseButton(2))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            Vector3 movement = new Vector3(mouseDelta.x, mouseDelta.y, 0) * panSpeed;

            canvasRectTransform.position += movement;
            lastMousePosition = Input.mousePosition;
        }
        else
        {
            lastMousePosition = Input.mousePosition;
        }
    }
}

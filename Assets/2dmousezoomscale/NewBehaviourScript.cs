using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] RectTransform canvasRectTransform;

    public float zoomSpeed = 0.1f;

    void Update()
    {
        Zoom();

        Pan();
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            Vector3 scaleChange = new Vector3(scrollInput, scrollInput, scrollInput) * zoomSpeed;
            Vector3 newScale = canvasRectTransform.localScale + scaleChange;

            var oldPivot = canvasRectTransform.pivot;

            ChangePivotAndHold(canvasRectTransform, new Vector2(0, 0));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, null, out var localPointBeforeScale);

            ChangePivotAndHold(canvasRectTransform, localPointBeforeScale / canvasRectTransform.rect.size);
            canvasRectTransform.localScale = newScale;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, null, out var localPointAfterScale);

            ChangePivotAndHold(canvasRectTransform, oldPivot);

            //Vector3 offset = localPointAfterScale - localPointBeforeScale;
            //canvasRectTransform.localPosition += offset;
        }
    }

    void Zoom2()
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




    /// <summary>
    /// Changes Pivot of UI Object, and hold object in place (does not having the object moving, position values changes to achive this)
    /// 
    /// Context:
    /// In Unity Inspector, while changing pivot, it also automatically changes the position accordingly so that the object stays in place while pivot changes
    /// In Unity Inspector Debug mode, changing the pivot does not change the position, hence the object will move.
    /// Changing Pivot in script is the same as Unity Inspector Debug Mode.
    /// </summary>
    void ChangePivotAndHold(RectTransform rt, Vector2 newPivot)
    {
        var oldPivot = rt.pivot;

        rt.pivot = newPivot;

        var pivotDiff = newPivot - oldPivot;
        var offset = pivotDiff * (rt.rect.size * rt.localScale);

        rt.anchoredPosition += offset;
    }
}

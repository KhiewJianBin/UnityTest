using UnityEngine;
using UnityEngine.UI;

public class UIZoomController : MonoBehaviour
{
    public Camera mainCamera;
    public CanvasScaler canvasScaler;
    public float minZoomLevel = 0.1f;
    public float maxZoomLevel = 10f;
    public float zoomSensitivity = 5f;

    private float currentZoomLevel;
    private Vector2 zoomCenter;

    void Start()
    {
        // Initialize zoom level
        currentZoomLevel = Mathf.Lerp(minZoomLevel, maxZoomLevel, canvasScaler.uiScaleInPixels / canvasScaler.referenceResolution.y);
        zoomCenter = Input.mousePosition;
    }

    void Update()
    {
        // Check for mouse wheel input
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            HandleZoom();
        }
    }

    void HandleZoom()
    {
        // Calculate delta based on mouse wheel sensitivity
        float delta = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;

        // Calculate new zoom level
        currentZoomLevel -= delta;

        // Clamp zoom level between min and max
        currentZoomLevel = Mathf.Clamp(currentZoomLevel, minZoomLevel, maxZoomLevel);

        // Update UI scale
        canvasScaler.uiScale = currentZoomLevel;

        // Update zoom center
        zoomCenter = Input.mousePosition;
    }

    void LateUpdate()
    {
        // Center zoom around mouse position
        Vector2 mousePositionInCanvas = mainCamera.WorldToScreenPoint(zoomCenter);
        mousePositionInCanvas.y = Screen.height - mousePositionInCanvas.y;

        RectTransform canvasTransform = canvasScaler.GetComponent<RectTransform>();
        Vector2 canvasSize = canvasTransform.sizeDelta;
        Vector2 halfCanvasSize = canvasSize / 2;

        float offsetX = (mousePositionInCanvas.x - halfCanvasSize.x) / canvasSize.x;
        float offsetY = (mousePositionInCanvas.y - halfCanvasSize.y) / canvasSize.y;

        canvasTransform.anchoredPosition = new Vector2(offsetX, offsetY);
    }
}
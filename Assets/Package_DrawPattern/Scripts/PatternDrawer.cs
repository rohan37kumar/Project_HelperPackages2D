using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PatternDrawer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Canvas targetCanvas; // Canvas which is parent

    [SerializeField] private float lineWidth = 2f;
    //[SerializeField] private Color lineColor = Color.white;

    private List<GameObject> selectedNodes = new List<GameObject>();
    private List<Vector3> linePositions = new List<Vector3>();
    private bool isDragging = false;
    private Vector3 currentPointerPosition;
    private RectTransform canvasRectTransform;

    [Header("Output Text")]
    [SerializeField] private Text outputText;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (targetCanvas == null)
            targetCanvas = GetComponentInParent<Canvas>();

        canvasRectTransform = targetCanvas.GetComponent<RectTransform>();
        SetupLineRenderer();
    }

    private void SetupLineRenderer()
    {
        lineRenderer.startWidth = lineWidth / targetCanvas.scaleFactor;
        lineRenderer.endWidth = lineWidth / targetCanvas.scaleFactor;
        //lineRenderer.material.color = lineColor;
        lineRenderer.positionCount = 0;

        // Set the line renderer to use world space
        lineRenderer.useWorldSpace = true;

        // Important: Set the line renderer material to be UI compatible
        lineRenderer.material = new Material(Shader.Find("UI/Default"));
        lineRenderer.sortingOrder = 1; // Ensure line appears above UI elements

        // Set the line renderer to render in 2D mode
        lineRenderer.generateLightingData = false;
        lineRenderer.allowOcclusionWhenDynamic = false;
    }

    private Vector3 GetWorldPositionFromPointer(Vector2 screenPosition)
    {
        // Convert screen position to Canvas space
        Vector2 pointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPosition,
            targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out pointerPosition
        );

        // Convert to world space
        Vector3 worldPosition = canvasRectTransform.TransformPoint(pointerPosition);

        // Set a fixed Z position slightly in front of the canvas
        worldPosition.z = transform.position.z - 0.1f;

        return worldPosition;
    }


    //Intentionally used Update and Input.touchCount
    //will use the events in Assembler project to call the HandleTouchInput() function
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        //Gets the position where user touches
        Touch touch = Input.GetTouch(0);
        currentPointerPosition = GetWorldPositionFromPointer(touch.position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                HandleInputBegan(touch.position);
                break;

            case TouchPhase.Moved:
                HandleInputMoved(touch.position);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                HandleInputEnded(touch.position);
                break;
        }
    }

    private void HandleMouseInput()
    {
        Vector2 mousePosition = Input.mousePosition;
        currentPointerPosition = GetWorldPositionFromPointer(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            HandleInputBegan(mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            HandleInputMoved(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleInputEnded(mousePosition);
        }
    }

    private void HandleInputBegan(Vector2 position)
    {
        GameObject touchedNode = GetNodeAtPosition(position);
        if (touchedNode != null)
        {
            isDragging = true;
            selectedNodes.Clear();
            linePositions.Clear();
            AddNodeToPattern(touchedNode);
        }
    }

    private void HandleInputMoved(Vector2 position)
    {
        if (!isDragging) return;
        UpdateLineRenderer();

        GameObject touchedNode = GetNodeAtPosition(position);
        if (touchedNode != null && !selectedNodes.Contains(touchedNode))
        {
            AddNodeToPattern(touchedNode);
        }
    }

    private void HandleInputEnded(Vector2 position)
    {
        if (!isDragging) return;

        isDragging = false;
        ValidatePattern();

        lineRenderer.positionCount = 0;
        selectedNodes.Clear();
        linePositions.Clear();
    }

    private GameObject GetNodeAtPosition(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("PatternNode"))
            {
                return result.gameObject;
            }
        }

        return null;
    }

    private void AddNodeToPattern(GameObject node)
    {
        selectedNodes.Add(node);

        // Get the world position of the UI element
        RectTransform rectTransform = node.GetComponent<RectTransform>();
        Vector3 nodePosition;

        // Convert UI position to world position
        if (targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
            nodePosition = GetWorldPositionFromPointer(screenPoint);
        }
        else
        {
            nodePosition = rectTransform.position;
            nodePosition.z = transform.position.z - 0.1f; // Keep consistent Z position
        }

        linePositions.Add(nodePosition);
        UpdateLineRenderer();

    }

    private void UpdateLineRenderer()
    {
        int totalPoints = linePositions.Count + (isDragging ? 1 : 0);
        lineRenderer.positionCount = totalPoints;

        for (int i = 0; i < linePositions.Count; i++)
        {
            lineRenderer.SetPosition(i, linePositions[i]);
        }

        if (isDragging)
        {
            lineRenderer.SetPosition(totalPoints - 1, currentPointerPosition);
        }
    }

    private void ValidatePattern()
    {
        List<int> pattern = new List<int>();
        string patternValue = "";
        foreach (GameObject node in selectedNodes)
        {
            pattern.Add(node.GetComponent<PatternNode>().nodeID);
            patternValue = patternValue + " " + node.GetComponent<PatternNode>().nodeValue;
        }

        //TODO: Verification Logic Here...
        Debug.Log($"Pattern completed: {string.Join("-", pattern)}");
        outputText.text = patternValue;
        Debug.Log(patternValue);
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Text> outputText;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string requiredTag = "Node";

    private List<GameObject> selectedNodes = new List<GameObject>();
    private List<Vector3> linePositions = new List<Vector3>();
    private bool isDragging = false;
    private Vector3 currentMousePos;

    // C# action events
    //public event Action OnLineStarted;
    //public event Action<string> OnLineEnded;
    //public event Action<Vector3[]> OnLineUpdated;

    public bool IsSolved { get; private set; }

    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                Debug.LogError("LineRenderer is missing. Please attach one to the GameObject.");
                enabled = false;
                return;
            }
        }
        ConfigureLineRenderer();
        selectedNodes.Clear();
        linePositions.Clear();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            HandleMouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseDown()
    {
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null)
        {
            StartDragging(node);
            LineActions.TriggerLineStarted();
        }
    }

    private void HandleMouseDrag()
    {
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null && !selectedNodes.Contains(node))
        {
            AddNodeToLine(node);
        }
        else
        {
            UpdateMousePosition(Input.mousePosition);
        }
    }

    private void HandleMouseUp()
    {
        isDragging = false;

        // Check if fewer than two nodes are selected
        if (selectedNodes.Count < 2)
        {
            Debug.Log("Only one node selected. Ignoring line.");
            ResetLine();
            return;
        }

        ValidatePattern();
        ResetLine();
        LineActions.TriggerLineEnded(string.Join(" ", outputText.Select(t => t.text)));
    }

    private void StartDragging(GameObject node)
    {
        isDragging = true;
        AddNodeToLine(node);
    }

    private void AddNodeToLine(GameObject node)
    {
        selectedNodes.Add(node);
        Vector3 nodePosition = node.transform.position;
        nodePosition.z = 0; // Ensure it's in the same plane

        linePositions.Add(nodePosition);
        linePositions.Add(nodePosition);
        linePositions.Add(nodePosition);
        linePositions.Add(nodePosition);
        UpdateLineRenderer();
    }

    private void UpdateMousePosition(Vector3 mousePosition)
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        currentMousePos.z = 0;
        UpdateLineRenderer();
    }

    private void ResetLine()
    {
        lineRenderer.positionCount = 0;
        selectedNodes.Clear();
        linePositions.Clear();
    }

    private void UpdateLineRenderer()
    {
        if (!lineRenderer) return;

        int pointCount = linePositions.Count + (isDragging ? 1 : 0);
        lineRenderer.positionCount = pointCount;

        for (int i = 0; i < linePositions.Count; i++)
        {
            lineRenderer.SetPosition(i, linePositions[i]);
        }

        if (isDragging)
        {
            lineRenderer.SetPosition(pointCount - 1, currentMousePos);
        }
        //OnLineUpdated?.Invoke(linePositions.ToArray());
    }

    private GameObject GetNodeUnderMouse(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(requiredTag))
            {
                return result.gameObject;
            }
        }

        return null;
    }

    private void ValidatePattern()
    {

        for (int i = 0; i < selectedNodes.Count; i++)
        {
            outputText[i].text = selectedNodes[i].GetComponentInChildren<Text>().text;
        }


    }

    private void ConfigureLineRenderer()
    {
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
    }

}

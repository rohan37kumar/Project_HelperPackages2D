using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This class is resposible for dragging operation and verifying it the answer is correct(applicable for test case only. Refactoring needed)
//This class also has event action for different stages of line dragging
public class DraggingLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Text outputText;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Camera mainCamera;

    private List<GameObject> selectedNodes = new List<GameObject>();
    private List<Vector3> linePositions = new List<Vector3>();
    private bool isDragging = false;
    private Vector3 currentMousePos;

    // C# action events
    public event Action OnLineStarted;
    public event Action OnLineEnded;
    public event Action<Vector3[]> OnLineUpdated;

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
            OnLineStarted?.Invoke();
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
        ValidatePattern();
        ResetLine();
        OnLineEnded?.Invoke();
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
        OnLineUpdated?.Invoke(linePositions.ToArray());
    }

    private GameObject GetNodeUnderMouse(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Node"))
            {
                return result.gameObject;
            }
        }

        return null;
    }

    private void ValidatePattern()
    {
        List<int> pattern = new List<int>();
        string patternValue = "";

        foreach (GameObject node in selectedNodes)
        {
            if (node.TryGetComponent(out LineDrawNode lineDrawNode))
            {
                pattern.Add(lineDrawNode.nodeID);
                patternValue += $" {lineDrawNode.nodeValue}";
            }
        }

        Debug.Log($"Pattern completed: {string.Join("-", pattern)}");
        outputText.text = patternValue.Trim();

        if (patternValue.Trim().Equals("Please Help Me !"))
        {
            IsSolved = true;
            Debug.Log($"Puzzle solved: {IsSolved}");
            //puzzleHandler.QuestionSolved();
        }
        else
        {
            Debug.Log("Incorrect pattern. Puzzle not solved.");
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

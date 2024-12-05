using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This class is responsible for handling line-dragging operations,
// including verifying patterns to determine correctness in test cases.
// It provides event-based actions for different stages of the dragging process.
public class DraggingLine : MonoBehaviour
{
    // References to various components used by the script.
    [SerializeField] private LineRenderer lineRenderer; // For rendering the dragged line.
    [SerializeField] private Text outputText; // Text to display the result or pattern.
    [SerializeField] private Material lineMaterial; // Material used for the line.
    [SerializeField] private float lineWidth = 0.1f; // Width of the rendered line.
    [SerializeField] private Camera mainCamera; // Reference to the main camera.
    [SerializeField] private string tag = "Node"; //Tag which is used to check for nodes.

    // Internal variables to manage the dragging logic.
    private List<GameObject> selectedNodes = new List<GameObject>(); // Tracks the nodes selected during dragging.
    private List<Vector3> linePositions = new List<Vector3>(); // Stores positions for the line renderer.
    private bool isDragging = false; // Indicates whether the user is currently dragging.
    private Vector3 currentMousePos; // Tracks the current mouse position during dragging.

    // Event actions for notifying different stages of line dragging.
    public event Action OnLineStarted; // Triggered when a line starts being drawn.
    public event Action OnLineEnded; // Triggered when the line drawing ends.
    public event Action<Vector3[]> OnLineUpdated; // Triggered whenever the line is updated.

    // Property to indicate if the puzzle is solved.
    public bool IsSolved { get; private set; }

    // Unity lifecycle method, initializes components and clears any previous state.
    private void Start()
    {
        // Ensure LineRenderer is set up correctly.
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

        ConfigureLineRenderer(); // Apply initial configurations to the LineRenderer.
        selectedNodes.Clear();
        linePositions.Clear();
    }

    // Unity lifecycle method, handles user input for dragging operations.
    private void Update()
    {
        // Handle mouse button events for starting, continuing, and ending dragging.
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

    // Handles the start of dragging when the mouse is pressed down.
    private void HandleMouseDown()
    {
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null)
        {
            StartDragging(node);
            OnLineStarted?.Invoke();
        }
    }

    // Handles the continuation of dragging as the mouse moves.
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

    // Handles the end of dragging when the mouse is released.
    private void HandleMouseUp()
    {
        isDragging = false;
        ValidatePattern(); // Check if the pattern created by dragging is correct.
        ResetLine(); // Clear the line and reset for the next drag.
        OnLineEnded?.Invoke();
    }

    // Starts the dragging process from a specific node.
    private void StartDragging(GameObject node)
    {
        isDragging = true;
        AddNodeToLine(node); // Add the initial node to the line.
    }

    // Adds a node to the line and updates the LineRenderer.
    private void AddNodeToLine(GameObject node)
    {
        selectedNodes.Add(node);
        Vector3 nodePosition = node.transform.position;
        nodePosition.z = 0; // Ensure the node position is on the same plane.

        linePositions.Add(nodePosition);
        UpdateLineRenderer(); // Update the line to include the new node.
    }

    // Updates the line renderer based on the current mouse position.
    private void UpdateMousePosition(Vector3 mousePosition)
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        currentMousePos.z = 0;
        UpdateLineRenderer();
    }

    // Resets the line renderer and clears all nodes and positions.
    private void ResetLine()
    {
        lineRenderer.positionCount = 0;
        selectedNodes.Clear();
        linePositions.Clear();
    }

    // Updates the LineRenderer with the latest positions.
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

    // Returns the GameObject under the mouse pointer with the tag "Node".
    private GameObject GetNodeUnderMouse(Vector2 mousePosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return result.gameObject;
            }
        }

        return null;
    }

    // Validates the pattern formed by the selected nodes and checks for correctness.
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
            // Additional puzzle solved logic can be added here.
        }
        else
        {
            Debug.Log("Incorrect pattern. Puzzle not solved.");
        }
    }

    // Configures the LineRenderer with the specified material and width.
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

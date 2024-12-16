using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingLine : MonoBehaviour
{
    // Serialized fields to configure the script from the Unity Inspector.
    [SerializeField] private LineRenderer lineRenderer; // The LineRenderer component used to draw the line.
    [SerializeField] private List<Text> outputText; // List of Text components for displaying node data.
    [SerializeField] private Material lineMaterial; // Material used for the line.
    [SerializeField] private float lineWidth = 0.1f; // Width of the line.
    [SerializeField] private Camera mainCamera; // Reference to the main camera.
    [SerializeField] private string requiredTag = "Node"; // Tag used to identify valid nodes.

    // Private fields for managing state during interaction.
    private List<GameObject> selectedNodes = new List<GameObject>(); // List of nodes selected during the drag.
    private List<Vector3> linePositions = new List<Vector3>(); // Positions of the line points.
    private bool isDragging = false; // Flag to check if the user is currently dragging.
    private Vector3 currentMousePos; // Current position of the mouse in world space.

    public bool IsSolved { get; private set; } // Property to indicate if the task is solved.

    private void Start()
    {
        // Ensure the LineRenderer component is assigned and configured.
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
        // Handle mouse input to start, update, and end the line drawing.
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
        // Detect if a valid node is clicked and start dragging.
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null)
        {
            StartDragging(node);
            LineActions.TriggerLineStarted(); // Trigger event when the line starts.
        }
    }

    private void HandleMouseDrag()
    {
        // Update the line as the user drags the mouse.
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null && !selectedNodes.Contains(node))
        {
            AddNodeToLine(node); // Add a new node to the line if not already selected.
        }
        else
        {
            UpdateMousePosition(Input.mousePosition); // Update the current mouse position.
        }
    }

    private void HandleMouseUp()
    {
        // Finalize the line when the mouse button is released.
        isDragging = false;

        // Check if fewer than two nodes are selected.
        if (selectedNodes.Count < 2)
        {
            Debug.Log("Only one node selected. Ignoring line.");
            ResetLine();
            return;
        }

        ValidatePattern(); // Validate the selected nodes' pattern.
        ResetLine(); // Reset the line for a new drag session.
        LineActions.TriggerLineEnded(string.Join(" ", outputText.Select(t => t.text))); // Trigger event with the output data.
    }

    private void StartDragging(GameObject node)
    {
        // Start the drag process and add the initial node.
        isDragging = true;
        AddNodeToLine(node);
    }

    private void AddNodeToLine(GameObject node)
    {
        // Add a node to the selected list and update the line.
        selectedNodes.Add(node);
        Vector3 nodePosition = node.transform.position;
        nodePosition.z = 0; // Ensure the position is in the same plane.

        // Add the node position multiple times to smooth the line.
        linePositions.Add(nodePosition);
        linePositions.Add(nodePosition);
        linePositions.Add(nodePosition);
        linePositions.Add(nodePosition);
        UpdateLineRenderer();
    }

    private void UpdateMousePosition(Vector3 mousePosition)
    {
        // Update the current mouse position in world space.
        currentMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        currentMousePos.z = 0;
        UpdateLineRenderer();
    }

    private void ResetLine()
    {
        // Clear the line and reset state for a new interaction.
        lineRenderer.positionCount = 0;
        selectedNodes.Clear();
        linePositions.Clear();
    }

    private void UpdateLineRenderer()
    {
        // Update the LineRenderer to match the current line positions.
        if (!lineRenderer) return;

        int pointCount = linePositions.Count + (isDragging ? 1 : 0);
        lineRenderer.positionCount = pointCount;

        for (int i = 0; i < linePositions.Count; i++)
        {
            lineRenderer.SetPosition(i, linePositions[i]);
        }

        if (isDragging)
        {
            lineRenderer.SetPosition(pointCount - 1, currentMousePos); // Extend the line to the mouse position.
        }
    }

    private GameObject GetNodeUnderMouse(Vector2 mousePosition)
    {
        // Perform a raycast to find a valid node under the mouse pointer.
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
        // Validate the selected pattern and update output text.
        for (int i = 0; i < selectedNodes.Count; i++)
        {
            outputText[i].text = selectedNodes[i].GetComponentInChildren<Text>().text;
        }
    }

    private void ConfigureLineRenderer()
    {
        // Set up the LineRenderer with the specified material and width.
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
    }
}

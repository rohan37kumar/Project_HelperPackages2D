using UnityEngine;
using System.Collections.Generic;

public class LineDrawManager : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.2f;

    private List<GameObject> selectedNodes = new List<GameObject>();
    private List<Vector3> linePositions = new List<Vector3>();
    private bool isDragging = false;
    private Vector3 currentPointerPosition;

    public bool IsDragging => isDragging;
    public List<GameObject> SelectedNodes => selectedNodes;
    public List<Vector3> LinePositions => linePositions;

    private void Start()
    {
        SetupLineRenderer();
    }

    private void SetupLineRenderer()
    {
        if (!lineRenderer) return;
        
        lineRenderer.startWidth = lineWidth / LineDrawUIManager.Instance.CanvasScaleFactor;
        lineRenderer.endWidth = lineWidth / LineDrawUIManager.Instance.CanvasScaleFactor;
        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = lineMaterial;
        lineRenderer.sortingOrder = 1;
        lineRenderer.generateLightingData = false;
        lineRenderer.allowOcclusionWhenDynamic = false;
    }

    public void StartPattern(GameObject node)
    {
        if (node == null) return;
        
        isDragging = true;
        selectedNodes.Clear();
        linePositions.Clear();
        AddNodeToPattern(node);
    }

    public void UpdatePattern(GameObject node)
    {
        if (!isDragging || node == null) return;

        if (!selectedNodes.Contains(node))
        {
            AddNodeToPattern(node);
        }
    }

    public void EndPattern()
    {
        if (!isDragging) return;

        isDragging = false;
        ValidatePattern();

        lineRenderer.positionCount = 0;
        selectedNodes.Clear();
        linePositions.Clear();
    }

    public void UpdateLinePosition(Vector3 position)
    {
        currentPointerPosition = position;
        UpdateLineRenderer();
    }

    private void AddNodeToPattern(GameObject node)
    {
        selectedNodes.Add(node);
        Vector3 nodePosition = LineDrawHelperFunctions.GetNodeWorldPosition(node);
        linePositions.Add(nodePosition);
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        if (!lineRenderer) return;

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
            pattern.Add(node.GetComponent<LineDrawNode>().nodeID);
            patternValue += " " + node.GetComponent<LineDrawNode>().nodeValue;
        }

        Debug.Log($"Pattern completed: {string.Join("-", pattern)}");
        LineDrawUIManager.Instance.UpdateOutputText(patternValue);
    }
}

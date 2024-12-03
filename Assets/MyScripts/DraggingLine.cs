using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Text outputText;
    [SerializeField] private PuzzleScript puzzleHandler;

    private List<GameObject> selectedNodes = new List<GameObject>();
    private List<Vector3> linePositions = new List<Vector3>();

    private bool isDragging = false;
    private Vector3 currentMousePos;
    public bool isSolved {  get;  private set; }


    private void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        selectedNodes.Clear();
        linePositions.Clear();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject node = GetNodeAtPosition(Input.mousePosition);
            if (node != null)
            {
                isDragging = true;
                selectedNodes.Add(node);

                Vector3 nodePosition = node.transform.position;
                nodePosition.z = 0;
                linePositions.Add(nodePosition);
                linePositions.Add(nodePosition);
                linePositions.Add(nodePosition);
                linePositions.Add(nodePosition);

                UpdateLineRenderer();
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            GameObject node = GetNodeAtPosition(Input.mousePosition);
            if (node != null && !selectedNodes.Contains(node))
            {
                selectedNodes.Add(node);

                Vector3 nodePosition = node.transform.position;
                nodePosition.z = 0;
                linePositions.Add(nodePosition);
                linePositions.Add(nodePosition);
                linePositions.Add(nodePosition);
                linePositions.Add(nodePosition);

                UpdateLineRenderer();
            }
            else
            {
                currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePos.z = 0;

                UpdateLineRenderer();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ValidatePattern();
            lineRenderer.positionCount = 0;
            selectedNodes.Clear();
            linePositions.Clear();
            UpdateLineRenderer();
        }
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
            lineRenderer.SetPosition(totalPoints - 1, currentMousePos);
        }
    }

    private GameObject GetNodeAtPosition(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = position
        };

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
            pattern.Add(node.GetComponent<LineDrawNode>().nodeID);
            patternValue += " " + node.GetComponent<LineDrawNode>().nodeValue;
        }

        Debug.Log($"Pattern completed: {string.Join("-", pattern)}");
        outputText.text = patternValue;
        Debug.Log($"patternValue: {patternValue}");

        if (patternValue.Trim().Equals("Please Help Me !"))
        {
            isSolved = true;
            Debug.Log($"isSolved: {isSolved}");
            puzzleHandler.QuestionSolved();
        }
        else
        {
            Debug.Log($"isSolved: {isSolved}");
        }
    }

    
}

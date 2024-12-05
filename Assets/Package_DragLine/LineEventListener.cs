using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEventListener : MonoBehaviour
{
    [SerializeField] private DraggingLine draggingLine;

    private void OnEnable()
    {
        draggingLine.OnLineStarted += HandleLineStarted;
        draggingLine.OnLineEnded += HandleLineEnded;
        draggingLine.OnLineUpdated += HandleLineUpdated;
    }

    private void OnDisable()
    {
        draggingLine.OnLineStarted -= HandleLineStarted;
        draggingLine.OnLineEnded -= HandleLineEnded;
        draggingLine.OnLineUpdated -= HandleLineUpdated;
    }

    private void HandleLineStarted()
    {
        Debug.Log("Line drawing started!");
    }

    private void HandleLineEnded()
    {
        Debug.Log("Line drawing ended!");
    }

    

    private void HandleLineUpdated(Vector3[] linePositions)
    {
        Debug.Log($"Line updated with {linePositions.Length} positions.");
    }
}

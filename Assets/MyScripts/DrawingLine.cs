using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingLine : MonoBehaviour
{
    [SerializeField] private LineDrawManager lineDrawManager;
    private GameObject nodeObject;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            nodeObject = LineDrawHelperFunctions.GetNodeAtPosition(Input.mousePosition);
            if (nodeObject != null)
            {
                lineDrawManager.StartPattern(nodeObject);
            }

        }

        if (Input.GetMouseButton(0))
        {
            GameObject currentNode = LineDrawHelperFunctions.GetNodeAtPosition(Input.mousePosition);

            if (currentNode != null)
            {
                lineDrawManager.UpdatePattern(currentNode);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            lineDrawManager.EndPattern();
        }

    }
}

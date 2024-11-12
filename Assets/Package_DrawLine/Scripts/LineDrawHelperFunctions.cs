using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class LineDrawHelperFunctions
{
    public static Vector3 GetWorldPositionFromPointer(Vector2 screenPosition, RectTransform canvasRectTransform, Canvas targetCanvas, Camera mainCamera)
    {
        Vector2 pointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            screenPosition,
            targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out pointerPosition
        );

        Vector3 worldPosition = canvasRectTransform.TransformPoint(pointerPosition);
        worldPosition.z = canvasRectTransform.position.z - 0.1f;

        return worldPosition;
    }

    public static GameObject GetNodeAtPosition(Vector2 position)
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

    public static Vector3 GetNodeWorldPosition(GameObject node)
    {
        RectTransform rectTransform = node.GetComponent<RectTransform>();
        Canvas canvas = node.GetComponentInParent<Canvas>();
        Vector3 nodePosition;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
            nodePosition = GetWorldPositionFromPointer(screenPoint, canvas.GetComponent<RectTransform>(), canvas, null);
        }
        else
        {
            nodePosition = rectTransform.position;
            nodePosition.z = node.transform.position.z - 0.1f;
        }

        return nodePosition;
    }
}
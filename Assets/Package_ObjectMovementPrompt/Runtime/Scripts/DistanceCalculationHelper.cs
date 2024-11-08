using UnityEngine;

namespace ezygamers.ObjectMovementPrompt
{
    public static class DistanceCalculationHelper
    {
        public static float CalculateDistanceBetween(GameObject uiObject1, GameObject uiObject2)
        {
            // Ensure both GameObjects have RectTransforms
            RectTransform rectTransform1 = uiObject1.GetComponent<RectTransform>();
            RectTransform rectTransform2 = uiObject2.GetComponent<RectTransform>();

            if (rectTransform1 == null || rectTransform2 == null)
            {
                Debug.LogError("One or both of the provided GameObjects do not have RectTransform components.");
                return -1f;
            }

            // Get world positions of the RectTransforms
            Vector3 position1 = rectTransform1.position;
            Vector3 position2 = rectTransform2.position;

            // Calculate and return the distance
            return Vector3.Distance(position1, position2);
        }

        public static (Vector2 startPosition, Vector2 endPosition) CalculateStartAndEndPositions(RectTransform rectTransform, float verticalDistance)
        {
            // Start position is the current anchored position
            Vector2 startPosition = rectTransform.anchoredPosition;

            // End position is the same x with the specified downward distance
            Vector2 endPosition = new Vector2(startPosition.x, startPosition.y - verticalDistance);

            return (startPosition, endPosition);
        }
    }
}

# Object Movement Prompt Package

This package provides utility classes and components for managing object movement prompts in Unity UI systems. It helps in creating visual cues for dragging actions with smooth animations.

## DistanceCalculationHelper Class

### Overview
The `DistanceCalculationHelper` class provides methods for calculating distances between UI elements and determining start and end positions for animations.

### Methods

#### CalculateDistanceBetween(GameObject uiObject1, GameObject uiObject2)
- **Description**: Computes the distance between two UI objects in world space.
- **Parameters**:
  - `uiObject1`: First UI GameObject.
  - `uiObject2`: Second UI GameObject.
- **Returns**: The distance between the two objects or `-1` if either object lacks a `RectTransform`.
- **Usage**:
```csharp
float distance = DistanceCalculationHelper.CalculateDistanceBetween(uiObject1, uiObject2);
```

#### CalculateStartAndEndPositions(RectTransform rectTransform, float verticalDistance)
- **Description**: Calculates the start and end positions for animations based on a vertical distance.
- **Parameters**:
  - `rectTransform`: The target `RectTransform`.
  - `verticalDistance`: The distance to move vertically.
- **Returns**: A tuple containing the start position and the calculated end position.
- **Usage**:
```csharp
(Vector2 start, Vector2 end) = DistanceCalculationHelper.CalculateStartAndEndPositions(rectTransform, 100f);
```

## DragObjectPrompt Class

### Overview
The `DragObjectPrompt` class manages drag prompts with animations for UI elements. It provides customizable options such as auto prompts, animation duration, and curve offsets.

### Serialized Fields
- **`enableAutoPrompt`**: Toggles automatic prompts.
- **`handPromptImage`**: The `Image` component representing the hand prompt.
- **`idleTimeThreshold`**: Time in seconds before showing the prompt.
- **`animationDuration`**: Duration of the animation.
- **`verticalDistance`**: Vertical movement distance for the prompt.
- **`curveOffset`**: Horizontal offset for the animation curve.

### Public Methods

#### CallObjectMovement()
- **Description**: Cancels any ongoing animations and prepares the prompt for manual triggering.

#### StartPromptAnimation()
- **Description**: Manually starts the prompt animation.

#### StopPromptAnimation()
- **Description**: Stops any ongoing animations.

#### SetPromptEnabled(bool enabled)
- **Description**: Enables or disables the prompt system.
- **Parameters**:
  - `enabled`: `true` to enable; `false` to disable.

### Animation Workflow

1. **Setup Prompt Positions**: The start and end positions for the animation are calculated during initialization.
2. **Start Animation**:
   - Moves the hand prompt image in a curved path using the `AnimationHelper`.
   - On completion, the prompt fades out and restarts if auto-prompt is enabled.
3. **Stop Animation**: Cancels any ongoing animations and hides the prompt.

### Example Usage

#### DragObjectPrompt Component
Attach the `DragObjectPrompt` script to a GameObject and configure its serialized fields in the Unity Inspector.

```csharp
public class ExampleUsage : MonoBehaviour
{
    [SerializeField] private DragObjectPrompt dragPrompt;

    private void Start()
    {
        // Enable or disable the prompt
        dragPrompt.SetPromptEnabled(true);

        // Start the prompt animation manually
        dragPrompt.StartPromptAnimation();
    }
}

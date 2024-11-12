using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LineDrawUIManager : MonoBehaviour
{
    public static LineDrawUIManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private Text outputText;
    [SerializeField] private LineDrawManager patternManager;

    private RectTransform canvasRectTransform;

    public float CanvasScaleFactor => targetCanvas.scaleFactor;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //Actions.gotInput += HandleInput;
    }

    private void OnDisable()
    {
        //Actions.gotInput -= HandleInput;
    }
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (targetCanvas == null)
            targetCanvas = GetComponentInParent<Canvas>();

        canvasRectTransform = targetCanvas.GetComponent<RectTransform>();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 worldPos = LineDrawHelperFunctions.GetWorldPositionFromPointer(touch.position, canvasRectTransform, targetCanvas, mainCamera);
        patternManager.UpdateLinePosition(worldPos);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                HandleInputBegan(touch.position);
                break;
            case TouchPhase.Moved:
                HandleInputMoved(touch.position);
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                HandleInputEnded(touch.position);
                break;
        }
    }

    private void HandleMouseInput()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldPos = LineDrawHelperFunctions.GetWorldPositionFromPointer(mousePosition, canvasRectTransform, targetCanvas, mainCamera);
        patternManager.UpdateLinePosition(worldPos);

        if (Input.GetMouseButtonDown(0))
            HandleInputBegan(mousePosition);
        else if (Input.GetMouseButton(0))
            HandleInputMoved(mousePosition);
        else if (Input.GetMouseButtonUp(0))
            HandleInputEnded(mousePosition);
    }

    private void HandleInputBegan(Vector2 position)
    {
        GameObject touchedNode = LineDrawHelperFunctions.GetNodeAtPosition(position);
        patternManager.StartPattern(touchedNode);
    }

    private void HandleInputMoved(Vector2 position)
    {
        GameObject touchedNode = LineDrawHelperFunctions.GetNodeAtPosition(position);
        patternManager.UpdatePattern(touchedNode);
    }

    private void HandleInputEnded(Vector2 position)
    {
        patternManager.EndPattern();
    }

    public void UpdateOutputText(string text)
    {
        if (outputText != null)
            outputText.text = text;
    }
}
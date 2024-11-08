using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace ezygamers.dragndropv1
{
    //this class implements the IDragStrategy interface to handle drap operation on UI element
    public class UIElementDrag : IDragStrategy
    {
        //reference of the element
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        //private bool isInDropZone = false;
        //store the intial position
        private Vector3 originalPosition;
        private GameObject currentDropArea = null;
        private Canvas parentCanvas;
        private Camera mainCamera;
        private bool isPointerDown = false;

        //intialization through constructor with required components
        public UIElementDrag(RectTransform rectTransform, CanvasGroup canvasGroup)
        {
            this.rectTransform = (rectTransform);
            this.canvasGroup = canvasGroup;
            originalPosition = rectTransform.localPosition;

            // Get reference to the parent canvas and camera, to make the dragged object visible
            parentCanvas = rectTransform.GetComponentInParent<Canvas>();
            mainCamera = parentCanvas.worldCamera;
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        //when user taps down on the image
        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
            Actions.onDragHighlight?.Invoke();
            rectTransform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isPointerDown && !eventData.dragging)
            {
                isPointerDown = false;
                Actions.onDragRemoveHighlight?.Invoke();
                rectTransform.localScale = Vector3.one;
            }
        }

        //when user start dragging the UI element
        public void OnBeginDrag(PointerEventData eventData)
        {
            //disable the raycast of dragged item
            canvasGroup.blocksRaycasts = false;   
        }

        //
        public void OnDrag(PointerEventData eventData)
        {
            Actions.onDrag?.Invoke();
            //update the position of the UI
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = parentCanvas.planeDistance; // Use canvas's plane distance
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            rectTransform.position = worldPos;

            GameObject newDropArea = null;
            if (eventData.pointerEnter != null)
            {
                // Try to get DropHandler from the current object or its parents
                var dropHandler = eventData.pointerEnter.GetComponent<DropHandler>();
                if (dropHandler == null)
                {
                    dropHandler = eventData.pointerEnter.GetComponentInParent<DropHandler>();
                }

                if (dropHandler != null)
                {
                    newDropArea = dropHandler.gameObject;
                }
            }

            // Handle drop area changes
            if (newDropArea != currentDropArea)
            {
                // Remove highlight from previous drop area
                if (currentDropArea != null)
                {
                    Actions.onDropRemoveHighlight?.Invoke(currentDropArea);
                    //isInDropZone = false;
                }

                // Add highlight to new drop area
                if (newDropArea != null)
                {
                    Actions.onDropHighlight?.Invoke(newDropArea);
                    //isInDropZone = true;
                }

                currentDropArea = newDropArea;
            }
        }

        //when user stops dragging
        public void OnEndDrag(PointerEventData eventData)
        {
            isPointerDown = false;
            //enable the raycast for interaction
            canvasGroup.blocksRaycasts = true;

            bool validDrop = false;
            if (eventData.pointerEnter != null)
            {
                var dropHandler = eventData.pointerEnter.GetComponent<DropHandler>();
                if (dropHandler == null)
                {
                    dropHandler = eventData.pointerEnter.GetComponentInParent<DropHandler>();
                }
                validDrop = dropHandler != null;
            }

            if (!validDrop)
            {
                // Convert original position back to world space
                Vector3 localPos = originalPosition;
                localPos.z = parentCanvas.planeDistance;
                rectTransform.localPosition = localPos;
            }

            // Remove all highlights
            if (currentDropArea != null)
            {
                Actions.onDropRemoveHighlight?.Invoke(currentDropArea);
                currentDropArea = null;
                //isInDropZone = false;
            }

            Actions.onDragRemoveHighlight?.Invoke();
            rectTransform.localScale = Vector3.one;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ezygamers.dragndropv1
{

    public class DragNDropActionManager : MonoBehaviour
    {

        [SerializeField] Image dropPanelImage;  // Reference to the drop panel's Image component
        [SerializeField] Color highlightColor;  // Color when highlighted
        private Color originalColor;

        //private bool isHighlighted = false;

        private void Start()
        {
            if (dropPanelImage != null)
            {
                originalColor = dropPanelImage.color;
            }
        }

        private void OnEnable()
        {
            Actions.onDragHighlight += HighlightDragItem;
            Actions.onDragRemoveHighlight += RemoveHighlightDragItem;
            Actions.onDropHighlight += HighlightDropArea;
            Actions.onDropRemoveHighlight += RemoveHighlightDropArea;

        }
        private void OnDisable()
        {
            //for outline
            Actions.onDragHighlight -= HighlightDragItem;
            Actions.onDragRemoveHighlight -= RemoveHighlightDragItem;

            //for drop area
            Actions.onDropHighlight -= HighlightDropArea;
            Actions.onDropRemoveHighlight -= RemoveHighlightDropArea;
        }


        private void RemoveHighlightDropArea(GameObject dropArea)
        {
            if (dropArea == this.gameObject && dropPanelImage != null)
            {
                dropPanelImage.color = originalColor;
            }
            //isHighlighted = false;
            //Debug.Log($"Remove highlight: {gameObject.name}");
        }

        private void HighlightDropArea(GameObject dropArea)
        {
            if (dropArea == this.gameObject && dropPanelImage != null)
            {
                dropPanelImage.color = highlightColor;
            }
            //isHighlighted = true;
            //Debug.Log($"Add highlight: {gameObject.name}");
        }



        private void RemoveHighlightDragItem()
        {
            //if (dropOutline != null)
            //{
            //    dropOutline.enabled = false;
            //}
        }

        private void HighlightDragItem()
        {
            //if (dropOutline != null)
            //{
            //    dropOutline.enabled = true;
            //}
        }

    }
}

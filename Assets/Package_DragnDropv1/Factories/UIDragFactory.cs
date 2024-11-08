using UnityEngine;
using Utilitis;//This my own Utilities for the GameObject
namespace ezygamers.dragndropv1
{

    //implementation of DragStrategyFactory for creating UIDragStrategy instance
    public class UIDragFactory : DragStrategyFactory
    {
        //method return a UIDragStrategy instance
        public override IDragStrategy CreateDraggable(GameObject gameObject)
        {
            // Ensure the GameObject has a RectTransform component (add it if it doesn't exist) from the utilities
            var rectTransform =gameObject.GetOrAddComponent<RectTransform>();
            // Ensure the GameObject has a CAnvasGroup component (add it if it doesn't exist)from the utilities
            var canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            //return the new UIDragStrategy
            return new UIElementDrag(rectTransform, canvasGroup);
        }
    }

    //TODO: add class for ObjectDragFactory

}

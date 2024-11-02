using UnityEngine;
namespace ezygamers.dragndropv1
{

    //base class for creating drag strategy instances e.g- UIDragStrategy
    public abstract class DragStrategyFactory {
        // Abstract method that must be implemented by derived classes to create an IDragStrategy
        // Takes a GameObject as a parameter and returns an IDragStrategy instance
        public abstract IDragStrategy CreateDraggable(GameObject gameObject);

    }
}

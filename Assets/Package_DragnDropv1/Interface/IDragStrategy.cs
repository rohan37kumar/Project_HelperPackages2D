using UnityEngine.EventSystems;

namespace ezygamers.dragndropv1
{
    //this interface  defines the contract for drag strategies
    public interface IDragStrategy
    {
        //called when being dragging an object
        void OnBeginDrag(PointerEventData eventData);

        //called when user is dragging the object
        void OnDrag(PointerEventData eventData);

        //called when user stops dragging the object
        void OnEndDrag(PointerEventData eventData);

        //called when pointer is down or up
        void OnPointerDown(PointerEventData eventData);
        void OnPointerUp(PointerEventData eventData);

    }

}

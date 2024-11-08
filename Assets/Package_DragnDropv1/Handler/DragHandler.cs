using UnityEngine;
using UnityEngine.EventSystems;

namespace ezygamers.dragndropv1
{
    //This class handle the drag logic of the GameObject
    //to the strategy defined by IDragHandler
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {

        //it used for handling the drag operations 
        public IDragStrategy dragStrategy;

        //if the gamobject is UI element or Not
        [SerializeField] bool isUI;

        private void Start()
        {

            if (isUI)
            {
                //create a new instance of UIDragFactory
                DragStrategyFactory factory = new UIDragFactory();
                //use the factory to create and assign a UIDragStrategy to dragStrategy
                dragStrategy = factory.CreateDraggable(this.gameObject);
                Debug.Log("UI elemnet can be dragged");
            }

            //TODO:create the strategy for Non UI Gameobject in else block

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Handle pointer down event
            dragStrategy?.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Handle pointer up event
            dragStrategy?.OnPointerUp(eventData);
        }

        //when user begins dragging a gameobject
        //if dragStrategy is assigned.
        public void OnBeginDrag(PointerEventData eventData)
        {
            dragStrategy?.OnBeginDrag(eventData);
        }


        //when user is dragging the GameObject
        //if dragStrategy is assigned.
        public void OnDrag(PointerEventData eventData) => dragStrategy?.OnDrag(eventData);
        //when the user stops dragging the GameObject
        //if dragStrategy is assigned.
        public void OnEndDrag(PointerEventData eventData)
        {
            dragStrategy?.OnEndDrag(eventData);
            
        }
    }

}
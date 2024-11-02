using UnityEngine;
using UnityEngine.EventSystems;
//using VContainer;


namespace ezygamers.dragndropv1
{
    public class DropHandler : MonoBehaviour, IDropHandler
    {
        public string OptionID; //holds the value of option ID from the OptionID of Question Data -rohan37kumar

        //this holds the initial position of the draggable object
        [SerializeField] private GameObject originalPos;


        // This method is called when an object is dropped onto this GameObject
        public void OnDrop(PointerEventData eventData)
        {
            //retrieve the DragHandler component from the object being dragged
            DragHandler draggableHandler = eventData.pointerDrag?.GetComponent<DragHandler>();


            if (draggableHandler != null)
            {

                //Get the transform of the draggableHandler GameObject
                var draggedGameObject = draggableHandler.gameObject.transform;

                //Snapping back the object to original position
                draggedGameObject.transform.position = originalPos.transform.position;

                Debug.Log($"Item Dropped on: {gameObject.name}");

                Actions.onItemDropped?.Invoke(this.gameObject); //triggering the event -rohan37kumar
                //TODO: Nudge this gameobject in Assembler Project
                //Debug.Log(OptionID);
                //Debug.Log("action called");
            }
        }

    }
}

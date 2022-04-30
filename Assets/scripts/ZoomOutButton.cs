using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomOutButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // attributes:
    Inspector inspector;
    private bool isPressed;
    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
        isPressed = false;
    }

    // OnPointerDown() sets the isPressed bool to true, allowing Update() to
    // make the inspected item appear to zoom out.
    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }

    // OnPointerUp() sets the isPressed bool to false, halting the zoom out
    // functionality within the Update() method.
    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
    }

    // Update() is called once per frame, and will make the inspected item zoom out
    // if the zoom out button is currently being pressed.
    void Update()
    {
        if (isPressed)
        {
            inspector.InspectedItem.InGameObject.GetComponent<InspectorControl>().ZoomOut();
        }
    }

    // OnDisable() is called when the zoom out button is no longer being pressed (i.e. when
    // the inspector is no longer in use). This method prevents the object from appearing 
    // more zoomed out than usual upon opening the inspector the next time around.
    void OnDisable()
    {
        isPressed = false;
    }

}

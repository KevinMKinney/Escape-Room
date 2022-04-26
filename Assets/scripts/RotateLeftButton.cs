using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // attributes:
    Inspector inspector;
    public bool isPressed;
    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
        isPressed = false;
    }

    // OnPointerDown() sets the isPressed bool to true, allowing Update()
    // rotate the inspected item in the specified direction
    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }

    // OnPointerUp() sets the isPressed bool to false, halting the
    // rotation of the inspected item within the Update() function below.
    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
    }

    // Update() runs once per frame. If the button is being pressed during the
    // current frame, the inspected item will rotate in the specified direction.
    void Update()
    {
        if (isPressed)
        {
            inspector.InspectedItem.InGameObject.GetComponent<InspectorControl>().RotateLeft();
        }
    }

    // OnDisable() runs when the button has been disable (i.e. when the inspector is not open).
    // This method ensures that the next inspected item will not begin rotating the moment that
    // the inspector is reopened.
    void OnDisable()
    {
        isPressed = false;
    }

}
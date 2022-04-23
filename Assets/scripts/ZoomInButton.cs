using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomInButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Inspector inspector;
    private bool isPressed;
    // Start is called before the first frame update
    void Start()
    {
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
        isPressed = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        isPressed = false;
    }

    void Update()
    {
        if (isPressed)
        {
            inspector.InspectedItem.InGameObject.GetComponent<InspectorControl>().ZoomIn();
        }
    }

    void OnDisable()
    {
        isPressed = false;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectOpenButton : MonoBehaviour, IPointerClickHandler
{
    // attributes:
    private Inspector inspector;

    // Start is called before the first frame update
    void Start()
    {
        // locate the inspector game object 
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
    }

    // OnPointerClick() handles the what happens when the inspect button is pressed
    // within the item control field.
    public void OnPointerClick(PointerEventData data)
    {
        inspector.ActivateInspector();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectCloseButton : MonoBehaviour, IPointerClickHandler
{
    // attributes:
    private Inspector inspector;

    // Start is called before the first frame update
    void Start()
    {
        // locate the inspector game object
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
    }

    // OnPointerClick() handles what happens when the 'esc' button is pressed
    // within the inspector window
    public void OnPointerClick(PointerEventData data)
    {
        inspector.DeactivateInspector();
    }
}

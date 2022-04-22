using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectButton : MonoBehaviour, IPointerClickHandler
{
    private InspectorControl inspectorControl;
    // Start is called before the first frame update
    void Start()
    {
        inspectorControl = GameObject.Find("UIPanel").GetComponent<InspectorControl>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        inspectorControl.ActivateInspector(); // closes the notebook/uicontrol screen
    }
}

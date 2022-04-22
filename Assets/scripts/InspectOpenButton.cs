using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectOpenButton : MonoBehaviour, IPointerClickHandler
{
    private UIControl uiControl;
    private Inspector inspector;

    // Start is called before the first frame update
    void Start()
    {
        uiControl = GameObject.Find("UIPanel").GetComponent<UIControl>();
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        inspector.ActivateInspector();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectCloseButton : MonoBehaviour, IPointerClickHandler
{
    private Inspector inspector;

    // Start is called before the first frame update
    void Start()
    {
        inspector = GameObject.Find("UIPanel").GetComponent<Inspector>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        inspector.DeactivateInspector();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorControl : MonoBehaviour
{
    private bool inspectorActive;
    private UIControl uiControl;
    // Start is called before the first frame update
    void Start()
    {
        uiControl = this.GetComponent<UIControl>();
        inspectorActive = false;
    }

    public bool InspectorActive
    {
        get { return inspectorActive; }
    }

    public void ActivateInspector()
    {
        inspectorActive = true;
    }

    public void DeactivateInspector()
    {
        inspectorActive = false;
    }
}

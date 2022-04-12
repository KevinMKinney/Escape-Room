using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public bool visible;
    // Start is called before the first frame update
    void Start()
    {
        HideUI();
    }

    public void HideUI()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        visible = false;
    }

    public void ShowUI()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            if (visible)
            {
                HideUI();
            } else
            {
                ShowUI();
            }
        }
    }
}

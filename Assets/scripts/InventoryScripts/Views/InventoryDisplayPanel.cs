using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayPanel : MonoBehaviour
{
    #region attributes
    public InventoryControl inventoryController;
    public GameObject displayPanel;
    public GameObject displayName;
    public GameObject displayDescription;
    public GameObject displayImage;
    #endregion

    #region methods
    // init() locates the game objects and scripts that are relevant to the display panel views
    public void init()
    {
        displayPanel = this.gameObject;
        displayName = displayPanel.transform.Find("DisplayName").gameObject;
        displayDescription = displayPanel.transform.Find("DisplayDescription").gameObject;
        displayImage = displayPanel.transform.Find("DisplayImage").gameObject;
        inventoryController = GameObject.Find("InventoryWrapper").GetComponent<InventoryControl>();
    }
    #endregion
}
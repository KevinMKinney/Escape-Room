using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryDisplayControl : MonoBehaviour
{
    #region attributes
    public GameObject inventoryPanel;
    #endregion

    #region methods
    private void Awake()
    {
        // locate inventory panel and disable it so it doesn't appear upon game start
        inventoryPanel = transform.GetChild(0).gameObject;
        inventoryPanel.SetActive(false);
    }

    public void Update()
    {
        // check if the "Inventory" button has been pressed
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventoryPanel.activeSelf)
            {
                // user has exited the inventory panel
                inventoryPanel.SetActive(false);
            }
            else
            {
                // user has opened the inventory panel
                inventoryPanel.SetActive(true);
            }
        }
    }

    #endregion
}
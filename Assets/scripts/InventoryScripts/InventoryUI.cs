using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region attributes
    private Inventory inventoryInstance;
    public GameObject inventoryPanelParent;
    public GameObject inventoryPanel;
    private List<InventorySlot> inventorySlots;
    #endregion

    #region constructors
    public void Awake()
    {
        inventoryPanelParent = GameObject.Find("Inventory");
        inventoryPanel = inventoryPanelParent.transform.GetChild(0).gameObject;
        inventoryPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            if (inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
            } else
            {
                inventoryPanel.SetActive(true);
            }
        }
    }

    #endregion

    #region methods
    public void UpdateUI()
    {
        // update of the inventory panel
    }
    #endregion
}

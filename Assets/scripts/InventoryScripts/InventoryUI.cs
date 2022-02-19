using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region attributes
    public GameObject inventoryGameObj;
    public List<Item> inventoryInstance;
    public GameObject itemPanel;
    public GameObject displayPanel;
    #endregion

    private void Awake()
    {
        // get inventory game object and update the inventory
        inventoryGameObj = transform.parent.gameObject;
        GetUpdatedInventory();

        // get ui components
        itemPanel = transform.GetChild(0).gameObject;
        displayPanel = transform.GetChild(1).gameObject;
    }

    private void OnEnable()
    {
        GetUpdatedInventory();
        UpdateItemPanel();
    }

    private void GetUpdatedInventory()
    {
        inventoryInstance = inventoryGameObj.GetComponent<Inventory>().GetItems();
    }

    private void UpdateItemPanel()
    {
        inventoryInstance.ForEach(x => 
        {
            Debug.Log(x.ItemName);
            Debug.Log(x.ShortDescription);
            Debug.Log(x.LongDescription);
        });
    }
}

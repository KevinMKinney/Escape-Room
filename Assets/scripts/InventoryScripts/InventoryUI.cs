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
        int i = 0;
        UnityEngine.UI.Text itemNameComponent;
        UnityEngine.UI.Text itemDescComponent;
        //UnityEngine.UI.Image itemSpriteComponent;
        inventoryInstance.ForEach(x => 
        {
            // get the ui components to assign this item to
            itemNameComponent = itemPanel.transform.GetChild(i).GetChild(0).GetComponent<UnityEngine.UI.Text>();
            itemDescComponent = itemPanel.transform.GetChild(i).GetChild(1).GetComponent<UnityEngine.UI.Text>();
            //itemSpriteComponent = itemPanel.transform.GetChild(i).GetChild(1).GetComponent<UnityEngine.UI.Image>();

            // format the text for the item name
            itemNameComponent.text = x.ItemName;
            itemNameComponent.fontSize = 24;
            itemNameComponent.fontStyle = FontStyle.Bold;
            itemNameComponent.alignment = TextAnchor.LowerLeft;

            // format the text for the item short description
            itemDescComponent.text = x.ShortDescription;
            itemDescComponent.fontSize = 16;
            itemDescComponent.fontStyle = FontStyle.Italic;
            itemDescComponent.alignment = TextAnchor.UpperLeft;

            // move to next ui component
            i++;
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropButton : MonoBehaviour, IPointerClickHandler
{
    private Inventory inventory;
    private ItemList itemList;
    private ItemDisplay itemDisplay;
    private ItemControls itemControls;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        Item itemToDrop = inventory.GetSelectedItem();

        if (itemToDrop != null)
        {
            itemToDrop.InGameObject.SetActive(true);
            itemToDrop.InGameObject.GetComponent<pick_up>().dropInitiated = true;
        }
        
        inventory.DropItem(inventory.GetSelectedItemIndex());
        itemDisplay.UpdateItemDisplay(null);
        itemList.UpdateList(inventory);
    }
}

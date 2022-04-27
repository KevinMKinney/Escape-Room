using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropButton : MonoBehaviour, IPointerClickHandler
{
    // attributes:
    private Inventory inventory;
    private ItemList itemList;
    private ItemDisplay itemDisplay;
    private ItemControls itemControls;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();
    }

    // OnPointerClick() handles what happens when the drop button is clicked
    public void OnPointerClick(PointerEventData data)
    {
        Item itemToDrop = inventory.GetSelectedItem(); // get the item to be dropped

        if (itemToDrop != null)
        {
            // activate the item in-game object in the game world and initiate the drop for
            // when the time scale is normalized again (after ui close)
            itemToDrop.InGameObject.SetActive(true);
            itemToDrop.InGameObject.GetComponent<pick_up>().dropInitiated = true;
        }
        
        // drop the item from the inventory and update the item display and item list
        inventory.DropItem(inventory.GetSelectedItemIndex());
        itemDisplay.UpdateItemDisplay(null);
        itemList.UpdateList(inventory);
    }
}

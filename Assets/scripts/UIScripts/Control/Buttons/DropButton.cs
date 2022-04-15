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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData data)
    {
        inventory.DropItem(inventory.GetSelectedItemIndex());
        itemDisplay.UpdateItemDisplay(null);
        itemList.UpdateList(inventory);
    }
}

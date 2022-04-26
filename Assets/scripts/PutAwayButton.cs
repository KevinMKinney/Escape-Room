using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PutAwayButton : MonoBehaviour, IPointerClickHandler
{
    // attributes:
    private Inventory inventory;
    private ItemList itemList;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of the attributes:
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
    }

    // OnPointerClick() handles the actions taken when the PutAwayButton is
    // clicked: the selected item is unequipped and the item list is updated
    // to reflect that the item is no longer equipped.
    public void OnPointerClick(PointerEventData data)
    {
        inventory.PutAwayItem();
        itemList.UpdateList(inventory);
    }
}

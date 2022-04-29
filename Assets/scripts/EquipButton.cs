using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipButton : MonoBehaviour, IPointerClickHandler
{
    // attributes:
    private Inventory inventory;
    private ItemList itemList;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
    }

    /// <summary>
    /// OnPointerClick() handles what happens when the EquipButton is pressed.
    /// When pressed, the selected item will be equipped within the inventory
    /// and the inventory list will update to reflect that change.
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerClick(PointerEventData data)
    {
        inventory.EquipItem(inventory.GetSelectedItemIndex());
        itemList.UpdateList(inventory);
    }
}

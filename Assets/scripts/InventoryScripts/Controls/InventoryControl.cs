using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControl : MonoBehaviour
{
    GameObject inventoryWrapper;
    GameObject inventoryPanel;
    Inventory inventory;
    // Start is called before the first frame update
    void Awake()
    {
        inventoryWrapper = this.gameObject;
        inventoryPanel = inventoryWrapper.transform.Find("InventoryPanel").gameObject;
        inventoryPanel.SetActive(false);

        inventory = inventoryWrapper.GetComponent<Inventory>();
    }

    public List<Item> Add(Item item)
    {
        inventory.AddItem(item);
        return inventory.GetItems();
    }

    public List<Item> Drop(int index)
    {
        inventory.DropItem(index);
        return inventory.GetItems();
    }

    public int Equip(int index)
    {
        inventory.EquipItem(index);
        return inventory.GetEquippedItemIndex();
    }

    public void PutAway()
    {
        inventory.PackUpItem();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

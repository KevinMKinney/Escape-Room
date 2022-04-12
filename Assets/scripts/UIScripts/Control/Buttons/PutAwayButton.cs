using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PutAwayButton : MonoBehaviour, IPointerClickHandler
{
    private Inventory inventory;
    private ItemList itemList;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData data)
    {
        inventory.PutAwayItem();
        itemList.UpdateList(inventory);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public bool visible;
    private ItemList itemList;
    private Inventory inventory;
    private ItemDisplay itemDisplay;
    private ItemControls itemControls;
    // Start is called before the first frame update
    void Start()
    {
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
        inventory = GameObject.FindGameObjectWithTag("items").GetComponent<Inventory>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();
        HideUI();
    }

    public void HideUI()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        visible = false;
    }

    public void ShowUI()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        visible = true;
        itemList.UpdateList(inventory);
        inventory.SelectItem(-1);
        itemDisplay.UpdateItemDisplay(null);
        itemControls.HideControls();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            if (visible)
            {
                Time.timeScale = 1.0f;
                HideUI();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            } else
            {
                Time.timeScale = 0f;
                ShowUI();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}

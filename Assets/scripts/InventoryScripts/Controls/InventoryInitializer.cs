using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ***********************************************************
// ***********************************************************
// ** InventoryInitializer
// ** 
// ** The sole purpose of this class is to create the in-game
// ** panels that make up the inventory user-interface at the 
// ** start of the game, and attach the necessary scripts to
// ** each panel. This class needs to be attached to a
// ** parent Canvas object, and the inventory will be created
// ** as a child to that Canvas.
// *
//

public class InventoryInitializer : MonoBehaviour
{
    private GameObject parentCanvas;
    private GameObject inventoryPanel;
    private GameObject slotPanel;
    private GameObject displayPanel;
    private GameObject controlPanel;
    private GameObject inventoryObject;
    private Inventory inventory;

    // Start is the function used to initialize all UI elements, as well as the 
    // actual inventory...
    void Start()
    {
        // get the Canvas element that this script is attached to
        parentCanvas = this.gameObject;

        // create an empty Inventory object as a sibling to the parent Canvas,
        // then attach the Inventory class/script to the Inventory object
        inventoryObject = new GameObject("Inventory");
        inventoryObject.transform.SetParent(parentCanvas.transform.parent);
        inventoryObject.AddComponent<Inventory>();
        inventory = inventoryObject.GetComponent<Inventory>();

        // Create Inventory Panel if it doesn't exist
        if (!parentCanvas.transform.Find("InventoryPanel"))
        {
            inventoryPanel = CreatePanel("InventoryPanel", parentCanvas);
            SetBoundaries(inventoryPanel.GetComponent<RectTransform>(), 10f, 10f, 10f, 10f);
        }

        // Create the slot panel if it doesn't exist
        if (!inventoryPanel.transform.Find("SlotPanel"))
        {
            slotPanel = CreatePanel("SlotPanel", inventoryPanel);
            SetBoundaries(slotPanel.GetComponent<RectTransform>(), 20f, 500f, 20f, 20f);
            slotPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(255f, 0f, 0f);
            slotPanel.AddComponent<InventorySlotPanel>();
        }

        // Create the slots in the slot panel
        for (int i = 1; i <= inventory.GetMaxItemCount(); i++)
        {
            GameObject slot = new GameObject("Item" + i);
            slot.AddComponent<SlotPanel>();
        }

        // Create the display panel if it doesn't exist
        if (!inventoryPanel.transform.Find("DisplayPanel"))
        {
            displayPanel = CreatePanel("DisplayPanel", inventoryPanel);
            SetBoundaries(displayPanel.GetComponent<RectTransform>(), 20f, 20f, 100f, 300f);
            displayPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 255f, 0f);
            displayPanel.AddComponent<InventoryDisplayPanel>();
        }

        // Create the control panel if it doesn't exist
        if (!inventoryPanel.transform.Find("ControlPanel"))
        {
            controlPanel = CreatePanel("ControlPanel", inventoryPanel);
            SetBoundaries(controlPanel.GetComponent<RectTransform>(), 500f, 20f, 20f, 300f);
            controlPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 0f, 255f);
            controlPanel.AddComponent<InventoryControlPanel>();
        }
    }

    public GameObject CreatePanel(string name, GameObject parent)
    {
        GameObject newGameObj = new GameObject(name);
        newGameObj.transform.SetParent(parent.transform);
        newGameObj.AddComponent<RectTransform>();
        newGameObj.AddComponent<CanvasRenderer>();
        newGameObj.AddComponent<UnityEngine.UI.Image>();

        return newGameObj;
    }

    public void SetBoundaries(RectTransform panelRT, float top, float right, float bottom, float left)
    {
        // set anchors
        panelRT.anchorMin = new Vector2(0f, 0f);
        panelRT.anchorMax = new Vector2(1f, 1f);

        // set boundaries
        panelRT.offsetMin = new Vector2(left, bottom);
        panelRT.offsetMax = new Vector2(-right, -top);

        // set scale
        panelRT.localScale = new Vector3(1f, 1f, 1f);
    }
}

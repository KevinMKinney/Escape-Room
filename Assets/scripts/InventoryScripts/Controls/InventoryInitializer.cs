using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ***********************************************************
// ***********************************************************
// ** InventoryInitializer
// ** 
// ** The purpose of this class is to create the in-game panels
// ** and objects that make up the inventory user-interface at the 
// ** start of the game, and attach the necessary scripts to
// ** each panel. This class needs to be attached to a parent
// ** Canvas object.
// *
//

public class InventoryInitializer : MonoBehaviour
{
    #region attributes
    private GameObject inventoryPanel;
    private GameObject slotPanel;
    private GameObject displayPanel;
    private GameObject controlPanel;
    private GameObject inventoryWrapper;
    private GameObject displayImage;
    private GameObject displayName;
    private GameObject displayDescription;
    private GameObject equipButton;
    private GameObject dropButton;
    private GameObject putAwayButton;
    private GameObject inspectButton;
    private Inventory inventory;

    // Attributes used for tests:
    private InventoryControl inventoryController;
    private InventorySlotPanel inventorySlotPanel;
    #endregion

    // Start is the function used to initialize all UI elements, as well as the 
    // actual inventory...
    void Start()
    {
        // Get the object that this script is attached to
        inventoryWrapper = this.gameObject;

        // Add the inventory
        inventoryWrapper.AddComponent<Inventory>();
        inventory = inventoryWrapper.GetComponent<Inventory>();

        // Create Inventory Panel:
        inventoryPanel = CreatePanel("InventoryPanel");
        inventoryPanel.transform.SetParent(inventoryWrapper.transform);
        SetBoundaries(inventoryPanel.GetComponent<RectTransform>(), 10f, 10f, 10f, 10f);

        // Create the Slot Panel:
        slotPanel = CreatePanel("SlotPanel");
        slotPanel.transform.SetParent(inventoryPanel.transform);
        //SetBoundaries(slotPanel.GetComponent<RectTransform>(), 20f, 500f, 20f, 20f);
        //slotPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(255f, 0f, 0f);
        slotPanel.AddComponent<InventorySlotPanel>();

        // Create the Inventory Slots in the Slot Panel:
        // TODO: Functional reformatting?
        for (int i = 1; i <= Inventory.maxItemCount; i++)
        {
            GameObject slot = CreateItemSlot("Item", i, slotPanel);
        }

        // Create the DisplayPanel:
        displayPanel = CreatePanel("DisplayPanel");
        displayPanel.transform.SetParent(inventoryPanel.transform);
        //SetBoundaries(displayPanel.GetComponent<RectTransform>(), 20f, 20f, 100f, 300f);
        //displayPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 255f, 0f);
        displayPanel.AddComponent<InventoryDisplayPanel>();

        // Create Display Panel Elements (image, name, description):
        displayImage = CreatePanel("DisplayImage");
        displayName = CreateTextObject("DisplayName");
        displayDescription = CreateTextObject("DisplayDescription");
        displayImage.transform.SetParent(displayPanel.transform);
        displayName.transform.SetParent(displayPanel.transform);
        displayDescription.transform.SetParent(displayPanel.transform);

        // Create the control panel if it doesn't exist
        controlPanel = CreatePanel("ControlPanel");
        controlPanel.transform.SetParent(inventoryPanel.transform);
        //SetBoundaries(controlPanel.GetComponent<RectTransform>(), 500f, 20f, 20f, 300f);
        //controlPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 0f, 255f);
        controlPanel.AddComponent<InventoryControlPanel>();

        // Create the control panel buttons
        equipButton = CreateButton("EquipButton", controlPanel);
        putAwayButton = CreateButton("PutAwayButton", controlPanel);
        dropButton = CreateButton("DropButton", controlPanel);
        inspectButton = CreateButton("InspectButton", controlPanel);

        // Add the inventory control
        inventoryWrapper.AddComponent<InventoryControl>();

        // set inventoryPanel to inactive at start of game
        inventoryPanel.SetActive(false);

        // initialize inventory tests
        inventory.initTests();

        // initialize scripts
        inventoryWrapper.GetComponent<InventoryControl>().init();
        slotPanel.GetComponent<InventorySlotPanel>().init();
        displayPanel.GetComponent<InventoryDisplayPanel>().init();
        controlPanel.GetComponent<InventoryControlPanel>().init();

        // initialize scripts in the slots
        for (int i = 0; i < slotPanel.transform.childCount; i++)
        {
            slotPanel.transform.GetChild(i).GetComponent<InventorySlot>().init();
        }

    }

    // CreatePanel() dynamically creates a Panel object, with equivalent
    // components to the default Panel objects created by Unity
    public GameObject CreatePanel(string name)
    {
        GameObject newGameObj = new GameObject(name);
        newGameObj.AddComponent<RectTransform>();
        newGameObj.AddComponent<CanvasRenderer>();
        newGameObj.AddComponent<UnityEngine.UI.Image>();

        return newGameObj;
    }

    // CreateTextObject() dynamically creates a Text object, with equivalent
    // components to the default Text objects created by Unity
    public GameObject CreateTextObject(string name)
    {
        GameObject textObj = new GameObject(name);
        textObj.AddComponent<RectTransform>();
        textObj.AddComponent<CanvasRenderer>();
        textObj.AddComponent<UnityEngine.UI.Text>();

        return textObj;
    }

    // CreateButton() dynamically creates a Button object, with equivalent
    // components to the default Button objects created by Unity
    public GameObject CreateButton(string name, GameObject parent)
    {
        // Create button GameObject
        GameObject button = CreatePanel(name);
        button.AddComponent<UnityEngine.UI.Button>();

        // Create text GameObject for button
        GameObject buttonText = CreateTextObject("Text");

        buttonText.transform.SetParent(button.transform);
        button.transform.SetParent(parent.transform);

        return button;
    }

    // CreateItemSlot() dynamically creates a Panel with multiple Text object
    // children representing the name, short-description, and icon of an
    // item in the inventory.
    public GameObject CreateItemSlot(string name, int i, GameObject parent)
    {
        GameObject slot = CreatePanel(name + i);
        slot.transform.SetParent(parent.transform);

        // Create individual slot elements: (slotName, slotDescription, slotIcon)
        GameObject slotName = CreateTextObject("SlotName");
        slotName.transform.SetParent(slot.transform);

        GameObject slotDescription = CreateTextObject("SlotDescription");
        slotDescription.transform.SetParent(slot.transform);

        GameObject slotIcon = CreatePanel("SlotIcon");
        slotIcon.transform.SetParent(slot.transform);

        // assign the 
        slot.AddComponent<InventorySlot>();
        slot.GetComponent<InventorySlot>().slotNumber = i;
        return slot;
    }
    
    // SetBoundaries() assigns the size and location of a Panel object
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

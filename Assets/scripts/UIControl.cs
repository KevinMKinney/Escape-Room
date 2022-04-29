using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    // attributes:
    public bool visible;
    private ItemList itemList;
    private Inventory inventory;
    private ItemDisplay itemDisplay;
    private ItemControls itemControls;
    private Tab itemTab;
    private Tab notesTab;
    private Tab menuTab;
    private NoteBook noteBook;
    private Inspector inspector;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
        inventory = GameObject.FindGameObjectWithTag("items").GetComponent<Inventory>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();

        // need to locate the following to handle positioning of tabs
        // during notebook scaling
        itemTab = GameObject.Find("ItemsTab").GetComponent<Tab>();
        notesTab = GameObject.Find("NotesTab").GetComponent<Tab>();
        menuTab = GameObject.Find("MenuTab").GetComponent<Tab>();

        // locate the notebook and hide it
        noteBook = GameObject.Find("NoteBook").GetComponent<NoteBook>();
        HideUI();

        // locate inspectorController
        inspector = this.GetComponent<Inspector>();
    }

    /// <summary>
    /// HideUI() deactivates the Notebook UI, and handles the changes in each UI
    /// component necessary to prevent odd behavior!
    /// </summary>
    public void HideUI()
    {

        // handle tab position and then deactivate tabs (prevents tabs from
        // being in the wrong position the next time the notebook is opened)
        if (itemTab.tabHovered) itemTab.MoveLeft();
        if (notesTab.tabHovered) notesTab.MoveLeft();
        if (menuTab.tabHovered) menuTab.MoveLeft();

        itemTab.tabHovered = false;
        notesTab.tabHovered = false;
        menuTab.tabHovered = false;

        itemTab.gameObject.SetActive(false);
        notesTab.gameObject.SetActive(false);
        menuTab.gameObject.SetActive(false);

        if (noteBook.CurrentTab == 1)
        {
            // deactivate to prevent change in scrollbar scaling
            GameObject.Find("Scrollbar").SetActive(false);
        }

        // disable the cursor and show the reticle
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Reticle").GetComponent<Image>().enabled = true;

        // hide (but don't disable) the ui by changing the scale
        this.transform.localScale = new Vector3(0, 0, 0);
        visible = false;
    }

    /// <summary>
    /// ShowUI() activates the notebook UI and handles the changes needed
    /// to prevent odd behavior when opening.
    /// </summary>
    public void ShowUI()
    {
        // pause the game, activate the notebook tabs
        Time.timeScale = 0f;
        itemTab.gameObject.SetActive(true);
        notesTab.gameObject.SetActive(true);
        menuTab.gameObject.SetActive(true);

        // display the notebook ui, update the item list to reflect any
        // changes in the inventory since last opening the ui
        this.transform.localScale = new Vector3(1, 1, 1);
        visible = true;
        itemList.UpdateList(inventory);
        inventory.SelectItem(-1);
        itemDisplay.UpdateItemDisplay(null);
        itemControls.HideControls();


        noteBook.OnTabChange(); // trigger tab change

        // show the cursor and give the cursor control
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // turn off the reticle
        GameObject.Find("Reticle").GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            if (visible)
            {
                // un pause the game and hide the ui
                Time.timeScale = 1.0f;
                HideUI();
            } else
            {
                // pause (or keep the game paused) and show the ui
                Time.timeScale = 0f;
                if (inspector.Active) inspector.DeactivateInspector();
                ShowUI();
            }
        }
    }
}

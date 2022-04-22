using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public bool visible;
    private ItemList itemList;
    private Inventory inventory;
    private ItemDisplay itemDisplay;
    private ItemControls itemControls;
    private Tab itemTab;
    private Tab notesTab;
    private Tab menuTab;
    private NoteBook noteBook;
    private InspectorControl inspectorControl;

    // Start is called before the first frame update
    void Start()
    {
        itemList = GameObject.Find("ItemList").GetComponent<ItemList>();
        inventory = GameObject.FindGameObjectWithTag("items").GetComponent<Inventory>();
        itemDisplay = GameObject.Find("ItemDisplay").GetComponent<ItemDisplay>();
        itemControls = GameObject.Find("ItemControls").GetComponent<ItemControls>();

        // need to locate the following to handle positioning of tabs
        // during notebook scaling
        itemTab = GameObject.Find("ItemsTab").GetComponent<Tab>();
        notesTab = GameObject.Find("NotesTab").GetComponent<Tab>();
        menuTab = GameObject.Find("MenuTab").GetComponent<Tab>();

        // locate the notebook
        noteBook = GameObject.Find("NoteBook").GetComponent<NoteBook>();
        HideUI();

        // locate inspectorController
        inspectorControl = this.GetComponent<InspectorControl>();
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

                // handle tab position and then deactivate tabs
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

                HideUI();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            } else
            {

                if (inspectorControl.InspectorActive)
                {
                    inspectorControl.DeactivateInspector();
                }

                Time.timeScale = 0f;
                itemTab.gameObject.SetActive(true);
                notesTab.gameObject.SetActive(true);
                menuTab.gameObject.SetActive(true);

                ShowUI();
                noteBook.OnTabChange(); // trigger tab change
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}

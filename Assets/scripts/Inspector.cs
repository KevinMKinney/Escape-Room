using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inspector : MonoBehaviour
{
    // attributes:
    private bool active;
    private UIControl uiControl;
    private Inventory inventory;
    private Item currentlyEquippedItem;
    private Item inspectedItem;
    private GameObject inspectorPanel;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        uiControl = this.GetComponent<UIControl>();
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        currentlyEquippedItem = null;
        inspectedItem = null;
        active = false;
        inspectorPanel = GameObject.Find("InspectorPanel");
        inspectorPanel.SetActive(false);
    }

    #region Getters
    public bool Active
    {
        get { return active; }
    }

    public Item InspectedItem
    {
        get { return inspectedItem; }
    }
    #endregion

    public void ActivateInspector()
    {
        // store and disable the currently equipped item (so we can re-enable after inspection)
        currentlyEquippedItem = inventory.GetEquippedItem();
        if (currentlyEquippedItem != null)
        {
            currentlyEquippedItem.InGameObject.SetActive(false);
        }


        // get the selected item
        inspectedItem = inventory.GetSelectedItem();

        if (inspectedItem == null) {
            // no item actually selected so re-enable the equipped item and exit
            if (currentlyEquippedItem != null)
            {
                currentlyEquippedItem.InGameObject.SetActive(true);
            }
            return;
        }
        
        // activate the selectedItem, deactivate the pick-up script,
        // and activate the inspector control script
        inspectedItem.InGameObject.SetActive(true);
        inspectedItem.InGameObject.GetComponent<pick_up>().enabled = false;
        inspectedItem.InGameObject.GetComponent<InspectorControl>().enabled = true;
        inspectedItem.InGameObject.GetComponent<InspectorControl>().SetInspectorPosition();

        // hide the UI menu and replace with inspector UI
        uiControl.HideUI();
        inspectorPanel.SetActive(true);

        // set the inspector panel item name text
        GameObject.Find("InspectorItemName").GetComponent<TMP_Text>().text = inspectedItem.ItemName;

        // keep the mouse active and remove the reticle (which was reactivated by HideUI)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject.Find("Reticle").GetComponent<Image>().enabled = false;

        // set inspector as active        
        active = true;

        Debug.Log(inspectedItem.ItemName);
    }

    public void DeactivateInspector()
    {
        // deactivate the selectedItem
        if (inspectedItem != null)
        {
            inspectedItem.InGameObject.GetComponent<InspectorControl>().RemoveInspectorPosition();
            inspectedItem.InGameObject.GetComponent<InspectorControl>().ResetScaleFactor();
            inspectedItem.InGameObject.GetComponent<InspectorControl>().enabled = false;
            inspectedItem.InGameObject.GetComponent<pick_up>().enabled = true;
            inspectedItem.InGameObject.SetActive(false);
        }

        // reactivate the equippedItem
        if (currentlyEquippedItem != null)
        {
            currentlyEquippedItem.InGameObject.SetActive(true);
        }

        // hide the inspector panel
        inspectorPanel.SetActive(false);

        // set the currentlyEquippedItem to null to prepare for the
        // next inspection
        currentlyEquippedItem = null;

        active = false;

        // redisplay the ui
        uiControl.ShowUI();
    }
}

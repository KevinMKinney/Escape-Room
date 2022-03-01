using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControlPanel : MonoBehaviour
{
    #region attributes
    GameObject controlPanel;
    GameObject equipButton;
    GameObject putAwayButton;
    GameObject dropButton;
    GameObject inspectButton;
    InventoryControl inventoryController;
    #endregion attributes

    // init() locates the relavent game objects for the control panel and
    // assigns functions to the control buttons
    public void init()
    {
        // locate game objects
        controlPanel = this.gameObject;
        equipButton = controlPanel.transform.Find("EquipButton").gameObject;
        putAwayButton = controlPanel.transform.Find("PutAwayButton").gameObject;
        dropButton = controlPanel.transform.Find("DropButton").gameObject;
        inspectButton = controlPanel.transform.Find("InspectButton").gameObject;

        // assign on click functions
        equipButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(equip);
        putAwayButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(putAway);
        dropButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(drop);
        inspectButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(inspect);
    }


    // Control button functions (NOTE: functions have not been fleshed out yet,
    // but they will be calling functions within the main InventoryControl object
    public void equip()
    {
        Debug.Log("Equip Button clicked");
    }

    public void putAway()
    {
        Debug.Log("PutAway Button clicked");
    }

    public void drop()
    {
        Debug.Log("Drop Button clicked");
    }

    public void inspect()
    {
        Debug.Log("Inspect Button clicked");
    }
}

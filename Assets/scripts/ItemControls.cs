using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControls : MonoBehaviour
{
    // attributes:
    public Inventory inventory;
    public GameObject equipButton;
    public GameObject putAwayButton;
    public GameObject inspectButton;
    public GameObject dropButton;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        equipButton = GameObject.Find("EquipButton");
        putAwayButton = GameObject.Find("PutAwayButton");
        inspectButton = GameObject.Find("InspectButton");
        dropButton = GameObject.Find("DropButton");
    }

    /// <summary>
    /// ShowControls() activates and displays the item controls within the 'item'
    /// page of the notebook
    /// </summary>
    public void ShowControls()
    {
        equipButton.SetActive(true);
        putAwayButton.SetActive(true);
        inspectButton.SetActive(true);
        dropButton.SetActive(true);
    }

    /// <summary>
    /// HideControls() deactivates and hides the item controls within the 'item'
    /// page of the notebook
    /// </summary>
    public void HideControls()
    {
        equipButton.SetActive(false);
        putAwayButton.SetActive(false);
        inspectButton.SetActive(false);
        dropButton.SetActive(false);
    }
}

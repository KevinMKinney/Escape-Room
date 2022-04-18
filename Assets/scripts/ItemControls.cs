using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControls : MonoBehaviour
{
    public Inventory inventory;
    public GameObject equipButton;
    public GameObject putAwayButton;
    public GameObject inspectButton;
    public GameObject dropButton;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        equipButton = GameObject.Find("EquipButton");
        putAwayButton = GameObject.Find("PutAwayButton");
        inspectButton = GameObject.Find("InspectButton");
        dropButton = GameObject.Find("DropButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowControls()
    {
        equipButton.SetActive(true);
        putAwayButton.SetActive(true);
        inspectButton.SetActive(true);
        dropButton.SetActive(true);
    }

    public void HideControls()
    {
        equipButton.SetActive(false);
        putAwayButton.SetActive(false);
        inspectButton.SetActive(false);
        dropButton.SetActive(false);
    }
}

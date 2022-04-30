using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    // attributes:
    GameObject itemDrawing;
    GameObject itemName;
    GameObject itemLongDescription;
    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        itemDrawing = GameObject.Find("ItemDrawing");
        itemName = GameObject.Find("ItemName");
        itemLongDescription = GameObject.Find("ItemLongDescription");
    }

    /// <summary>
    /// UpdateItemDisplay() takes an item in the inventory and dislpays the item
    /// information to the ItemDisplay section of the 'item' page in the notebook
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItemDisplay(Item item)
    {
        if (item == null)
        {
            // somehow, an empty item got passed to the display
            // set the display fields accordingly
            itemName.GetComponent<TMP_Text>().text = "";
            itemLongDescription.GetComponent<TMP_Text>().text = "";
            itemDrawing.SetActive(false);
        } else
        {
            // otherwise, set the display fields with the appropriate item information
            itemName.GetComponent<TMP_Text>().text = item.ItemName;
            itemLongDescription.GetComponent<TMP_Text>().text = item.LongDescription;
            itemDrawing.SetActive(true);
            itemDrawing.GetComponent<Image>().sprite = item.ItemIcon;
        }
    }
}

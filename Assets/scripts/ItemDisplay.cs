using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    GameObject itemDrawing;
    GameObject itemName;
    GameObject itemLongDescription;
    // Start is called before the first frame update
    void Start()
    {
        itemDrawing = GameObject.Find("ItemDrawing");
        itemName = GameObject.Find("ItemName");
        itemLongDescription = GameObject.Find("ItemLongDescription");
    }

    public void UpdateItemDisplay(Item item)
    {
        if (item == null)
        {
            itemName.GetComponent<TMP_Text>().text = "";
            itemLongDescription.GetComponent<TMP_Text>().text = "";
            itemDrawing.SetActive(false);
        } else
        {
            itemName.GetComponent<TMP_Text>().text = item.ItemName;
            itemLongDescription.GetComponent<TMP_Text>().text = item.LongDescription;
            itemDrawing.SetActive(true);
        }
    }
}

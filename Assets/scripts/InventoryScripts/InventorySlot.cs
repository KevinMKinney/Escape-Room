using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    #region attributes
    public Item item;
    public GameObject slot;
    #endregion

    #region constructor
    public InventorySlot(Item item)
    {
        this.item = item;
    }
    #endregion
}

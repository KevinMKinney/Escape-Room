using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{

    #region attributes
    // attributes:
    private string itemName;
    private string itemDescription;
    private Sprite itemIcon;

    #endregion

    #region constructors
    // constructors:
    public Item(string itemName, string itemDescription)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
    }

    public Item(string itemName)
    {
        this.itemName = itemName;
    }

    public Item()
    {
        this.itemName = "";
        this.itemDescription = "";
    }
    #endregion

    #region methods
    // methods:
    string ItemName
    {
        get { return this.itemName; }
        set { this.itemName = value; }
    }

    string ItemDescription
    {
        get { return this.itemDescription; }
        set { this.itemDescription = value; }
    }

    Sprite ItemSprite
    {
        get { return this.ItemSprite; }
        set { this.ItemSprite = value; }
    }
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    #region attributes
    // attributes:
    private string itemName;
    private string shortDescription;
    private string longDescription;
    public Sprite itemIcon;
    public GameObject inGameObject;
    #endregion

    #region constructors
    // constructors:
    public Item(string itemName, string shortDescription)
    {
        this.itemName = itemName;
        this.shortDescription = shortDescription;
        this.longDescription = "NO DESCRIPTION";
    }
    #endregion

    #region methods
    // methods:
    public string ItemName
    {
        get { return this.itemName; }
        set { this.itemName = value; }
    }

    public string ShortDescription
    {
        get { return this.shortDescription; }
        set { this.shortDescription = value; }
    }

    public string LongDescription
    {
        get { return this.longDescription; }
        set { this.longDescription = value; }
    }

    public Sprite ItemSprite
    {
        get { return this.ItemSprite; }
        set { this.ItemSprite = value; }
    }
    #endregion

}

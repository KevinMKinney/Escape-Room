using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region attributes
    // attributes:
    // NOTE: items are serialized here to allow editing within the
    // unity editor window
    [SerializeField]
    private string itemName;

    [SerializeField]
    private string shortDescription;

    [SerializeField]
    private string longDescription;

    [SerializeField]
    private Sprite itemIcon;

    [SerializeField]
    private GameObject inGameObject;

    #endregion

    #region constructors
    // constructors:
    public Item(string itemName, string shortDescription)
    {
        this.itemName = itemName;
        this.shortDescription = shortDescription;
        this.longDescription = "";
    }

    public Item()
    {
        this.itemName = "";
        this.shortDescription = "";
        this.longDescription = "";
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

    public GameObject InGameObject
    {
        get { return this.inGameObject; }
        set { this.inGameObject = value; }
    }
    #endregion

}
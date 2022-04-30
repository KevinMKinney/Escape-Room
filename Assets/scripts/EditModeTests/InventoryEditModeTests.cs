using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// InventoryEditModeTests.cs

public class InventoryEditModeTests
{
    [UnityTest]
    public IEnumerator ItemSlotsEqualMaxItemCount()
    {
        // locate the parent object of the item slots
        GameObject itemList = GameObject.Find("ItemList");

        // test that the number of item slots equals the maximum number of items in the inventory
        Assert.AreEqual(Inventory.maxItemCount, itemList.transform.childCount);
        yield return null;
    }
}

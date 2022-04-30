using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

// InventoryControlTests.cs
public class InventoryControlTests
{
    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    [UnityTest]
    public IEnumerator ItemSlotsEqualMaxItemCount()
    {
        // acceptance test

        // locate the parent object of the item slots
        GameObject itemList = GameObject.Find("ItemList");

        // test that the number of item slots equals the maximum number of items in the inventory
        Assert.AreEqual(Inventory.maxItemCount, itemList.transform.childCount);

        yield return null;
    }

    [UnityTest]
    public IEnumerator EquipItemButtonTest()
    {
        // this is a bottom-up integration test between the Inventory and ButtonControl classes

        // locate the inventory and add an item to it
        Inventory inventory = GameObject.Find("Items").GetComponent<Inventory>();
        inventory.AddItem(new Item());

        // select the added item to the inventory
        inventory.SelectItem(0);

        // locate the equip button and simulate a click
        GameObject.Find("EquipButton").GetComponent<EquipButton>().OnPointerClick(null);

        // test to see if the selected item is now the equipped item
        Assert.AreEqual(inventory.GetSelectedItemIndex(), inventory.GetEquippedItemIndex());

        // drop the item to return to an empty inventory
        inventory.DropItem(0);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PutAwayButtonTest()
    {
        // this is a bottom-up integration test between the Inventory and ButtonControl classes

        // locate the inventory and add an item to it
        Inventory inventory = GameObject.Find("Items").GetComponent<Inventory>();
        inventory.AddItem(new Item());

        // equip the item that was added (equippedItemIndex will now == 0)
        inventory.EquipItem(0);

        // locate the PutAwayButton and simulate a click
        GameObject.Find("PutAwayButton").GetComponent<PutAwayButton>().OnPointerClick(null);

        // test to see if the equipped item is now -1 (indicated no item being currently equipped)
        Assert.AreEqual(-1, inventory.GetEquippedItemIndex());

        // drop the item to return to an empty inventory
        inventory.DropItem(0);

        yield return null;
    }

    [UnityTest]
    public IEnumerator DropButtonTest()
    {
        // this is a bottom-up integration test between the Inventory and ButtonControl classes

        // locate the inventory and add an item to it
        Inventory inventory = GameObject.Find("Items").GetComponent<Inventory>();
        inventory.AddItem(new Item());

        // select the newly added item
        inventory.SelectItem(0);

        // locate the DropButton and simulate a click
        GameObject.Find("DropButton").GetComponent<DropButton>().OnPointerClick(null);

        // test to see if the item count in the inventory is back to 0
        Assert.AreEqual(0, inventory.GetItems().Count);

        yield return null;
    }
}

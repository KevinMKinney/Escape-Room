using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

// InventoryTests.cs

public class InventoryTests
{
    [UnityTest]
    public IEnumerator AddItemToEmptyInventoryTest()
    {
        // acceptance test

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // add the new item
        testInventory.AddItem(new Item());

        yield return new WaitForSeconds(0.1f);

        // make sure that an item was added to the inventory
        Assert.AreEqual(1, testInventory.GetItems().Count);
    }

    [UnityTest]
    public IEnumerator AddItemToFullInventoryTest()
    {
        // acceptance test

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // attempt to add maxItemCount + 1 items to the inventory
        for (int i = 0; i < Inventory.maxItemCount + 1; i++)
        {
            testInventory.AddItem(new Item());
        }

        yield return new WaitForSeconds(0.1f);

        // make sure that only maxItemCount items were added to the inventory
        Assert.AreEqual(Inventory.maxItemCount, testInventory.GetItems().Count);
    }

    [UnityTest]
    public IEnumerator EquipItemBelowBounds()
    {
        // acceptance test

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // attempt to equip an item with a negative index value
        testInventory.EquipItem(-5);

        // make sure that the equippedItemIndex is now -1
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(-1, testInventory.GetEquippedItemIndex());
    }

    [UnityTest]
    public IEnumerator EquipItemAboveBounds()
    {
        // acceptance test

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // add only one item to inventory and then attempt to equip an item at an index greater than 1
        testInventory.AddItem(new Item());
        testInventory.EquipItem(3);

        // make sure that the equippedItemIndex is now -1
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(-1, testInventory.GetEquippedItemIndex());
    }

    [UnityTest]
    public IEnumerator EquipItemInBounds()
    {
        // acceptance test

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // add a single item to the inventory and then attempt to equip the item at index 0
        testInventory.AddItem(new Item());
        testInventory.EquipItem(0);

        // make sure that the equippedItemIndex is now 0
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0, testInventory.GetEquippedItemIndex());
    }

    [UnityTest]
    public IEnumerator DropInBoundsItem_CorrectItemCount()
    {
        // tests DropInBoundsItem_CorrectItemCount(), IEnumerator DropOutOfBoundsItem_CorrectItemCount(),
        // and DropEquippedItem() acheive branch coverage of the Inventory.DropItem() method

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // add max number of items to the inventory
        for (int i = 0; i < Inventory.maxItemCount; i++)
        {
            testInventory.AddItem(new Item());
        }

        // drop an item from the inventory at an in-bounds index
        testInventory.DropItem(0);

        // test that inventory item count is now equal to maxItemCount - 1
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(Inventory.maxItemCount - 1, testInventory.GetItems().Count);
    }

    [UnityTest]
    public IEnumerator DropOutOfBoundsItem_CorrectItemCount()
    {
        // tests DropInBoundsItem_CorrectItemCount(), IEnumerator DropOutOfBoundsItem_CorrectItemCount(),
        // and DropEquippedItem() acheive branch coverage of the Inventory.DropItem() method

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // add max number of items to the inventory
        for (int i = 0; i < Inventory.maxItemCount; i++)
        {
            testInventory.AddItem(new Item());
        }

        // drop an item from the inventory at an out-of-bounds index
        testInventory.DropItem(Inventory.maxItemCount);

        // test that inventory item count still equals the maxItemCount
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(Inventory.maxItemCount, testInventory.GetItems().Count);
    }

    [UnityTest]
    public IEnumerator DropEquippedItem()
    {
        // tests DropInBoundsItem_CorrectItemCount(), IEnumerator DropOutOfBoundsItem_CorrectItemCount(),
        // and DropEquippedItem() acheive branch coverage of the Inventory.DropItem() method

        // create the test inventory
        GameObject inventoryObject = new GameObject();
        inventoryObject.AddComponent<Inventory>();
        Inventory testInventory = inventoryObject.GetComponent<Inventory>();

        // add item and equip it
        testInventory.AddItem(new Item());
        testInventory.EquipItem(0);

        // drop the item
        testInventory.DropItem(0);

        // make sure that the equipped item index is now -1
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(-1, testInventory.GetEquippedItemIndex());

    }
}

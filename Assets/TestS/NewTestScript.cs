using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript // 100 V
{
    // Acceptance Test
    [Test]
    public void TestGetArrayOfHidingSpots()
    {
        int numHidingSpots;
        numHidingSpots = AIWorldHide.Instance.GetHidingSpots().Length;
        Assert.That(numHidingSpots > 0, ""); // Assert that we got Hiding spots
    }

    // Acceptance Test
    [Test]
    public void TestGetArrayOfWaypoints()
    {
        int numWP;
        numWP = AIWorldWP.Instance.GetWaypoints().Length;
        Assert.That(numWP > 0, ""); // Assert that we got Waypoints
    }

    // Acceptance Test
    [Test]
    public void TestGetDictionaryOfStates()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates = states.Count;
        Assert.That(numStates >= 0, ""); // Assert that we got a Dictionary
    }

    // Acceptance Test
    [Test]
    public void TestElementInDictIfAdded()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates1;
        numStates1 = states.Count;
        AIGWorld.Instance.GetWorld().AddState("Hello", 1);
        int newNumStates1;
        newNumStates1 = states.Count;
        Assert.That(newNumStates1 > numStates1, ""); // Assert an Element was added
        bool containsState;
        containsState = AIGWorld.Instance.GetWorld().HasState("Hello");
        Assert.That(containsState == true, ""); // Assert that the state is not present in the dictionary
    }

    // Acceptance Test
    [Test]
    public void TestElementInDictIfNotAdded()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        bool containsState1;
        containsState1= AIGWorld.Instance.GetWorld().HasState("The");
        Assert.That(containsState1 == false, ""); // Assert that the state is not present in the dictionary
    }

    // Acceptance Test
    [Test]
    public void TestAddElementToDict()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates2;
        numStates2 = states.Count;
        AIGWorld.Instance.GetWorld().AddState("We", 1);
        int newNumStates2;
        newNumStates2 = states.Count;
        Assert.That(newNumStates2 > numStates2, ""); // Assert that the state is not present in the dictionary
    }

    // Acceptance Test
    [Test]
    public void TestElementRemovedFromDict()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates4;
        numStates4 = states.Count;
        AIGWorld.Instance.GetWorld().AddState("Beans", 1);
        int newNumStates4;
        newNumStates4 = states.Count;
        Assert.That(newNumStates4 > numStates4, ""); // Assert an Element was added
        AIGWorld.Instance.GetWorld().RemoveState("Beans");
        bool containsState2;
        containsState2 = AIGWorld.Instance.GetWorld().HasState("Beans");
        Assert.That(containsState2 == false, ""); // Assert that the element was removed from the Dict
    }

    // This along with "TestModifyStateInDictIfNotInDict" and "TestModifyStateInDictIfStateIsInDictButValueTooLow" provides full coverage
    // For method <ModifyState> which can be found below
    [Test]
    public void TestModifyStateInDictIfStateIsInDict()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates5;
        numStates5 = states.Count;
        Assert.That(numStates5 >= 0, ""); // Assert that we got a Dictionary
        AIGWorld.Instance.GetWorld().AddState("Other", 1);
        int newNumStates5;
        newNumStates5 = states.Count;
        Assert.That(newNumStates5 > numStates5, ""); // Assert an Element was added
        AIGWorld.Instance.GetWorld().ModifyState("Other", 2);
        Assert.That(states["Other"] == 3); // Assert that the state was modified and the value was increased
    }

    // This along with "TestModifyStateInDictIfStateIsNotInDict" and "TestModifyStateInDictIfStateIsInDictButValueTooLow" provides full coverage
    // For method <ModifyState> which can be found below
    [Test]
    public void TestModifyStateInDictIfStateIsNotInDict()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        AIGWorld.Instance.GetWorld().ModifyState("Beverage", 2);
        bool containsState3;
        containsState3 = AIGWorld.Instance.GetWorld().HasState("Beverage");
        Assert.That(containsState3 == true); // Assert that the state was modified and the key was added
    }

    // This along with "TestModifyStateInDictIfNotInDict" and "TestModifyStateInDictIfStateIsInDict" provides full coverage
    // For method <ModifyState> which can be found below
    [Test]
    public void TestModifyStateInDictIfStateIsInDictButValueTooLow()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates6;
        numStates6 = states.Count;
        Assert.That(numStates6 >= 0, ""); // Assert that we got a Dictionary
        AIGWorld.Instance.GetWorld().AddState("Happy", 1);
        int newNumStates6;
        newNumStates6 = states.Count;
        Assert.That(newNumStates6 > numStates6, ""); // Assert an Element was added
        AIGWorld.Instance.GetWorld().ModifyState("Happy", -6);
        bool containsState4 = AIGWorld.Instance.GetWorld().HasState("Beverage");
        Assert.That(containsState4 == false); // Assert that the state was modified and the key was removed
    }

    /*public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
            if (states[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else
        {
            states.Add(key, value);
        }
    }*/

    // This along with "TestSetStateIfElementIsInNotDict" provide full coverage for Method <SetState> which can be found below
    [Test]
    public void TestSetStateIfElementIsInDict()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        int numStates7 = states.Count;
        Assert.That(numStates7 >= 0, ""); // Assert that we got a Dictionary
        AIGWorld.Instance.GetWorld().AddState("PineApple", 1);
        int newNumStates7 = states.Count;
        Assert.That(newNumStates7 > numStates7, ""); // Assert an Element was added
        AIGWorld.Instance.GetWorld().SetState("PineApple", 7);
        Assert.That(states["PineApple"] == 7, ""); // Assert that the value of PineApple has changed to 7
    }

    // This along with "TestSetStateIfElementIsInDict" provide full coverage for Method <SetState> which can be found below
    [Test]
    public void TestSetStateIfElementIsInNotDict()
    {
        Dictionary<string, int> states;
        states = AIGWorld.Instance.GetWorld().GetStates();
        AIGWorld.Instance.GetWorld().SetState("Job", 7);
        bool containsState5 = AIGWorld.Instance.GetWorld().HasState("Job");
        Assert.That(containsState5 == true, ""); // Assert that Job was added to the Dict
    }

    /*public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }*/
}
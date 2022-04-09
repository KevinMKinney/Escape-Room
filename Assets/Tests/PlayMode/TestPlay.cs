using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
//Method being tested
/*public override void OnTriggerEnter(GuideStateManager Guide, Collider collider)
    {
        //If user enters any of the puzzle triggers, switch to RoomDialogue
        if(collider.gameObject.tag=="spawn"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 1 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 2 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 0 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 3 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
    }*/
public class TestPlay
{   
    //Instantiating variables for classes RoomTrigger and RoomDialogue
    public RoomTrigger TestingR = new RoomTrigger();
    public RoomDialogue TestingD = new RoomDialogue();
    public GivingHint TestingH = new GivingHint();
    public NotTalking TestingT = new NotTalking();
 
    //Branch coverage of the OnTriggerEnter function in RoomTrigger class
    [Test]
    public void TestStateEqualOnTriggerEnter()
    {
        //Making a new GameObject to attach a collider and GuideStateManager 
        GameObject TestObject = new GameObject();
        GuideStateManager Guide = TestObject.AddComponent<GuideStateManager>();
        
        TestObject.AddComponent<BoxCollider>();
        Collider TestCollider = TestObject.GetComponent<BoxCollider>();
        
        //Beginning of branch coverage//

        //Passing in the argument parameters for the function
        TestObject.gameObject.tag="spawn";
        TestingR.OnTriggerEnter(Guide,TestCollider);
        //Testing output of OnTriggerEnter()
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());

        TestObject.gameObject.tag="Puzzle 1 Wait";
        TestingR.OnTriggerEnter(Guide,TestCollider);
        //Testing output of OnTriggerEnter()
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());

        TestObject.gameObject.tag="Puzzle 2 Wait";
        TestingR.OnTriggerEnter(Guide,TestCollider);
        //Testing output of OnTriggerEnter()
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());
        
        TestObject.gameObject.tag="Puzzle 3 Wait";
        TestingR.OnTriggerEnter(Guide,TestCollider);
        //Testing output of OnTriggerEnter()
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());

        TestObject.gameObject.tag="Puzzle 0 Wait";
        TestingR.OnTriggerEnter(Guide,TestCollider);
        //Testing output of OnTriggerEnter()
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());
    }
    //Branch coverage of the OnTriggerEnter function in RoomTrigger class
    [Test]
    public void TestNotEqualOnTriggerEnter(){
        //Making a new GameObject to attach a collider and GuideStateManager 
        GameObject TestObject = new GameObject();
        GuideStateManager Guide = TestObject.AddComponent<GuideStateManager>();
        TestObject.AddComponent<BoxCollider>();
        Collider TestCollider = TestObject.GetComponent<BoxCollider>();
        
        //Beginning of branch coverage//

        //Passing in invalid argument for complete branch coverage
        TestObject.gameObject.tag="Dummy Variable";
        TestingR.OnTriggerEnter(Guide,TestCollider);
        //Testing output of OnTriggerEnter()
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());
    }

    [Test]
    //Integration test, Approach: Big-Bang
    //Since all classes are already made, we're testing two already made classes
    public void TestClasses(){
        GameObject TestObject = new GameObject();
        GuideStateManager Guide = TestObject.AddComponent<GuideStateManager>();
        TestObject.AddComponent<BoxCollider>();
        Collider TestCollider = TestObject.GetComponent<BoxCollider>();
    
        //First integration test
        TestObject.gameObject.tag="spawn";
        TestingT.OnTriggerEnter(Guide,TestCollider);
        //Checking if we've switched from class "NotTalking" to class "RoomDialogue"
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());

        //Second integration test
        TestObject.gameObject.tag="puzzle 1";
        TestingT.OnTriggerEnter(Guide,TestCollider);
        //Checking if we've switched from class "NotTalking" to class "RoomDialogue"
        Assert.AreEqual(TestingD.ToString(),Guide.currentState.ToString());        
    }

    [Test]
    //Acceptance test
    public void TestObjectDestroyed(){
        GameObject TestObject = new GameObject();
        GuideStateManager Guide = TestObject.AddComponent<GuideStateManager>();
        GameObject Test = new GameObject();
        TestObject.AddComponent<BoxCollider>();
        Collider TestCollider = TestObject.GetComponent<BoxCollider>();

        //Checking if object exists
        Assert.IsNotNull(TestObject);
        //Calling destroy method
        TestingD.DestroyGameObject(TestObject);
        //Checking if object is destroyed
        Assert.IsNull(TestObject);
    }

    [Test]
    //Acceptance test
    public void TestHintSet(){
        GameObject TestObject = new GameObject();
        GuideStateManager Guide = TestObject.AddComponent<GuideStateManager>();
        TestObject.AddComponent<BoxCollider>();
        Collider TestCollider = TestObject.GetComponent<BoxCollider>();
        
        //Acceptance test 1
        Assert.AreEqual(300,TestingD.timer);
        //Acceptance test 2
        Assert.AreEqual(0,TestingD.dialogueSet);
        //Acceptance test 3
        Assert.AreEqual(0,TestingD.HintSet);
    }
}

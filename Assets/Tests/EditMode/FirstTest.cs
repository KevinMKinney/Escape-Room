using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

//using GuideStateManager;
public class FirstTest
{
   // public GuideStateManager Testing = new GuideStateManager();
    
    
  //  public GuideStateManager Manager = new GuideStateManager();
    // A Test behaves as an ordinary method
    [Test]
    public void FirstTestSimplePasses()
    {
  /*      testscript count = new testscript();
        GuideStateManager TestingM = new GuideStateManager();
        Debug.Log(TestingM.GetComponent<sometext>());
        
        int newtotal = 1;
        Debug.Log(count.total);
*/
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator FirstTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

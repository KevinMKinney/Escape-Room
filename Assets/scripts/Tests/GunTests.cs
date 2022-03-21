using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;

public class GunTests
{
    /*
     * Fires the gun at from 'position' at 'objectToFireAt'.
     */
    FirePistol FireGun(Vector3 position, Vector3 objectToFireAt)
    {
        //Get the player a disabled movement component.
        GameObject player = GameObject.Find("FPSController");
        foreach (var com in player.GetComponents(typeof(MonoBehaviour)))
            Object.DestroyImmediate(com);
        foreach (var com in player.GetComponents(typeof(Component)))
            if (!com.GetType().Equals(typeof(Transform))) Object.Destroy(com);

        //Sets the position of the player to the correct position.
        player.transform.position = position;
        player.transform.LookAt(objectToFireAt);
        //Make the player camera look at the position to fire at so when the gun is fired it will target the point.
        player.transform.GetChild(0).LookAt(objectToFireAt);

        //Gets the gunscript and sets the position of the gun to the correct position.
        FirePistol fire = MonoBehaviour.FindObjectOfType<FirePistol>();
        fire.transform.parent.position = position;
        //Make the gun look at the point to fire at
        fire.transform.parent.LookAt(objectToFireAt);
        //Start to fire the gun.
        fire.StartCoroutine(fire.GunFire());
        //Return the gun script in case we need it again.
        return fire;
    }

    /*
     * Tests if firing the gun at a ghost will make it mad.
     * Integration test between the gun script and the ghost script. 
     * Bottom-up approach by moving the ghost and the gun to an empty area to do testing there.
     */
    [UnityTest]
    public IEnumerator GunAngersGhost()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get a ghost and move it above everything to prevent anything else form getting in the way.
        GameObject ghost = GameObject.Find("Ghost(Clone)");
        ghost.transform.position = new Vector3(0, 100, 0);

        yield return new WaitForFixedUpdate();
        //Fire the gun at the ghost.
        FireGun(new Vector3(0, 100, 10), ghost.transform.position - new Vector3(0, 0.1f, 0));

        //Wait for the ghost to respond.
        yield return new WaitForSeconds(2);
        //Make sure that the particle system is now showing red particles, which means the ghost script changed the color and is now mad.
        Assert.AreEqual(ghost.GetComponent<ParticleSystem>().main.startColor.color.ToString(), new Color(1, 0, 0, 0.01960784f).ToString());
    }

    /*
     * Tests if the target respond when hit.
     * Integration test between the gun script and the target script. 
     * Bottom-up approach by testing only the target and the gun.
     */
    [UnityTest]
    public IEnumerator GunActivatesTarget()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the target and find the gameobject that when fired at should make the target bend backward.
        GameObject target = GameObject.Find("Main Target").transform.GetChild(1).gameObject;
        //Save the rotation at this point in time.
        Quaternion q = target.transform.parent.rotation;

        //Fire the gun at the target.
        FireGun(target.transform.position + new Vector3(-5, 0, -1), target.transform.position);

        //Wait for the target to respond.
        yield return new WaitForSeconds(0.5f);
        //Make sure the rotation changed, which means the target responded by rotating like it should.
        Assert.AreNotEqual(target.transform.parent, q);
    }

    /*
     * Tests if the gas can will blow up when fired at.
     * Integration test between the gun script and the target script. 
     * Bottom-up approach by moving the gas can and the gun to an empty area to do testing there.
     */
    [UnityTest]
    public IEnumerator GunBlowsUpGasCan()
    {
        // Load the game scene and wait for it to load, again. This cannot be put in a function because it must return something for this function.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the gas can and position it above everything so nothing else can interfere.
        GameObject can = GameObject.Find("Gas Can");
        can.transform.position = new Vector3(0, 100, 0);

        //Wait for the gas can's position to update
        yield return new WaitForFixedUpdate();

        //Fire the gun at the gas can.
        FireGun(new Vector3(0, 100, 10), can.transform.position - new Vector3(0, 0.1f, 0));

        //Wait for the gas can respond.
        yield return new WaitForSeconds(0.5f);
        //Makes sure the gas can have respawned in the house. 
        Assert.Less(can.transform.position.y, 20);
    }

    /*
     * The next four are white box tests for the following method
     * In Class "TargetScriptManager"

    public void FiredAtTarget(TargetColorChange self)
    {
        if (targetColorOrder.Count != 0)
        {
            Material target = targetColorOrder.Pop();
            if (self.GetComponent<MeshRenderer>().sharedMaterial != target)
            {
                ResetTarget();
                onReset();
            } else if (targetColorOrder.Count == 0)
            {
                gameObjectToEnable.SetActive(true);
                onReset = () => { };
            }
        }
    }
    */

    /*
     *  TargetScriptManagerActivatesGameObject, TargetsAreNotResetWhenHitInOrder, ResetsTargetsWhenTargetsAreFiredOutOfOrder, TargetScriptWorksWhenTheStackOfTargetsAreEmpty
     *  together they achieve 100% path coverage.
     */

    /*
     * Test if the above script activates the gameobject when all the targets have been hit.
     */
    [UnityTest]
    public IEnumerator TargetScriptManagerActivatesGameObject()
    {
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the script that has the function we are testing.
        TargetScriptManager manager = Object.FindObjectOfType<TargetScriptManager>();

        //Tell the script that the targets have been fired at in the correct order by looking at what color is next on the stack.
        while (manager.targetColorOrder.Count > 0)
        {
            manager.FiredAtTarget(manager.targetColorOrder.Peek());
        }
        //Make sure that the gameobject is now enabled.
        Assert.IsTrue(manager.gameObjectToEnable.activeInHierarchy, "Manager Did Not Turn On The GameObject: " + manager.gameObjectToEnable);
    }
    /*
     * Test if the function does not reset the targets when the targets are hit in the correct order.
     */
    [UnityTest]
    public IEnumerator TargetsAreNotResetWhenHitInOrder()
    {
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the script to test.
        TargetScriptManager manager = Object.FindObjectOfType<TargetScriptManager>();

        //Make a flag variable and subscribe a function to the OnReset event on the script manager that will set this flag to true.
        //This allows us to see if the event was fired.
        bool resetFlag = false;
        void OnReset()
        {
            resetFlag = true;
        }
        //This is one of the reasons this is a white box test, we manipulate and look at the variables in the script.
        manager.onReset += OnReset;

        //Continue hitting the targets in order until the stack is empty.
        while (manager.targetColorOrder.Count > 0)
        {
            //Get the next color that needs to be hit.
            Material nextColor = manager.targetColorOrder.Peek();
            //Tell the script that the correct target was hit.
            manager.FiredAtTarget(nextColor);
            //After that function call, if the flag is true, then the OnReset event was fired. That is not good because OnReset should not be called if the targets were hit in the correct order.
            Assert.IsFalse(resetFlag, "OnReset Called When The Correct Material Was Given\n Materail: " + nextColor);
        }
    }
    /*
     * Test if the targets are reset if the targets are fired out of order.
     */
    [UnityTest]
    public IEnumerator ResetsTargetsWhenTargetsAreFiredOutOfOrder()
    {
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the script to test.
        TargetScriptManager manager = Object.FindObjectOfType<TargetScriptManager>();

        //Make a flag variable and subscribe a function to the OnReset event on the script manager that will set this flag to true.
        //This allows us to see if the event was fired.
        bool resetFlag = false;
        void OnReset()
        {
            resetFlag = true;
        }
        //Subscribe to event.
        manager.onReset += OnReset;

        //Get the correct target for debugging.
        Material temp = manager.targetColorOrder.Peek();
        //Get the second element in the stack and tell the script that the target was fired at.
        manager.FiredAtTarget(manager.targetColorOrder.ElementAt(1));
        //After the incorrect target was passed to the script, the flag should be true because OnReset should have been called.
        Assert.IsTrue(resetFlag, "OnReset Not Called When The Incorrect Material Was Given\n Given Materail: " + manager.targetColorOrder.ElementAt(2) + "\n Expected:" + temp);
    }
    /*
     * The last branch to test. In the function we can see if the stack is empty and none of the code inside should execute. 
     * If the code were to execute, an exception would be thrown because the stack would be empty.
     */
    [UnityTest]
    public IEnumerator TargetScriptWorksWhenTheStackOfTargetsAreEmpty()
    {
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the script to test.
        TargetScriptManager manager = Object.FindObjectOfType<TargetScriptManager>();

        //Store a material to test later.
        Material material = manager.targetColorOrder.Peek();

        //Tell the script that the targets have been fired at in the correct order by looking at what color is next on the stack.
        while (manager.targetColorOrder.Count > 0)
        {
            manager.FiredAtTarget(manager.targetColorOrder.Peek());
        }
        //Make sure the stack is empty like it should be.
        Assert.IsTrue(manager.targetColorOrder.Count == 0);

        //After the stack is empty, calling the function again should not throw an exception.
        Assert.DoesNotThrow(() => manager.FiredAtTarget(material));
    }
}

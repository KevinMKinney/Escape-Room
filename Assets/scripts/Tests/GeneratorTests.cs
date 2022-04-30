using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GeneratorTests
{
    /*
     * Black box test. Tests if the generator shakes when turned on.
     */
    [UnityTest]
    public IEnumerator GeneratorShakes()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the generator script.
        Generator g = MonoBehaviour.FindObjectOfType<Generator>();

        //Get the position of the body (the part that will shake).
        Vector3 lastPosition = g.transform.GetChild(1).position;

        //Start the generator.
        g.hasGas = true;
        g.StartGenerator();

        //Give the generator a chance to start shaking.
        yield return new WaitForSeconds(0.1f);
        //Check if the position of the generator body is in a different place than when we recorded it earlier.
        Assert.AreNotEqual(lastPosition, g.transform.GetChild(1).position);
    }

    /*
     * Black box test. Tests if the generator moves the door up within 20 seconds of turning on.
     */
    [UnityTest]
    public IEnumerator GeneratorOpensDoor()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the generator script.
        Generator g = MonoBehaviour.FindObjectOfType<Generator>();

        //Store the current position of the door that the generator will move and add its length to it. The door must move above this height for the test to succeed.
        float doorPosition = g.door.transform.position.y + g.door.transform.localScale.y;

        //Start the generator.
        g.hasGas = true;
        g.StartGenerator();

        //Wait for the door to move.
        yield return new WaitForSeconds(20);
        //Check that the door has moved up.
        Assert.GreaterOrEqual(g.transform.position.y, doorPosition);
    }
    /*
     * Black box test. Tests if the generator turns on when it has gas.
     */
    [UnityTest]
    public IEnumerator GeneratorTurnsOnWhenItHasGas()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the generator script.
        Generator g = MonoBehaviour.FindObjectOfType<Generator>();

        //Get the position of the body (the part that will shake).
        Vector3 position = g.transform.GetChild(1).position;

        //Start the generator.
        g.hasGas = true;
        g.StartGenerator();

        //Give the generator a chance to start shaking.
        yield return new WaitForSeconds(0.1f);
        //Check if the position of the generator body is in a different place than when we recorded it earlier. That means that the generator turned on.
        Assert.False(g.transform.GetChild(1).position == position);
    }
    /*
     * Black box test. Tests if the generator does not turn on when it has gas.
     */
    [UnityTest]
    public IEnumerator GeneratorDoesNotTurnOnWhenItDoesNotHaveGas()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the generator script.
        Generator g = MonoBehaviour.FindObjectOfType<Generator>();

        //Get the position of the body (the part that will shake).
        Vector3 lastPosition = g.transform.GetChild(1).position;

        //Try to start the generator without gas.
        g.StartGenerator();

        //Give the generator a chance to start shaking if it does.
        yield return new WaitForSeconds(0.1f);
        //Make sure the generator has not started. 
        Assert.False(g.hasStarted);
        //Check if the position of the generator body is not in a different place than when we recorded it earlier.
        Assert.AreEqual(lastPosition, g.transform.GetChild(1).position);
    }

    /*
     * Black box test. Tests if the generator does not turn on again when it is on.
     */
    [UnityTest]
    public IEnumerator GeneratorDoesNotTurnOnAfterStarting()
    {
        // Load the game scene and wait for it to load.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        //Get the generator script.
        Generator g = MonoBehaviour.FindObjectOfType<Generator>();

        //Get the position of the body (the part that will shake).
        Vector3 position = g.transform.GetChild(1).position;

        //Set the generator to have gas but has already started.
        g.hasGas = true;
        g.hasStarted = true;

        //Try to start the generator.
        g.StartGenerator();
        //Give the generator a chance to start shaking if it does.
        yield return new WaitForSeconds(0.1f);
        //Make sure the generator did not start again by checking if it started shaking.
        Assert.True(g.transform.GetChild(1).position == position);

        // Try to start it again without gas.
        g.hasGas = false;
        g.StartGenerator();

        //Give the generator a chance to start shaking if it does.
        yield return new WaitForSeconds(0.1f);
        //The generator should still not have started shaking.
        Assert.True(g.transform.GetChild(1).position == position);
    }
}

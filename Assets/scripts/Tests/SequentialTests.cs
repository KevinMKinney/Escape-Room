using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SequentialTests
{
    [UnityTest]
    public IEnumerator ExitDoorMovesToNextSceneWhenOpened()
    {
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        ExitDoor script = Object.FindObjectOfType<ExitDoor>();
        script.key.SetActive(true);
        script.key.GetComponent<pick_up>().pickedup = 2;
        script.doremode = 1;

        yield return new WaitForSeconds(2);
        Assert.AreEqual(2, SceneManager.GetActiveScene().buildIndex);
    }

    [UnityTest]
    public IEnumerator TransitionSceneMovesToNextScene()
    {
        SceneManager.LoadScene(2);
        yield return new WaitForSeconds(4.5f);
        Assert.AreEqual(3, SceneManager.GetActiveScene().buildIndex);
    }
}

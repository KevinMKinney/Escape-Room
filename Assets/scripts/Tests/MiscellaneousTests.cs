using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;

public class MiscellaneousTests
{
    [UnityTest]
    public IEnumerator ItemsRespawnWhenInVoid()
    {
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(1);

        GameObject gun = GameObject.Find("Gun");
        gun.transform.position = new Vector3(200, 0, 200);

        yield return new WaitForSeconds(3);
        Assert.Less(gun.transform.position.x, 100);
        Assert.Less(gun.transform.position.z, 100);
    }
}

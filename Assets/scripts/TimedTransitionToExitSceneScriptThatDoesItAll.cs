using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TimedTransitionToExitSceneScriptThatDoesItAll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("exit");
    }

    // Update is called once per frame
    IEnumerator exit()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Outside");
    }
}

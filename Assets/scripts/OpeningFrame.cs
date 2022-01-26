using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningFrame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(opening());
    }

   IEnumerator opening()
    {
        //yield return new WaitForSeconds(28);
        while (!Input.GetMouseButton(0))
        {
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("Game");
    }
}

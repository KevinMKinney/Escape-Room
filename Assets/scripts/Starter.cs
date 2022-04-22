using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Starter : MonoBehaviour
{
    float fadeOutSpeed = 1;
    bool hasTextOnScreen;
    Text genText;

    // Start is called before the first frame update
    void Start()
    {
        genText = FindObjectOfType<Text>();
    }

    // Update is called once per frame
    public void Clicked()
    {
        Generator generator = transform.parent.parent.GetComponent<Generator>();
        if (generator.hasGas) 
            generator.StartGenerator();
        else
        if (!hasTextOnScreen)
        {
            Color color = genText.color;
            color.a = 1;
            genText.color = color;
            genText.text = "The generator needs gasoline";
            StartCoroutine("StopText");
            hasTextOnScreen = true;

        }

    }

    IEnumerator StopText()
    {
        yield return new WaitForSeconds(3);
        while (genText.color.a > 0.001)
        {
            Color color = genText.color;
            color.a /= fadeOutSpeed / 100 + 1;
            genText.color = color;
            yield return new WaitForEndOfFrame();
        }
        hasTextOnScreen = false;
    }
}

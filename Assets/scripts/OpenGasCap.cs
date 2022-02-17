using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OpenGasCap : MonoBehaviour
{

    float fadeOutSpeed = 1;
    bool hasTextOnScreen;
    Text genText;
    public float openSpeed = 0.1f;
    public float openAngle = 90;
    bool gasCanOpen;

    // Start is called before the first frame update
    void Start()
    {
        genText = FindObjectOfType<Text>();
    }

    // Update is called once per frame
    public void Clicked()
    {

        pick_up gasCan = null;
        foreach (pick_up islock in FindObjectsOfType<pick_up>())
        {
            if (islock.pickedup == 2)
            {
                gasCan = islock;
            }
        }
        if ((gasCan == null || gasCan.gameObject.name != "Gas Can") && !gasCanOpen)
        {
            if (!hasTextOnScreen)
            {
                Color color = genText.color;
                color.a = 1;
                genText.color = color;
                genText.text = "Need to find gas can";
                StartCoroutine("StopText");
                hasTextOnScreen = true;
            }
        }
        else if (!gasCanOpen)
        {
            StartCoroutine("FillGenWithGas");
            gasCanOpen = true;
            transform.parent.parent.parent.GetComponent<Generator>().hasGas = true;
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

    IEnumerator FillGenWithGas()
    {

        for (int i = 0; i < openAngle / openSpeed; i++)
        {

            Vector3 pos = transform.parent.eulerAngles;
            pos.x += openSpeed;
            transform.parent.eulerAngles = pos;
            yield return new WaitForEndOfFrame();
        }

    }

}

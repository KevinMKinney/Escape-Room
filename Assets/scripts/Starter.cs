using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Written this semester.
 * This script is placed on the starter cord GameObject on the generator. 
 * It tells the gernerator to start when clicked on if the generator has gas.
 * However it displays a message if clicked on while the generator does not has gas.
 */
public class Starter : MonoBehaviour
{
    //How fast the message will fade out.
    float fadeOutSpeed = 1;

    bool hasTextOnScreen;

    //The UI Text element that will be used to display the message.
    Text genText;

    //When the game starts, set the text element to the only on in the scene.
    void Start()
    {
        genText = FindObjectOfType<Text>();
    }

    //Clicked is called when the player's crosshair is on the GameObject and the left mouse button is pressed.
    public void Clicked()
    {
        //Get the generator script.
        Generator generator = transform.parent.parent.GetComponent<Generator>();
        if (generator.hasGas)
            generator.StartGenerator();
        else if (!hasTextOnScreen)
        {
            //Since no text is being displayed, display a message and start the fade out annimation. 
            Color color = genText.color;
            color.a = 1;
            genText.color = color;
            genText.text = "The generator needs gasoline";
            StartCoroutine("FadeOut");

            hasTextOnScreen = true;
        }

    }

    //Shows text for three seconds then slowly removes the text.
    IEnumerator FadeOut()
    {
        //Wait three seconds
        yield return new WaitForSeconds(3);
        //While the text is not transparent, loop.
        while (genText.color.a > 0.001)
        {
            //Set the transparency of the text to be a quotient of last frames. This creates exponental decay for the transparncy of the text.
            Color color = genText.color;
            color.a /= fadeOutSpeed / 100 + 1;
            genText.color = color;
            //Wait for the next frame before continuing.
            yield return new WaitForEndOfFrame();
        }
        //It no longer has text on the screen, so update this aiable.
        hasTextOnScreen = false;
    }
}

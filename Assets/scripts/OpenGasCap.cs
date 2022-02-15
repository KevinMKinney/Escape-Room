using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Written this semester.
 * This script is placed on the gas cap GameObject on the generator. 
 * It plays an opening annimation when clicked on while the player is holding the gas can. It will also tell the generator script hat it has gas.
 * However it displays a message if clicked on while the player is not holding the gas can.
 */
public class OpenGasCap : MonoBehaviour
{
    //How fast the message will fade out.
    float fadeOutSpeed = 1;
    bool hasTextOnScreen;
    //The UI Text element that will be used to display the message.
    Text genText;

    //Open animation speeds
    public float openSpeed = 0.1f;
    public float openAngle = 90;

    //Is the gas cap open?
    bool gasCanOpen;

    //When the game starts, set the text element to the only on in the scene.
    void Start()
    {
        genText = FindObjectOfType<Text>();
    }

    //Clicked is called when the player's crosshair is on the GameObject and the left mouse button is pressed.
    public void Clicked()
    {
        //Represents the current object being held by the player.
        pick_up gasCan = null;
        //Loop though all items that are picked up and find the one being held.
        foreach (pick_up islock in FindObjectsOfType<pick_up>())
        {
            //The item is held
            if (islock.pickedup == 2)
            {
                //Save the item
                gasCan = islock;
            }
        }

        if ((gasCan == null || gasCan.gameObject.name != "Gas Can") && !gasCanOpen)
        {
            if (!hasTextOnScreen)
            {
                //If the player does not have the Gas Can then display a mesage.
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
            //If the player is holding an object and it is the Gas Can and it is not already open or opening, than start the openng annimation and tell the generator that it is full of gas.
            StartCoroutine("FillGenWithGas");
            gasCanOpen = true;
            transform.parent.parent.parent.GetComponent<Generator>().hasGas = true;
        }


    }

    //Shows text for three seconds then slowly removes the text.
    IEnumerator StopText()
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

    //The opening annimation
    IEnumerator FillGenWithGas()
    {
        //Loop until the angle opened is the same as openAngle
        for (int i = 0; i < openAngle / openSpeed; i++)
        {
            //Rotate the x axis of rotation on the hinge to make it look like it is opening.
            Vector3 pos = transform.parent.eulerAngles;
            pos.x += openSpeed;
            transform.parent.eulerAngles = pos;

            //Wait for the next frame before continuing.
            yield return new WaitForEndOfFrame();
        }
    }

}

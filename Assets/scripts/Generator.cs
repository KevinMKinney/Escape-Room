using System.Collections;
using UnityEngine;

/* 
 * Written this semester.
 * This script is placed on the generator GameObject. When it is told to Start by other scipts it will:
 * 1. Cause the generator to shake violently, like all good generators do.
 * 2. Open an elecric door
 * 3. Prevent the generator from starting agian.
 */
public class Generator : MonoBehaviour
{
    //The door object to open.
    public GameObject door;

    [HideInInspector]
    //Is the geneator running? Has it already started?
    public bool hasStarted = false;

    [HideInInspector]
    //This bool is not used in this script, however other scripts will change and get this value.
    public bool hasGas = false;

    //The maximum range the generator can shake. This value is actually doubled since the generator shakes from -shakeRange to shakeRange.
    public Vector3 shakeRange = new Vector3(0.1f, 0.1f, 0.1f);
    //How many frames will pass before it chooses an new location to move, when shaking.
    //This way we can control how fast it shakes. The higher thae value the slower it gets.
    public int speed = 2;

    //Called when a script wants the generator to start.
    public void StartGenerator()
    {
        if (!hasStarted && hasGas)
        {
            // 1, Starts the shaking annimation
            StartCoroutine("StartShaking");
            // 2, Opens the door by moving it up.
            door.transform.position += new Vector3(0, door.transform.lossyScale.y, 0);
            // 3, Sets this bool to true so the script knows that it cannot start agian.
            hasStarted = true;
        }
    }

    IEnumerator StartShaking()
    {
        //Get the part of the generator that will shake.
        GameObject main = transform.GetChild(1).gameObject;
        //Saves the origanal position of the part that shakes.
        Vector3 vector3 = main.transform.position;
        //Saves the position it came from, lastPosition, and the position it is going, newPosition. These are in local space and usally very small.
        Vector3 lastPosition = Vector3.zero;
        Vector3 newPosition = Vector3.zero;
        //Acts as a counter
        int i = speed;
        //When started this annimation will continure forever until the application shuts down, or you know, crashes.
        while (true)
        {
            //Wait for the next frame every time the loop 
            yield return new WaitForEndOfFrame();
            //The counter will increase by one every frame.
            i++;
            if (i >= speed)
            {
                //When the counter is more than "speed" a new location to travel to is chossen and the counter is reset.
                //Save the position we are at now so we know were we came from.
                lastPosition = newPosition;

                //Choose random values for x, y, and z
                newPosition = Random.insideUnitSphere;
                newPosition.x *= shakeRange.x;
                newPosition.y *= shakeRange.y;
                newPosition.z *= shakeRange.z;

                //Reset counter
                i = 0;
            }
            else
            {
                //If i is less than speed, than this frame we interpolate from last position to the new position, using the counter to descide how close we are to the target position.
                main.transform.position = vector3 + Vector3.Lerp(lastPosition, newPosition, (float)i / speed);
            }
        }
    }
}

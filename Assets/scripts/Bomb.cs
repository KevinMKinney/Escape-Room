using UnityEngine;
using CustomEventKit;
using System.Collections.Generic;

/* 
 * Written this semester,  starting at line 14.
 * This script represents a bomb in unity. It can be activated when the user presses e and is holding the bomb.
 */
//This one line was generated by unity
public class Bomb : MonoBehaviour
{
    //The scriptable object referance.
    public List<BombScriptableObject> availableScriptableObjects;
    BombScriptableObject bombScriptableObject;
    //The pick_up script atteched to this gameobject.
    public pick_up pickUpScript = null;

    private void Start()
    {
        bombScriptableObject = availableScriptableObjects.RandomElement();
        //Set this variable to the pick_up script on this gameobject
        pickUpScript = GetComponent<pick_up>();
        //Subscribe an anonymous function to listen for the result of the event.
        bombScriptableObject.explodeEvent.AddResultListener((bomb, cancelableObject) =>
        {
            //If the Bomb that is exploding is itself, then destroy the gameobject.
            CancelableState state = cancelableObject.GetState();
            //If the Bomb that is exploding is itself, and no one on the event listener stack objects to exploding, then destroy the gameobject.
            if (bomb == this && (state == CancelableState.Default || state == CancelableState.Allow))
            {
                Destroy(gameObject);
            }
        });
    }

    private void Update()
    {
        //Every frame check if the right mosue button is down.
        if (Input.GetMouseButton(1))
        {
            //If e is pressed then fire the exploding event with itself.
            bombScriptableObject.InvokeExplodeEvent(this);
        }
    }
}

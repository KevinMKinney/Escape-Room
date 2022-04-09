using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventKit;

/* 
 * Written this semester.
 * This scriptable object acts as a data container for the Bomb script. It allows the player to become invisible when the event is fired.
 */
//This line allows an instance to be created in the unity editor by right clicking-> Create -> ScriptableObjects -> Invisable Data Scriptable Object.
[CreateAssetMenu(fileName = "InvisableDataScriptableObject", menuName = "ScriptableObjects/Invisable Data Scriptable Object", order = 1)]
public class InvisibleScriptableObject : BombScriptableObject
{
    //The time the player can be invisible for.
    [SerializeField]
    float InvisibleTime = 7;
    //Answers the question: Is the player invisible?
    [HideInInspector]
    public bool isInvisible = false;

    //Called at the start of the game by unity. Overriding BombScriptableObject's OnEnable.
    public override void OnEnable()
    {
        //Scriptable objects do not reset their values, so make sure this is variable is set to false.
        isInvisible = false;
        //Call the base class's OnEnable function.
        base.OnEnable();
    }

    //We need to add our own event handlers so override this function from BombScriptableObject.
    public override void AddEventHandlers()
    {
        //Add BombScriptableObject's event handlers.
        base.AddEventHandlers();
        //Listen for the result of the event.
        explodeEvent.AddResultListener((timeBomb, cancelableObject) =>
        {
            //Get the state of the event.
            CancelableState state = cancelableObject.GetState();
            //If no one the event listener stack objects to making the player invisible, then do so.
            if (state == CancelableState.Default || state == CancelableState.Allow)
            {
                //Get an instance of a MonoBehaviour to start the coroutine MakePlayerInvisible. ScriptableObjects do not StartCoroutine defined in the class hierarchy.
                TargetScriptManager.instance.StartCoroutine(MakePlayerInvisible());
            }
        });

        //The event must be canceled if the player is already invisible.
        explodeEvent += (timeBomb, cancelableObject) =>
        {
            if (isInvisible)
            {
                //Refuse to allow the item to be used.
                cancelableObject.SetState(CancelableState.Deny);
            }
        };
    }

    //Makes the player invisible for a set amount of time.
    IEnumerator MakePlayerInvisible()
    {
        //Get the player.
        GameObject player = GameObject.FindWithTag("Player");
        //Get the invisible script.
        Invisible invisibleScript = player.GetComponent<Invisible>();
        //Update bools to the state of invisibility.
        isInvisible = true;
        invisibleScript.isInvisible = true;
        //Wait for InvisibleTime to make the player visible again.
        yield return new WaitForSeconds(InvisibleTime);
        //Update bools to the state of invisibility.
        invisibleScript.isInvisible = false;
        isInvisible = false;
    }
}

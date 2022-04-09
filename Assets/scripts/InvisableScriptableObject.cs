using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventKit;

/* 
 * Written this semester.
 * This scriptable object acts as a data container for the Bomb script. It allows the player to become invisable when the event is fired.
 */
//This line allows an instance to be created in the unity editor by right clicking-> Create -> ScriptableObjects -> Invisable Data Scriptable Object
[CreateAssetMenu(fileName = "InvisableDataScriptableObject", menuName = "ScriptableObjects/Invisable Data Scriptable Object", order = 1)]
public class InvisableScriptableObject : BombScriptableObject
{
    //The time the player can be invisable for.
    [SerializeField]
    float InvisableTime = 7;
    //Answers the question : Is the player invisable?
    [HideInInspector]
    public bool isInvisable = false;

    //Called at the start of the game by unity. Overrieding BombScriptableObject's OnEnable.
    public override void OnEnable()
    {
        //Scriptable objects do not reset thair values, so make sure this is variable is set to false.
        isInvisable = false;
        //Call the base class's OnEnable function.
        base.OnEnable();
    }

    //We need to add our own event handlers so override this function from BombScriptableObject.
    public override void AddEventHandlers()
    {
        //Add BombScriptableObject's event handlers
        base.AddEventHandlers();
        //Listen for the result of the event.
        explodeEvent.AddResultListener((timeBomb, cancelableObject) =>
        {
            //Get the state of the event.
            CancelableState state = cancelableObject.GetState();
            //If no one the event listener stack objects to making the player invisable then do so.
            if (state == CancelableState.Default || state == CancelableState.Allow)
            {
                //Get a instance of a MonoBehaviour to start the coroutine MakePlayerInvisable. ScriptableObjects do not StartCoroutine defined in the class hierarchy.
                TargetScriptManager.instance.StartCoroutine(MakePlayerInvisable());
            }
        });

        //The event must be canceled if the player is already invisable.
        explodeEvent += (timeBomb, cancelableObject) =>
        {
            if (isInvisable)
            {
                //Refuse to allow the item to be used.
                cancelableObject.SetState(CancelableState.Deny);
            }
        };
    }

    //Makes the player ivisable for a set amount of time.
    IEnumerator MakePlayerInvisable()
    {
        //Get the player
        GameObject player = GameObject.FindWithTag("Player");
        //Get the invisable script
        Invisable invisableScript = player.GetComponent<Invisable>();
        //Update bools to the state of invisablility.
        isInvisable = true;
        invisableScript.isInvisable = true;
        //Wait for InvisableTime to make the player visable again.
        yield return new WaitForSeconds(InvisableTime);
        //Update bools to the state of invisablility.
        invisableScript.isInvisable = false;
        isInvisable = false;
    }
}

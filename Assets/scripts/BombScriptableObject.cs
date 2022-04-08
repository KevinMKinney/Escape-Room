using UnityEngine;
using CustomEventKit;

/* 
 * Written this semester.
 * This scriptable object acts as a data container for the Bomb script. It also manages bomb events.
 */
public class BombScriptableObject : ScriptableObject
{
    //The explosion event. It is cancelable becuase something may want to prevent the bomb from actually exploding. 
    public CancelableEvent<Bomb> explodeEvent;

    //Called at the start of the game by unity.
    public virtual void OnEnable()
    {
        //Set the explode event to a new Cancelable Event.
        explodeEvent = new CancelableEvent<Bomb>();
        //Add any event handlers to the explode event. Setting up is mainly split into two functions for inherited subclasses.
        AddEventHandlers();
    }

    //Adds Any event handlers we may need.
    public virtual void AddEventHandlers()
    {
        //The generic check. The bomb must be held in the hand. Since this function can be overriden this does not have to be the case for other subclasses.
        explodeEvent += (bomb, c) =>
        {
            //If the pickedup vaiable is not 2 then it must not be held.
            if (bomb.pickUpScript.pickedup != 2)
            {
                //Request that the event be canceled and that the bomb does not explode.
                c.SetState(CancelableState.Deny);
            }
        };
    }

    //A function for Invoking the explode event. Called when a Bomb script want to explode, or somthing else wants a bomb to explode.
    //Returns a CancelableObject that contains the result of the event.
    public virtual CancelableObject InvokeExplodeEvent(Bomb bomb)
    {
        //Create a new CancelableObject to pass the the event.
        CancelableObject cancelableObject = new();
        //Invoke the event with the CancelableObject and bomb argument
        explodeEvent.Invoke(bomb, cancelableObject);
        //Return the state of the event.
        return cancelableObject;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Written this semester.
 * This namespace contains helpful classes for events.
 */
namespace CustomEventKit
{
    //An event that has listeners and can be invoked to call all the listeners.
    public class CustomEvent<T, T2>
    {
        //Calls all the listeners.
        public virtual void Invoke(T t, T2 t2)
        {
            //Loop over each listener in the listeners array and call it.
            foreach (var listener in listeners)
            {
                listener(t, t2);
            }
        }
        //Holds all the listeners.
        public List<System.Action<T, T2>> listeners;
        //Custom operator. This class can be 'added' to a function to subscribe the function to the event.
        public static CustomEvent<T, T2> operator +(CustomEvent<T, T2> c, System.Action<T, T2> function)
        {
            //Add the listener to the listeners list.
            c.AddListener(function);
            //Return the custom event so += can be used.
            return c;
        }

        //Custom operator. This class can be 'subtracted' to a function to unsubscribe the function to the event.
        public static CustomEvent<T, T2> operator -(CustomEvent<T, T2> c, System.Action<T, T2> function)
        {
            //Remove the listener to the listeners list.
            c.RemoveListener(function);
            //Return the custom event so -= can be used.
            return c;
        }

        //Add a listener to the event.
        public virtual void AddListener(System.Action<T, T2> function)
        {
            //Add the listener to the listeners list.
            listeners.Add(function);
        }

        public virtual void RemoveListener(System.Action<T, T2> function)
        {
            listeners.Remove(function);
        }

        public CustomEvent()
        {
            //Instantiate an empty listener array when a new CustomEvent is created.
            listeners = new List<System.Action<T, T2>>();
        }
    }

    //Define states for a CancelableObject.
    public enum CancelableState
    {
        Default,
        Allow,
        Deny
    }

    //Holds a CancelableState. It is passed with cancelable event so listeners can change the state and decide if the event firer should continue with its action.
    public class CancelableObject
    {
        //The state the event is in. Starts with Default.
        CancelableState state = CancelableState.Default;
        //Getter for the state value.
        public CancelableState GetState()
        {
            return state;
        }
        //Setter for the state value.
        public void SetState(CancelableState newState)
        {
            state = newState;
        }
    }

    //A event that has listeners for the event and listeners for after all of the normal listers have been called.
    //Normal listeners can object or agree that the event's firer should continue with the action.
    public class CancelableEvent<T> : CustomEvent<T, CancelableObject>
    {
        //The event for after all of the normal listers have been called. The original object and CancelableObject is passed in.
        CustomEvent<T, CancelableObject> onEnd;
       
        //Calls all the listeners and fires the onEnd event with the result of the listeners.
        public override void Invoke(T t, CancelableObject cancelableObject)
        {
            //The index of the current listener.
            int index = 0;
            //Loop until there are no more listeners or the CancelableObject's state is Deny.
            while (cancelableObject.GetState() != CancelableState.Deny && index < listeners.Count)
            {
                //Call the listener at index int the listener's array.
                listeners[index](t, cancelableObject);
                index++;
            }
            //Invoke the onEnd event with the CancelableObject and the original object t.
            onEnd.Invoke(t, cancelableObject);
        }

        //Custom operator. This class can be 'added' to a function to subscribe the function to the event.
        public static CancelableEvent<T> operator +(CancelableEvent<T> c, System.Action<T, CancelableObject> function)
        {
            //Add the listener to the listeners list.
            c.AddListener(function);
            //Return the cancelable event so += can be used.
            return c;
        }

        //Custom operator. This class can be 'subtracted' to a function to unsubscribe the function to the event.
        public static CancelableEvent<T> operator -(CancelableEvent<T> c, System.Action<T, CancelableObject> function)
        {
            //Remove the listener to the listeners list.
            c.RemoveListener(function);
            //Return the custom event so -= can be used.
            return c;
        }

        //Add a listener to the end of the event, after all of the normal listeners have been called.
        public void AddResultListener(System.Action<T, CancelableObject> function)
        {
            //If the onEnd event is null create one.
            if (onEnd == null) onEnd = new CustomEvent<T, CancelableObject>();
            //Add the function to the event.
            onEnd += function;
        }

        //Remove a listener to the end of the event, after all of the normal listeners have been called.
        public void RemoveResultListener(System.Action<T, CancelableObject> function)
        {
            //If the onEnd event is null then return early.
            if (onEnd == null) return;
            //Add the function to the event.
            onEnd -= function;
        }
    }
}
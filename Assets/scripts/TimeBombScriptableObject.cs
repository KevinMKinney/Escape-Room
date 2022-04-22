using System.Collections;
using UnityEngine;
using CustomEventKit;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;

/* 
 * Written this semester.
 * This scriptable object acts as a data container for the Bomb script. It manages bomb events and slows down time when a bomb successfully explodes.
 */
//This line allows an instance to be created in the unity editor by right clicking-> Create -> ScriptableObjects -> Time Bomb Scriptable Object.
[CreateAssetMenu(fileName = "TimeBombScriptableObject", menuName = "ScriptableObjects/Time Bomb Scriptable Object", order = 1)]
public class TimeBombScriptableObject : BombScriptableObject
{
    //This curve represents how slow time is over time. All values are expected to be 0 (Not time passing) to 1 (Time passing normally).
    public AnimationCurve timeCurve;
    //Answers the question : Is time being slowed down?
    bool isTimeSlow = false;

    //Called at the start of the game by unity. Overriding BombScriptableObject's OnEnable.
    public override void OnEnable()
    {
        //Scriptable objects do not reset their values, so make sure this is variable is set to false.
        isTimeSlow = false;
        //Call the base class's OnEnable function.
        base.OnEnable();
    }

    //We need to add our own event handlers so override this function from BombScriptableObject.
    public override void AddEventHandlers()
    {
        //Add BombScriptableObject's event handlers.
        base.AddEventHandlers();
        //Listen for the result of the explosion event.
        explodeEvent.AddResultListener((timeBomb, cancelableObject) =>
        {
            //Get the state of the event.
            CancelableState state = cancelableObject.GetState();
            //If no one the event listener stack objects to exploding, slow down time.
            if (state == CancelableState.Default || state == CancelableState.Allow)
            {
                //Get an instance of a MonoBehaviour to start the coroutine SlowDownTime. ScriptableObject do not StartCoroutine defined in the class hierarchy.
                TargetScriptManager.instance.StartCoroutine(SlowDownTime(timeCurve));
            }
        });
    }

    //Slows down time using the curve to determine when time slows down and by how much.
    public IEnumerator SlowDownTime(AnimationCurve curve)
    {
        //Time is being slowed down so update this variable.
        isTimeSlow = true;

        //Get the minimum time and the maximum time in the curve.
        var time = curve.keys.Min((k) => k.time);
        var endTime = curve.keys.Max((k) => k.time);

        //Save the starting values for time.
        var saveFixedDeltaTime = Time.fixedDeltaTime;
        var saveTimeScale = Time.timeScale;
        //Save the player controller.
        var player = FindObjectOfType<FirstPersonController>();
        //Save all the values on the player for movement. When slowing time down we need to keep these variables updated so the player keeps moving normally.
        var savePlayerSpeed = player.m_WalkSpeed;
        var savePlayerRunSpeed = player.m_RunSpeed;
        var savePlayerGravitySpeed = player.m_GravityMultiplier;
        var savePlayerJumpSpeed = player.m_JumpSpeed;
        var savePlayerStickySpeed = player.m_StickToGroundForce;

        //Keep updating time until we reach the end of the curve.
        while (time < endTime)
        {
            //Set new time values based on the curve.
            Time.fixedDeltaTime = curve.Evaluate(time) * saveFixedDeltaTime;
            Time.timeScale = curve.Evaluate(time) * saveTimeScale;
            //Set player variables to the original value divided by the current curve value. This makes the player move faster because the player is not exempt from the game loop time.
            //This cancels out the time slow down for the player and keeps the player moving at the same speed.
            player.m_RunSpeed = savePlayerRunSpeed / curve.Evaluate(time);
            player.m_WalkSpeed = savePlayerSpeed / curve.Evaluate(time);
            player.m_JumpSpeed = savePlayerJumpSpeed / curve.Evaluate(time);
            player.m_StickToGroundForce = savePlayerStickySpeed / curve.Evaluate(time);
            //Gravity is special and needs to be exponentially increased and decreased to keep the player from flying into the sky.
            player.m_GravityMultiplier = savePlayerGravitySpeed / Mathf.Pow(curve.Evaluate(time), 2);

            //Advance the time variable.
            time += 0.01f;

            //Wait for a little bit without the changed time scale interfering.
            yield return new WaitForSecondsRealtime(0.01f);
        }
        //Reset all values to their original in case the curve did not end exactly at 1.
        Time.fixedDeltaTime = saveFixedDeltaTime;
        Time.timeScale = saveTimeScale;
        player.m_GravityMultiplier = savePlayerGravitySpeed;
        player.m_RunSpeed = savePlayerRunSpeed;
        player.m_WalkSpeed = savePlayerSpeed;
        player.m_JumpSpeed = savePlayerJumpSpeed;
        player.m_StickToGroundForce = savePlayerStickySpeed;
        //Since is no longer being slowed down.
        isTimeSlow = false;
    }

    //Override the InvokeExplodeEvent function to add a custom check.
    public override CancelableObject InvokeExplodeEvent(Bomb bomb)
    {
        //Create the CancelableObject to return.
        CancelableObject cancelableObject = new();

        // A local function that requests that the event be canceled.
        void DisableEvent(Bomb bomb, CancelableObject cancelableObject)
        {
            cancelableObject.SetState(CancelableState.Deny);
        }

        //If time is already being slowed down, then the event needs to be canceled.
        if (isTimeSlow)
            //Add the function to the event to cancel it.
            explodeEvent += DisableEvent;
        //Invoke the event.
        explodeEvent.Invoke(bomb, cancelableObject);

        //Remove the DisableEvent function so next time it does not cancel the event.
        explodeEvent -= DisableEvent;
        //Return the CancelableObject.
        return cancelableObject;
    }
}


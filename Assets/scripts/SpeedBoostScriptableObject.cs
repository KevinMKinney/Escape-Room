using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventKit;
using UnityStandardAssets.Characters.FirstPerson;

/* 
 * Written this semester.
 * This scriptable object acts as a data container for the Bomb script. It gives the player a speed boost when the event is fired.
 */
//This line allows an instance to be created in the unity editor by right clicking-> Create -> ScriptableObjects -> Speed Boost Scriptable Object
[CreateAssetMenu(fileName = "SpeedBoostDataScriptableObject", menuName = "ScriptableObjects/Speed Boost Scriptable Object", order = 1)]
public class SpeedBoostScriptableObject : BombScriptableObject
{
    //The time the player has the boost.
    [SerializeField]
    float SpeedBoostTime = 20;
    //The speed multiplier the player gets when the boost gets activated.
    [SerializeField]
    float SpeedBoostPower = 2.5f;
    //Answers the question : Is the boost active?
    [HideInInspector]
    public bool isBoosted = false;

    //Called at the start of the game by unity. Overrieding BombScriptableObject's OnEnable.
    public override void OnEnable()
    {
        //Scriptable objects do not reset thair values, so make sure this is variable is set to false.
        isBoosted = false;
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
            //If no one in the event listener stack objects to giving the player a speed boost then do so.
            if (state == CancelableState.Default || state == CancelableState.Allow)
            {
                //Get a instance of a MonoBehaviour to start the coroutine GiveSpeedBoost. ScriptableObjects do not StartCoroutine defined in the class hierarchy.
                TargetScriptManager.instance.StartCoroutine(GiveSpeedBoost());
            }
        });


        //The event must be canceled if the boost is already active.
        explodeEvent += (timeBomb, cancelableObject) =>
        {
            if (isBoosted)
            {
                //Refuse to allow the boost to activate.
                cancelableObject.SetState(CancelableState.Deny);
            }
        };
    }

    //Gives the player a speed boost for a set amount of time.
    IEnumerator GiveSpeedBoost()
    {
        //Get the player and the first person controller
        GameObject player = GameObject.FindWithTag("Player");
        FirstPersonController controller = player.GetComponent<FirstPersonController>();

        isBoosted = true;

        //Save values to revert back to later
        var saveWalkSpeed = controller.m_WalkSpeed;
        var saveRunSpeed = controller.m_RunSpeed;
        var saveStep = controller.m_StepInterval;

        //Multiply these values by SpeedBoostPower to keep the porportion between  walk speed, run speed and the step interval.
        controller.m_WalkSpeed = saveWalkSpeed * SpeedBoostPower;
        controller.m_RunSpeed = saveRunSpeed * SpeedBoostPower;
        //Step interval is updated to keep the footstep sound from being activated too frequently
        controller.m_StepInterval = saveStep * SpeedBoostPower;

        //The boost lasts for SpeedBoostTime seconds
        yield return new WaitForSeconds(SpeedBoostTime);

        //Revert walk speed, run speed and foodstep interval to their orginal values, therfor removing the boost.
        controller.m_WalkSpeed = saveWalkSpeed;
        controller.m_RunSpeed = saveRunSpeed;
        controller.m_StepInterval = saveStep;

        //Allow the boost to be avtivated again.
        isBoosted = false;
    }
}

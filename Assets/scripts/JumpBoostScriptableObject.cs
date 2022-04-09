using CustomEventKit;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/* 
 * Written this semester.
 * This scriptable object acts as a data container for the Bomb script. It gives the player a jump boost when the event is fired.
 */
//This line allows an instance to be created in the unity editor by right clicking-> Create -> ScriptableObjects -> Jump Boost Scriptable Object.
[CreateAssetMenu(fileName = "JumpBoostDataScriptableObject", menuName = "ScriptableObjects/Jump Boost Scriptable Object", order = 1)]
public class JumpBoostScriptableObject : BombScriptableObject
{
    //The time the player has the boost.
    [SerializeField]
    float JumpBoostTime = 20;
    //The amount of extra jump power the player gets when the boost gets activated.
    [SerializeField]
    float JumpBoostPower = 2;
    //Answers the question: Is the boost active?
    [HideInInspector]
    public bool isBoosted = false;

    //Called at the start of the game by unity. Overriding BombScriptableObject's OnEnable.
    public override void OnEnable()
    {
        //Scriptable objects do not reset their values, so make sure this is variable is set to false.
        isBoosted = false;
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
            //If no one in the event listener stack objects to giving the player a jump boost, then do so.
            if (state == CancelableState.Default || state == CancelableState.Allow)
            {
                //Get an instance of a MonoBehaviour to start the coroutine GiveJumpBoost. ScriptableObjects do not StartCoroutine defined in the class hierarchy.
                TargetScriptManager.instance.StartCoroutine(GiveJumpBoost());
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

    //Gives the player a jump boost for a set amount of time.
    IEnumerator GiveJumpBoost()
    {
        //Get the player.
        GameObject player = GameObject.FindWithTag("Player");
        //Get the first-person controller.
        FirstPersonController controller = player.GetComponent<FirstPersonController>();

        isBoosted = true;
        //Save the jump speed so it can revert to the original value later.
        var saveJumpSpeed = controller.m_JumpSpeed;
        //Make the player jump higher by JumpBoostPower amount.
        controller.m_JumpSpeed = saveJumpSpeed + JumpBoostPower;
        //The boost lasts for JumpBoostTime seconds.
        yield return new WaitForSeconds(JumpBoostTime);
        //Revert jump speed and deactivate the boost.
        controller.m_JumpSpeed = saveJumpSpeed;
        isBoosted = false;
    }
}

//Abstract
using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using UnityEngine.UI;
using TMPro;

public class GuideStateManager : MonoBehaviour //Code based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
    //Beginning here the code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
   GuideBaseState currentState;
   public GivingHint HintState = new GivingHint();
   public EventDialogue EventState = new EventDialogue();
   public RoomDialogue RoomState = new RoomDialogue();
   public RoomTrigger TriggerState = new RoomTrigger();
   public NotTalking TalkingState = new NotTalking();
   //End of iHeartGameDev code
   private TextMeshProUGUI sometext;
   //Beginning here the code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    void Start()
    {
        currentState = TriggerState;
        currentState.EnterState(this);
    }//End of iHeartGameDev code
     void OnTriggerEnter(Collider collider)
    {
        currentState.OnTriggerEnter(this,collider);
    }
    void Message(TextMeshProUGUI sometext){
        currentState.Message(this, sometext);
    }
   //Beginning here the code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    void Update()
    {
        currentState.UpdateState(this);
    }
    //Switches state when called in a concrete state
    public void SwitchState(GuideBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }//End of iHeartGameDev code
}

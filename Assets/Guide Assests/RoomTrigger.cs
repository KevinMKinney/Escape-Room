//Concrete state
using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using TMPro;//Used for the text UI components
using UnityEngine.UI;
public class RoomTrigger : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
    public GuideStateManager Guide2;//test
    public Collider collider2;//test
    public KeyCode key2 = KeyCode.N;//Listens for user keyboard input to close prompt
    private TextMeshProUGUI sometext;//defining text UI variable
    public RawImage background;
    //Calls OnTriggerEnter whenever the user enters a puzzle area
    public override void OnTriggerEnter(GuideStateManager Guide, Collider collider)
    {
        //If user enters any of the puzzle triggers, switch to RoomDialogue
        if(collider.gameObject.tag=="spawn"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 1 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 2 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 0 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Puzzle 3 Wait"){
            Guide.SwitchState(Guide.RoomState);
        }
        if(collider.gameObject.tag=="Prompt1" || collider.gameObject.tag=="Prompt2" ||collider.gameObject.tag=="Prompt3" ||collider.gameObject.tag=="Prompt4"){
            Guide.SwitchState(Guide.EventState);
        }
        if(collider.gameObject.tag=="midpointGuide" && Guide.EventState.midpoint==true){
            Guide.SwitchState(Guide.EventState);
        }
    }
    //Initial state called when switching state
    public override void EnterState(GuideStateManager Guide)//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    {
        GameObject random = GameObject.Find("GuideMessageBackground");
        background = random.GetComponent<RawImage>();
        background.enabled=false;
        GameObject prompt3Coll = GameObject.Find("Prompt3");
        BoxCollider prompt3CollSize = prompt3Coll.GetComponent<BoxCollider>();
        prompt3CollSize.enabled=false;
    }
    //Update listens every frame for user keyboard input, resets text UI
    public override void UpdateState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
        if(Input.GetKeyDown(key2)){
            GameObject random = GameObject.Find("GuideMessage");
            sometext = random.GetComponent<TextMeshProUGUI>();
            sometext.text=" ";
            Guide.SwitchState(Guide.TriggerState);
      }        
    }
    public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){    
    }
}

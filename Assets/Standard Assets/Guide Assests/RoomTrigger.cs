//Concrete state
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;//Calls user script
using TMPro;//Used for the text UI components

public class RoomTrigger : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
    KeyCode key2 = KeyCode.N;//Listens for user keyboard input to close prompt
    private TextMeshProUGUI sometext;//defining text UI variable
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
    }
    //Initial state called when switching state
    public override void EnterState(GuideStateManager Guide)//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    {
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

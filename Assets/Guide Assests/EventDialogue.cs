using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using TMPro;//Used for the text UI components
public class EventDialogue : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
int eventD = 0;//Dialogue system (Which event is which) 
private TextMeshProUGUI sometext;//defining text UI variable
KeyCode key2 = KeyCode.N; //listens for the N key being pressed
//When the user completes a puzzle, prompt with a message (not a hint)
public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){
        GameObject random = GameObject.Find("GuideMessage");//Whenever the player triggers a hint area, takes the object and attaches a text UI
        sometext = random.GetComponent<TextMeshProUGUI>();//Fetching the text UI component
        if(collider.gameObject.tag=="event1"){
        eventD = 1;
        sometext.text = "I wonder what that key is for?";
        if(Input.GetKeyDown(key2)){
        Message(Guide,sometext);
        }
        }
        if(collider.gameObject.tag=="event2"){
        eventD = 2;
        sometext.text="What could this be used for?";   
        if(Input.GetKeyDown(key2)){
        Message(Guide,sometext);
        }
        }
        if(collider.gameObject.tag=="event3"){
        eventD = 3;        
        sometext.text="You sure got an eye for patterns!";
        if(Input.GetKeyDown(key2)){
        Message(Guide,sometext);
        }
        }
    }
    public override void EnterState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    }
    public override void UpdateState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    }
    public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){ 
        sometext.text="";  
    }
}

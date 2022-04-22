//Concrete state
using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using TMPro;//Used for the text UI components
using UnityEngine.UI;
public class GivingHint : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
  private TextMeshProUGUI sometext;//defining text UI variable
  public int hintRoom=0;//A hint system that identifies which hint area the player has triggered
  KeyCode key = KeyCode.Y;//listens for the Y key being pressed
  public KeyCode key2 = KeyCode.N; //listens for the N key being pressed
  public RawImage background;
public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){//Detects collision with puzzle triggers
           GameObject random = GameObject.Find("GuideMessage");//Whenever the player triggers a hint area, takes the object and attaches a text UI
           sometext = random.GetComponent<TextMeshProUGUI>();//Fetching the text UI component 
           GameObject randomB = GameObject.Find("GuideMessageBackground");
           background = randomB.GetComponent<RawImage>();
        if(collider.gameObject.tag=="HintArea1" || collider.gameObject.tag=="HintArea1.2" || collider.gameObject.tag=="HintArea1.3" || collider.gameObject.tag=="HintArea1.4" || collider.gameObject.tag=="HintArea1.5"){//detects if player has collided with any of these area triggers
           hintRoom=1;//setting hint system value
           sometext.text = "Would you like a hint for this puzzle?  If yes press Y, if not then press N";//setting "sometext" to prompt user 
           background.enabled=true;
            if(Input.GetKeyDown(key)){//If they hit yes, set "sometext" to nothing, go to Message method
              sometext.text=" ";
              Message(Guide, sometext);
            }
            if(Input.GetKeyDown(key2)){//If they hit no, set "sometext" to nothing, switch to RoomTrigger
              sometext.text=" ";
              Guide.SwitchState(Guide.TriggerState);//Switch states
            }
        }
        //The next three if statements have the same functionality as the first
        if(collider.gameObject.tag=="HintArea2" || collider.gameObject.tag=="HintArea2.1" || collider.gameObject.tag=="HintArea2.2" || collider.gameObject.tag=="HintArea2.3" || collider.gameObject.tag=="HintArea2.4" || collider.gameObject.tag=="HintArea2.5" || collider.gameObject.tag=="HintArea2.6" || collider.gameObject.tag=="HintArea2.7" || collider.gameObject.tag=="HintArea2.8" || collider.gameObject.tag=="HintArea2.9" || collider.gameObject.tag=="HintArea2.91"){
           hintRoom=2;
           sometext.text = "Would you like a hint for this puzzle?  If yes press Y, if not then press N";
           background.enabled=true;
            if(Input.GetKeyDown(key)){
              sometext.text=" ";
              Message(Guide, sometext);
            }
            if(Input.GetKeyDown(key2)){
              sometext.text=" ";
              Guide.SwitchState(Guide.TriggerState);
            }
        }
        if(collider.gameObject.tag=="HintArea0" || collider.gameObject.tag=="HintArea0.1" || collider.gameObject.tag=="HintArea0.2" || collider.gameObject.tag=="HintArea0.3" || collider.gameObject.tag=="HintArea0.4" || collider.gameObject.tag=="HintArea0.5" || collider.gameObject.tag=="HintArea0.6" || collider.gameObject.tag=="HintArea0.7" || collider.gameObject.tag=="HintArea0.8"){
           hintRoom=3;
           sometext.text = "Would you like a hint for this puzzle?  If yes press Y, if not then press N";
           background.enabled=true; 
            if(Input.GetKeyDown(key)){
              sometext.text=" ";
              Message(Guide, sometext);
            }
            if(Input.GetKeyDown(key2)){
              sometext.text=" ";
              Guide.SwitchState(Guide.TriggerState);
            }
        }
        if(collider.gameObject.tag=="HintArea3" || collider.gameObject.tag=="HintArea3.1" || collider.gameObject.tag=="HintArea3.2" || collider.gameObject.tag=="HintArea3.3" || collider.gameObject.tag=="HintArea3.4" || collider.gameObject.tag=="HintArea3.5" || collider.gameObject.tag=="HintArea3.6" || collider.gameObject.tag=="HintArea3.7" || collider.gameObject.tag=="HintArea3.8" || collider.gameObject.tag=="HintArea3.9" || collider.gameObject.tag=="HintArea3.91" || collider.gameObject.tag=="HintArea3.92" || collider.gameObject.tag=="HintArea3.93" || collider.gameObject.tag=="HintArea3.94" || collider.gameObject.tag=="HintArea3.95" || collider.gameObject.tag=="HintArea3.96"){
           hintRoom=4;
           sometext.text = "Would you like a hint for this puzzle?  If yes press Y, if not then press N";
           background.enabled=true; 
            if(Input.GetKeyDown(key)){
              sometext.text=" ";
              Message(Guide, sometext);
            }
            if(Input.GetKeyDown(key2)){
              sometext.text=" ";
              Guide.SwitchState(Guide.TriggerState);
            }
        }
        if(collider.gameObject.tag=="TReset"){//Whenever the user collides with "TReset" trigger, hint timer for each room is reset and state is switched
           sometext.text=" ";
           Guide.SwitchState(Guide.TriggerState);
         }
    }
    //When you switch states, EnterState is the first method to be called
    public override void EnterState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
        background.enabled=false;
    }
    //Update listens for key input every frame
    public override void UpdateState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
      if(Input.GetKeyDown(key2)){//If the user wants to close the prompt, switch state
        sometext.text=" ";
        background.enabled=false;
        Guide.SwitchState(Guide.TriggerState);
      }
      if(Input.GetKeyDown(key)){//If user wants to see the hint, close current prompt and display the hint
           GameObject random = GameObject.Find("GuideMessage");
           sometext = random.GetComponent<TextMeshProUGUI>();
           sometext.text=" ";
           background.enabled=true;          
           Message(Guide, sometext);
            }
    }
    //Provides the correlated hint for the hint area triggered
    public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){
        //Depending on what hint is triggered, displays message, switches state and sets the hintRoom variable back to 0
        if(hintRoom==1){
          sometext.text = "It seems like the gun would be useful \n\n\nPress N to close hint";
          //Guide.SwitchState(Guide.TriggerState);
          hintRoom=0;
        }
        if(hintRoom==2){
          sometext.text = "Seems like these planks are easily knocked over \n\n\nPress N to close hint";
          //Guide.SwitchState(Guide.TriggerState);
          hintRoom=0;
        }
        if(hintRoom==3){
          sometext.text = "Looks like you'll need a gas can \n\n\nPress N to close hint";
          //Guide.SwitchState(Guide.TriggerState);
          hintRoom=0;
        }
        if(hintRoom==4){
          sometext.text = "There might be a pattern here.. \n\n\nPress N to close hint";
          //Guide.SwitchState(Guide.TriggerState);
          hintRoom=0;
        }
    }
}

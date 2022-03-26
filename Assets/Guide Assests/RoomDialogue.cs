//Concrete state
using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using UnityEngine.UI;
using TMPro;

public class RoomDialogue : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
public bool checkD=true;
private TextMeshProUGUI sometext;//defining text UI variable
KeyCode key2 = KeyCode.N;
public int dialogueSet=0; //When no triggers have been tripped
public int HintSet=0;//Setting the hint system to 0 
public float timer=5;//Hint timer (5 minutes)
  //Calls OnTriggerEnter whenever the user enters a puzzle area
  public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){
    //Debug.Log("ONTRIGGER in ROOMDIALOGUE");
         if(collider.gameObject.tag=="spawn")//When the player exits spawn, display intro message
         {
           dialogueSet = 1;//setting the dialogue to 1, introduces guide
           GameObject random = GameObject.Find("GuideMessage");
           sometext = random.GetComponent<TextMeshProUGUI>();
           Message(Guide, sometext);
         }
         if(collider.gameObject.tag=="SpawnReset"){//When the user exits spawn area, reset the hint timer and the hint system
           HintSet=0;
           timer=5;
         }
         if(collider.gameObject.tag=="Puzzle 1 Wait"){//When the player enters the first puzzle area, destroy the intro message and set hint system to specific puzzle
           HintSet=1;
           GameObject SpawnTrigger = GameObject.Find("SpawnTrigger");
           DestroyGameObject(SpawnTrigger);
         }
         //When the user triggers any of the puzzle areas, set hint system to specified number
         if(collider.gameObject.tag=="Puzzle 2 Wait"){
           HintSet=2;
         }
         if(collider.gameObject.tag=="Puzzle 0 Wait"){
           HintSet=3;
         }
         if(collider.gameObject.tag=="Puzzle 3 Wait"){
           HintSet=4;
         }
         if(collider.gameObject.tag=="TReset"){//When the user goes into main area of level, reset the hint timer and the hint system
           HintSet=0;
           timer=5;
         }
    }
  //Displays intro message for the user
  public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){
         if(dialogueSet==1){//When user enters SpawnTrigger object, display intro message
          sometext.text = "Hey, it seems you've gotten stuck in a haunted mansion, I'll be your guide to help you out! \n\nAfter a few minutes of trying to solve the puzzle, I'll check in on you to see if you need any help.\n\n Press N to close prompts.";
         }
      }
  //Initial state called when switching state
  public override void EnterState(GuideStateManager Guide)//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    {
    }
  //Update listens every frame for user keyboard input
  public override void UpdateState(GuideStateManager Guide)//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
     {
      if(Input.GetKeyDown(key2))//If the user presses N, closes current prompt
      {
        sometext.text = " ";//clears the text UI
        dialogueSet=0;//sets dialogue back to 0
        }
         if(HintSet==1 || HintSet==2 || HintSet==3 || HintSet==4){//listens for any of the puzzle areas being triggered, begins hint countdown
           if(timer>0){//Counts down for 5 minutes
             timer -= 1*Time.deltaTime;
             //Debug.Log(timer);
           }else{
             timer=5;
             HintSet=0;
             Guide.SwitchState(Guide.HintState);//Once the timer is up, switch to hint state
           }
         }
      }  
   public void DestroyGameObject(GameObject other)//destroys object being passed into the argument 
   {
    UnityEngine.Object.Destroy(other);//Destroys object, UnityEngine.Object used because concrete state does not derive from MonoBehaviour 
  }
}
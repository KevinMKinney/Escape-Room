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
public float timer=300;//Hint timer (5 minutes)
public RawImage background;//Declaring image variable for GuigeBackgroundMessage art
  //Calls OnTriggerEnter whenever the user enters a puzzle area
  public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){
           GameObject randomB = GameObject.Find("GuideMessageBackground");
           background = randomB.GetComponent<RawImage>();
         if(collider.gameObject.tag=="spawn")//When the player exits spawn, display intro message
         {
           dialogueSet = 1;//setting the dialogue to 1, introduces guide
           GameObject random = GameObject.Find("GuideMessage");
           sometext = random.GetComponent<TextMeshProUGUI>();
           background.enabled=true;
           Message(Guide, sometext);
         }
         if(collider.gameObject.tag=="SpawnReset"){//When the user exits spawn area, reset the hint timer and the hint system
           HintSet=0;
           timer=300;
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
           timer=300;
           //Creating the Prompt3 box collider once the player goes down the stairs
           GameObject prompt3Coll = GameObject.Find("Prompt3");
           prompt3Coll.transform.position = new Vector3(5.56f,0.58f,0.92f);
           BoxCollider prompt3CollSize = prompt3Coll.GetComponent<BoxCollider>();
           prompt3CollSize.size = new Vector3(30,2,12);
           prompt3CollSize.isTrigger = true;
         }
         if(collider.gameObject.tag=="Prompt1" || collider.gameObject.tag=="Prompt2" || collider.gameObject.tag=="Prompt3" || collider.gameObject.tag=="Prompt4"){
            Guide.SwitchState(Guide.EventState);
        }
        if(collider.gameObject.tag=="midpointGuide" && Guide.EventState.midpoint==true){
            Guide.SwitchState(Guide.EventState);
        }
    }
  //Displays intro message for the user
  public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){
         if(dialogueSet==1){//When user enters SpawnTrigger object, display intro message
          sometext.text = "Hey, it seems you've gotten stuck in a haunted mansion, I'll be your guide to help you out! \n\nAfter a few minutes of trying to solve the puzzle, I'll check in on you to see if you need any help.\n\n Press N to close prompts.";
          GameObject random = GameObject.Find("GuideMessageBackground");
          background = random.GetComponent<RawImage>();
          background.enabled=true;
         }
      }
  //Initial state called when switching state
  public override void EnterState(GuideStateManager Guide)//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    {
        GameObject random = GameObject.Find("GuideMessageBackground");
        background = random.GetComponent<RawImage>();
        background.enabled=false;
    }
  //Update listens every frame for user keyboard input
   public override void UpdateState(GuideStateManager Guide)//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
     {
      if(Input.GetKeyDown(key2))//If the user presses N, closes current prompt
      {
        checkD=false;
        sometext.text = " ";//clears the text UI
        dialogueSet=0;//sets dialogue back to 0
        background.enabled=false;
        }
         if(HintSet==1 || HintSet==2 || HintSet==3 || HintSet==4){//listens for any of the puzzle areas being triggered, begins hint countdown
           if(timer>0){//Counts down for 5 minutes
             timer -= 1*Time.deltaTime;
           }else{
             timer=300;
             HintSet=0;
             Guide.SwitchState(Guide.HintState);//Once the timer is up, switch to hint state
           }
         }
         if(checkD==false){//Makes sure the player sees intro prompt before timer starts
                sometext.text=" ";
                TimerG(timer);
         }
      }
    //Displays hint timer on bottom right of the screen
   public void TimerG(float timer){
            GameObject random = GameObject.Find("TimerMessage");
            sometext = random.GetComponent<TextMeshProUGUI>();
            float timeMath = timer/60;//Converts from seconds to minutes
            string TimerS = timeMath.ToString("#.##");//Rounding to two decimal places
            sometext.text="Time left until hint: " + TimerS;//Storing the text
   }
   public void DestroyGameObject(GameObject other)//destroys object being passed into the argument 
    {
    UnityEngine.Object.Destroy(other);//Destroys object, UnityEngine.Object used because concrete state does not derive from MonoBehaviour 
  }
}
using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using TMPro;//Used for the text UI components
public class EventDialogue : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
  /*Check to see if overallDemeanor is correct*/
  /*Left on punishment(), can't call other scripts (???) find different way to negatively affect user*/
  int eventD, posChoice, negChoice, overallDemeanor = 0;//Dialogue system (Which event is which) 
  private TextMeshProUGUI sometext;//defining text UI variable
  public KeyCode K_Key = KeyCode.K;
  public KeyCode L_Key = KeyCode.L;
  Vector3 playerPosition;
    //When the user completes a puzzle, prompt with a message (not a hint)
    public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){
      GameObject random = GameObject.Find("GuideMessage");//Whenever the player triggers a hint area, takes the object and attaches a text UI
      sometext = random.GetComponent<TextMeshProUGUI>();//Fetching the text UI component
        
        if(collider.gameObject.tag=="Prompt1"){
            eventD = 1;//Setting the event number
            sometext.text = "Do you prefer apples (K) or oranges (L)?\n\n";//Guide prompt
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="event2"){
            eventD = 2;
            sometext.text="Did you ever hear the Tragedy of Darth Plagueis the Wise?\n\n Yes (K) or No (L)";   
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="event3"){
            eventD = 3;        
            sometext.text="Which is your favorite, Ghostbusters (1984) (K) or Ghosbusters (2016) (L)?";
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="event4"){
            eventD = 4;        
            sometext.text="Final question. . . . Pepsi (K) or Coke (L)?";
            OnTriggerExit(collider);//destroys the trigger
        }
    }
    //Destroys guide prompts after user exits the trigger area
    public void OnTriggerExit(Collider other){
      DestroyGameObject(other.gameObject);
    }
    public override void EnterState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    }
    public override void UpdateState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
      
      GameObject FPSController = GameObject.Find("FPSController");
      //Keeping track of current player position
      playerPosition = FPSController.transform.position;
      PlayerPosition(playerPosition);
        if(Input.GetKeyDown(K_Key)){//If the user wants to close the prompt, switch state
            overallDemeanor++;
            posChoice=1;
            choice(posChoice);
            Message(Guide, sometext);
        }
        if(Input.GetKeyDown(L_Key)){//If user wants to see the hint, close current prompt and display the hint
            overallDemeanor--;
            negChoice=-1;
            choice(negChoice);
            Message(Guide, sometext);
          }
      }
    //Returns the choice made by the user
    public int choice(int choice){
      if(choice==1){
        posChoice=1;
        return posChoice;
      }
      if(choice==-1){
        negChoice=-1;
        return negChoice;
      }
      return 99;//Error check
    }
    public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){ 
      /*Start of puzzle related events*/
        //First puzzle event dialogue
        if(eventD==1){
          if(posChoice==1){
            sometext.text="A fellow apple enthusiast!\n\n (The guide favors you more!) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
          if(negChoice==-1){
            sometext.text="Shame... I'm more of an apple kind of ghost\n\n (The guide favors you less..) \n\n Press N to close prompt";
          }
        }
        //Second puzzle event dialogue
        if(eventD==2){
          if(posChoice==1){
            sometext.text="I see, you're well read! \n\n (The guide favors you more!) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
          if(negChoice==-1){
            sometext.text= "Unfortunate, but not unexpected.. \n\n (The guide favors you less..) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
        } 
        //Third puzzle event dialogue
        if(eventD==3){
            sometext.text=" ";
        } 
        //Fourth puzzle event dialogue
        if(eventD==4){
            sometext.text=" ";
        } 
        //Fifth puzzle evetn dialogue
        if(eventD==5){
            sometext.text=" ";
        } 
    }

    public Vector3 PlayerPosition(Vector3 playerPosition){
      return playerPosition;
    }
    //Spawns a cherry if the overall demeanor toward the player is positive
    public void spawnBonus(){
      GameObject bonusCherry = GameObject.Find("Cherry");
      GameObject bonusFruit = GameObject.Instantiate(bonusCherry, playerPosition, Quaternion.identity) as GameObject;
    }
    //Slows down the player for a short time if the overall demeanor toward the player is negative
    public void punishment(){
      GameObject player_position = GameObject.Find("Hammer");
      player_position.transform.position = new Vector3(8,3,1);

    }
    public void GuideBehavior(){
      Debug.Log(overallDemeanor);
      //Will need to change parameters for GuideBehavior to cover all scenarios
      if(overallDemeanor>0){
        sometext.text="I like your style human, here's a bonus fruit!";
        //Spawn object here
        spawnBonus();
      }
      if(overallDemeanor<0){
        sometext.text="You have awful taste! Let's slow things down a little..";
        //Change player speed here
        punishment();
      }
    }
    public void DestroyGameObject(GameObject other)//destroys object being passed into the argument 
   {
    UnityEngine.Object.Destroy(other);//Destroys object, UnityEngine.Object used because concrete state does not derive from MonoBehaviour 
  }
}

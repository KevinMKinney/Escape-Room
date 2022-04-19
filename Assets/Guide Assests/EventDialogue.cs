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
            sometext.text = "Which is your favorite hotsauce: Doctor Grant's Hotsauce (K) or Tabasco (L)?\n\n";//Guide prompt
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="Prompt2"){
            eventD = 2;
            sometext.text="Did you ever hear the Tragedy of Darth Plagueis the Wise?\n\n Yes (K) or No (L)";   
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="Prompt3"){
            eventD = 3;        
            sometext.text="Would you rather fight 100 duck-sized horses, or 1 horse-sized duck?\n\n 100 duck-sized horses (K) or 1 horse-sized duck (L)";
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="Prompt4"){
            eventD = 4;        
            sometext.text="If you could go back into the past and change one thing, would you: Save Harambe (K) or play the lotto knowing the winnning numbers (K)?";
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="midpointGuide" && eventD==2){
          GuideBehavior();
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
            sometext.text="It's my favorite as well!\n\n (The guide favors you more!) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
          if(negChoice==-1){
            sometext.text="-1 discretionary point\n\n (The guide favors you less..) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
        }
        //Second puzzle event dialogue
        if(eventD==2){
          if(posChoice==1){
            sometext.text="I see, a person of culture!\n\n (The guide favors you more!) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
          if(negChoice==-1){
            sometext.text= "I thought not, it's not a story the jedi would tell you..\n\n (The guide favors you less..) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
        } 
        //Third puzzle event dialogue
        if(eventD==3){
          if(posChoice==1){
            sometext.text="Great minds think alike!\n\n (The guide favors you more!) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
          if(negChoice==-1){
            sometext.text= "Google the inside of a duck mouth and tell me you'd still win\n\n (The guide favors you less..) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
        } 
        //Fourth puzzle event dialogue
        if(eventD==4){
            if(posChoice==1){
            sometext.text="You and me both\n\n (The guide favors you more!) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
          if(negChoice==-1){
            sometext.text= "Have you no shame?\n\n (The guide favors you less..) \n\n Press N to close prompt";
            Guide.SwitchState(Guide.TriggerState);
          }
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
    //Teleports the puzzle item back to spawn, making it inconvenient for the player
    public void punishment(){
      GameObject player_position = GameObject.Find("Hammer");
      player_position.transform.position = new Vector3(8,3,1);
    }
    public void GuideBehavior(){
      //Will need to change parameters for GuideBehavior to cover all scenarios
      if(overallDemeanor>0){
        sometext.text="I like your style human, here's a bonus fruit!\n\n (You were given a cherry)";
        //Spawn object here
        spawnBonus();
      }
      if(overallDemeanor<0){
        sometext.text="My disappointment is immeasurable, and my day is ruined \n\n (The guide moved the puzzle item, go back to previous rooms and find it)";
        //Teleporting puzzle item back to spawn
        punishment();
      }
    }
    public void DestroyGameObject(GameObject other)//destroys object being passed into the argument 
   {
    UnityEngine.Object.Destroy(other);//Destroys object, UnityEngine.Object used because concrete state does not derive from MonoBehaviour 
  }
}

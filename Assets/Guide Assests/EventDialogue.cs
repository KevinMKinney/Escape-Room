//Concrete state
using System.Collections;//auto-generated code
using System.Collections.Generic;//auto-generated code
using UnityEngine;//auto-generated code
using TMPro;//Used for the TextMeshPro components
using UnityEngine.UI;//Used for RawImage components
public class EventDialogue : GuideBaseState //this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
  int posChoice, negChoice, overallDemeanor = 0;//Dialogue system (Which event is which)
  public int eventD=0;
  public bool midpoint=false;
  private TextMeshProUGUI sometext;//defining text UI variable
  public KeyCode K_Key = KeyCode.K;//The "positive" key for the guide behavior
  public KeyCode L_Key = KeyCode.L;//The "negative" key for the guide behavior
  public KeyCode key2 = KeyCode.N;//The key for closing the guide prompts
  Vector3 playerPosition;//Used to store current player position for spawnBonus() function
  public RawImage background;//Used for the background for the guide prompts
    //When the user completes a puzzle, prompt with a message (not a hint)
    public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){
      GameObject random = GameObject.Find("GuideMessage");//Whenever the player triggers a hint area, takes the object and attaches a text UI
      sometext = random.GetComponent<TextMeshProUGUI>();//Fetching the text UI component
      GameObject randomB = GameObject.Find("GuideMessageBackground");
      background = randomB.GetComponent<RawImage>();
        if(collider.gameObject.tag=="Prompt1"){
            background.enabled=true;
            eventD=1;//Setting the event number
            sometext.text = "Which is your favorite hotsauce: Doctor Grant's Hotsauce (K) or Tabasco (L)?\n\n";//Guide prompt
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="Prompt2"){
            background.enabled=true;
            eventD=2;
            sometext.text="Did you ever hear the Tragedy of Darth Plagueis the Wise?\n\n Yes (K) or No (L)";   
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="Prompt3"){
            background.enabled=true;
            eventD=3;        
            sometext.text="Would you rather fight 100 duck-sized horses, or 1 horse-sized duck?\n\n 100 duck-sized horses (K) or 1 horse-sized duck (L)";
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="Prompt4"){
            background.enabled=true;
            eventD=4;        
            sometext.text="If you could go back into the past and change one thing, would you: Save Harambe (K) or play the lotto knowing the winnning numbers (L)?";
            OnTriggerExit(collider);//destroys the trigger
        }
        if(collider.gameObject.tag=="midpointGuide" && midpoint==true){
            background.enabled=true;
            GuideBehavior();
            OnTriggerExit(collider);
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
        if(Input.GetKeyDown(K_Key) && eventD !=0){//If the user wants to close the prompt, switch state
            background.enabled=false;
            overallDemeanor++;
            posChoice=1;
            choice(posChoice);
            Message(Guide, sometext);
        }
        if(Input.GetKeyDown(L_Key) && eventD !=0){//If user wants to see the hint, close current prompt and display the hint
            background.enabled=false;
            overallDemeanor--;
            negChoice=-1;
            choice(negChoice);
            Message(Guide, sometext);
          }
        if(Input.GetKeyDown(key2) && eventD==0){
          sometext.text=" ";
          Guide.SwitchState(Guide.RoomState);
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
          background.enabled=true;
          if(posChoice==1){
            eventD=0;
            sometext.text="It's my favorite as well!\n\n (The guide favors you more!) \n\n Press N to close prompt";
          }
          if(negChoice==-1){
            eventD=0;
            sometext.text="-1 discretionary point\n\n (The guide favors you less..) \n\n Press N to close prompt";
          }
        }
        //Second puzzle event dialogue
        if(eventD==2){
          background.enabled=true;
          if(posChoice==1){
            eventD=0;
            sometext.text="I see, a person of culture!\n\n (The guide favors you more!) \n\n Press N to close prompt";
          }
          if(negChoice==-1){
            eventD=0;
            sometext.text= "I thought not, it's not a story the jedi would tell you..\n\n (The guide favors you less..) \n\n Press N to close prompt";
          }
        } 
        //Third puzzle event dialogue
        if(eventD==3){
          background.enabled=true;
          if(posChoice==1){
            eventD=0;
            sometext.text="Great minds think alike!\n\n (The guide favors you more!) \n\n Press N to close prompt";
          }
          if(negChoice==-1){
            eventD=0;
            sometext.text= "Google the inside of a duck mouth and tell me you'd still win\n\n (The guide favors you less..) \n\n Press N to close prompt";
          }
        } 
        //Fourth puzzle event dialogue
        if(eventD==4){
          midpoint=true;
          background.enabled=true;
            if(posChoice==1){
              eventD=0;
              sometext.text="You and me both\n\n (The guide favors you more!) \n\n Press N to close prompt";
          }
          if(negChoice==-1){
              eventD=0;
              sometext.text= "Have you no shame?\n\n (The guide favors you less..) \n\n Press N to close prompt";
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
      GameObject player_position = GameObject.Find("Gun");
      player_position.transform.position = new Vector3(8,3,1);
    }
    public void GuideBehavior(){
      //Will need to change parameters for GuideBehavior to cover all scenarios
      if(overallDemeanor>0){
        sometext.text="I like your style human, here's a bonus fruit!\n\n (You were given a cherry)\n\n Press N to close prompt";
        //Spawn object here
        spawnBonus();
      }
      if(overallDemeanor<0){
        sometext.text="My disappointment is immeasurable, and my day is ruined \n\n (The guide moved the puzzle item, go back to previous rooms and find it)\n\n Press N to close prompt";
        //Teleporting puzzle item back to spawn
        punishment();
      }
      if(overallDemeanor==0){
        sometext.text="Very neutral responses so far, guess you can't be that bad\n\n (The guide reluctantly gives you a cherry)\n\n Press N to close prompt";
        spawnBonus();
      }
    }
    public void DestroyGameObject(GameObject other)//destroys object being passed into the argument 
   {
    UnityEngine.Object.Destroy(other);//Destroys object, UnityEngine.Object used because concrete state does not derive from MonoBehaviour 
  }
}

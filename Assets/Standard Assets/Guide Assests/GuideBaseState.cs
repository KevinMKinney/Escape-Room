//Base State 
using UnityEngine;//auto-generated code
using static UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController;//Calling RigidBody for the player
using UnityEngine.UI;
using TMPro;

public abstract class GuideBaseState//Beginning here the code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI 
{
    public abstract void EnterState(GuideStateManager Guide);
    public abstract void UpdateState(GuideStateManager Guide);
    //End of iHeartGameDev code
    public abstract void OnTriggerEnter(GuideStateManager Guide, Collider collider);
    public abstract void Message(GuideStateManager Guide, TextMeshProUGUI sometext);
}
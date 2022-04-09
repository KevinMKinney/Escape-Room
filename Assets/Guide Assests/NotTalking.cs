using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NotTalking : GuideBaseState//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
{
   public override void OnTriggerEnter(GuideStateManager Guide, Collider collider){
        if(collider.gameObject.tag=="spawn"){
            Guide.SwitchState(Guide.RoomState);
        }

        if(collider.gameObject.tag=="puzzle 1"){
            Guide.SwitchState(Guide.RoomState);
        }
    }

    public override void EnterState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    }

    public override void UpdateState(GuideStateManager Guide){//this line of code is based off of iHeartGameDev https://youtu.be/Vt8aZDPzRjI
    }

    public override void Message(GuideStateManager Guide, TextMeshProUGUI sometext){

    }
}

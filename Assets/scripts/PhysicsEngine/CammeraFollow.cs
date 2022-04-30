using System.Collections;
using System.Collections.Generic;
using UnityEngine;//everything above is unity generated

/* Description: This allows for the camera to be able to follow the player to give 1st pov. Also some other things such as zoom in/out
    */
public class CammeraFollow : MonoBehaviour //unity generated
{
    public GameObject player;
    private bool zoomedOut;
    private bool zoomedIn;
    private float cameraRegFov;
    public float cameraChangeRatio;

    /* Description: Start before the first frame to set up the camera, such as being next to the player, setting the cursor to be invisible
                    and set all bool values to false and constants to a certain value
    */
    void Start()//unity generated
    {
        player = GameObject.Find("Player");
        transform.position = player.transform.position;
        Cursor.visible=false;
        zoomedOut = false;
        cameraRegFov = 90.0f;
        if(cameraChangeRatio <= 0) cameraChangeRatio = 0.1f;

    }

    /* Description: Just like update, but does things a little faster, this will make sure the camera follows the player everywhere
                    and adust the players rotation based off where the player points too.
    */
    void FixedUpdate(){//actually I used this
    
        transform.position = GameObject.Find("Player").transform.position;//mine
        transform.Rotate(0, Input.GetAxis("Mouse X")*cameraChangeRatio, 0);
        transform.Rotate(-Input.GetAxis("Mouse Y")*cameraChangeRatio, 0, 0);
        player.transform.rotation = transform.rotation;
    }

    /* Description: Inverts the camera by setting the camera changing ratio to be switched to negative, or uninvert it if player wants to
    */
    public void invertCamera(){
        cameraChangeRatio = cameraChangeRatio * -1;
    } 

    /* Description: Zooms the camera out by changing the camera's field of view sligtly bigger, and only can you undo this by using same 
                    command again
    */
    public void zoomOut(){
        if(!zoomedOut && !zoomedIn){
            Camera.main.fieldOfView = cameraRegFov * 2;
            zoomedOut = true;
        }
        else if(zoomedOut){
            Camera.main.fieldOfView = cameraRegFov;
            zoomedOut = false;
        }
    }

    
    /* Description: Zooms the camera in by changing the camera's field of view sligtly smaller, and only can you undo this by using same 
                    command again
    */
    public void zoomIn(){
        if(!zoomedOut && !zoomedIn){
            Camera.main.fieldOfView = cameraRegFov / 2;
            zoomedIn = true;
        }
        else if(zoomedIn){
            Camera.main.fieldOfView = cameraRegFov;
            zoomedIn = false;
        }
    }
}

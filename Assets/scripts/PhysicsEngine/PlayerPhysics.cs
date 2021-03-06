using System.Collections;
using System.Collections.Generic;
using UnityEngine;//everything before this is auto generated
using System.Linq;//need this for functional refactoring
using System;//need this for math functions

 /* Description: The player physics engine to help the player have certain abilities and be able to interact with the eniviroment
*/
public class PlayerPhysics : MonoBehaviour
{
    private GameObject player;
    private GameObject pickedUpItem;
    private GameObject shootItem;
    public float weight;
    private PlayerObject pObj;
    private GameObject[] allObjects;//for collisions
    private ItemObject[] itemObjects;
    private bool hasJumped;
    public int speed;
    private int oldSpeed;
    private float amountWalked;
    private Vector3 orgPos;
    private bool doubleJumpAchieve;
    private bool speedUpMovementAcheive;
    private bool teleportAchieve;
    private bool dashMoveAchieve;
    private bool shooterAcheive;
    private int jumpCount;
    private int thrownCount;
    private int doubleJumpCount;
    private int teleportCount;
    private int speedBoostCount;
    private int shootAmount;
    

    
    /* Description: Start is called before the first frame update, sets everything up for script for when things are about to get started.
    This helps with getting the player's enviroment, information, and achievements set up. 
    */
    void Start()//auto generated by unity
    {
        player = GameObject.Find("Player");
        pObj = new PlayerObject(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), weight );
        
        //set up the mesh to be used
        //FindObjectOfType<MapDisplay>().textureRender
        Vector3 mP = GameObject.Find("MeshObj").transform.position;
        Vector3[] mV = FindObjectOfType<MapDisplay>().meshFilter.mesh.vertices;
        Vector3 bounds = GameObject.Find("MeshObj").transform.localScale;
        
        pObj.setMeshInfoUp(mV, mP, bounds);

        //put the player past the border of the mesh
        player.transform.position = pObj.meshPosition + new Vector3(50*25, (mV[(mV.Length)/2].y*bounds.y), 50*25);
        
        pObj.Position = player.transform.position;
        orgPos = player.transform.position;
        
        //filter this out for things that are not items to collide with, static amount of items
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().
                                                            Where(item => item.tag == "Item").ToArray();
        
        shootItem = GameObject.Find("Sphere");//what player will be able to shoot
        pickedUpItem = shootItem; //just to get the demo set up real quick
        //get the picked up info set up
        pickedUpItem.GetComponent<ItemPhysics>().setPickedUpItem(true);
        pickedUpItem.GetComponent<ItemPhysics>().getItem().Position.y = -100;
        hasJumped = false;
        //set the achievement checkers and counters for it up
        setUpAchievements();
        if(speed < 5) speed = 5;
    }

    /*Description: This will update every frame of the game, checking to see if certian keys are hit or get the movement translated
    into the game every move is called by user. Basically, how the user interacts with the game. 
    */
    void Update()//this was generated by unity
    {
        //how the achievements, individual checks and checking previous achievement to see if it was fulfilled
        if(!doubleJumpAchieve && jumpCount >= 10) doubleJumpAchieve = true; //must jump certain amount to get double jump
        //must walk a certain distance to get speed boost
        else if(doubleJumpAchieve && !speedUpMovementAcheive && amountWalked >= 100) speedUpMovementAcheive = true;
        //must throw a certain amount as well
        else if(speedUpMovementAcheive && !shooterAcheive && thrownCount >= 5)shooterAcheive = true;
        //must get all previous acheievements to get this shooter ability
        else if(!teleportAchieve && shooterAcheive) teleportAchieve = true;
        //must use all achievement power up to get the dash move
        else if(teleportCount >= 1 && doubleJumpCount >= 3 && speedBoostCount >= 5 && shootAmount >= 7) dashMoveAchieve = true;
        //get the amount walked from spawn
        if(!doubleJumpAchieve) amountWalked += findVectorDiff(orgPos, player.transform.position);

        //Teleport 
        pObj.collide(itemObjects);//collisions with items
        //need to set up picking up and dropping items
        //for picking up items in game
        if((Input.GetKey(KeyCode.P))){
            GameObject[] closeObjs = allObjects.Where(x => PlayerObject.getDistance(pObj.Position, x.transform.position) < 2).ToArray();
            
            if(closeObjs.Length != 0){
                //move item far away, disappear and then store it to drop later
                pickedUpItem = closeObjs[0];
                closeObjs[0].GetComponent<ItemPhysics>().setPickedUpItem(true);
                closeObjs[0].GetComponent<ItemPhysics>().getItem().Position.y = -100;
            }
        }
        //if user hits d key and has something to drop
        if((Input.GetKey(KeyCode.D)) && pickedUpItem != null){
            pickedUpItem.GetComponent<ItemPhysics>().getItem().Position = pObj.Position + new Vector3(1, 1, 1);
            pickedUpItem.GetComponent<ItemPhysics>().setPickedUpItem(false);
            pickedUpItem = null;
        }

        //if user wants to throw what they have in their hand, must do this to get the achievements
        if((Input.GetKey(KeyCode.T)) && pickedUpItem != null){
            pickedUpItem.GetComponent<ItemPhysics>().getItem().Position = pObj.Position + player.transform.forward;;
            pickedUpItem.GetComponent<ItemPhysics>().setPickedUpItem(false);
            Vector3 throwSpeed = transform.forward * 10;
            ItemObject.getPushed(pickedUpItem.GetComponent<ItemPhysics>().getItem(), throwSpeed);
            thrownCount += 1;
            pickedUpItem = null;
        }
        //move the player based on the key inputs and borders
        pObj.move(pObj.zBounds, pObj.xBounds, pObj.meshPosition, player.transform.forward, player.transform.right, speed);
        //looks at location at all time to determine height, based on the vertices
        float temp = pObj.getHeight(pObj.bounds, pObj.meshVertices, pObj.meshPosition);
        if(pObj.Position.y - temp < 1 && pObj.Velocity.y <= 0){
             //use mesh vertices to determine where we should be above of   
            pObj.Position.y = temp + .9f;
            pObj.isGrounded = true;
        }
        else pObj.isGrounded = false;

        //if player wants to jump, then jump
        if((Input.GetKey("space") && Input.anyKey) && pObj.isGrounded){
            pObj.jump();
            hasJumped = true;
            jumpCount += 1;
        }

        //double jump, if player is in middle of jumping, they will be able to jump again if still in the air
        if((Input.GetKey("space") && Input.anyKey) && hasJumped && doubleJumpAchieve){
            GameObject theCam = GameObject.Find("Camera");
            pObj.jump();
            hasJumped = false;
            theCam.GetComponent<CammeraFollow>().zoomOut(); //will always zoom out to show the user it's jumping twice
            //user must undo it
            doubleJumpCount += 1;
        }

        //Speed up Movement that will cause the camera to zoom in
        if(Input.GetKey(KeyCode.S) && speedUpMovementAcheive){
            GameObject theCam = GameObject.Find("Camera");
            if(oldSpeed == speed) speed = speed * 2;
            else speed = oldSpeed;
            theCam.GetComponent<CammeraFollow>().zoomIn();
            speedBoostCount += 1;
        }

        //Dash/Boost move for when jumping, short speed boost
        if(!pObj.isGrounded && Input.GetKey(KeyCode.B) && dashMoveAchieve){
            Vector3 forwardPos = player.transform.forward;
            pObj.Velocity = new Vector3(forwardPos.x * speed * 10, 0, forwardPos.z * speed * 10);
            teleportCount += 1;
        }

        //Teleport move, will teleport you forward a certain distance based off of where forward is.
        if(pObj.isGrounded && Input.GetKey(KeyCode.T) && (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && teleportAchieve){
            Vector3 forwardPos = player.transform.forward;
            pObj.Position = pObj.Position + new Vector3(forwardPos.x * 100, 0 , forwardPos.z * 100);
            float heightTemp = pObj.getHeight(pObj.bounds, pObj.meshVertices, pObj.meshPosition);
            pObj.Position.y = heightTemp + 10;
            player.transform.position = pObj.Position;
        }

        //shoot spheres at will, a shooter method with unlimited ammo
        if(Input.GetKey(KeyCode.S) && (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && shooterAcheive){
            GameObject shootSphere = Instantiate(shootItem);
            Vector3 forwardPos = player.transform.forward;
            shootSphere.GetComponent<ItemPhysics>().getItem().Position = pObj.Position + new Vector3(forwardPos.x * 2, 0 , forwardPos.z * 2);
            Vector3 shootSpeed = transform.forward * 20;
            ItemObject.getPushed(pickedUpItem.GetComponent<ItemPhysics>().getItem(), shootSpeed);
            shootAmount += 1;
        }

        //Zoom in if user wants to, based off of the type of control button and letter z
        if(Input.GetKey(KeyCode.Z) && (Input.GetKey(KeyCode.RightControl))){
            GameObject theCam = GameObject.Find("Camera");
            theCam.GetComponent<CammeraFollow>().zoomIn();
        }
        else if(Input.GetKey(KeyCode.Z) && (Input.GetKey(KeyCode.LeftControl))){
            GameObject theCam = GameObject.Find("Camera");
            theCam.GetComponent<CammeraFollow>().zoomOut();
        }

        //If the user wants to invert camera, for some reason, then just right control and hit I. 
        if(Input.GetKey(KeyCode.I) && (Input.GetKey(KeyCode.RightControl))){
            GameObject theCam = GameObject.Find("Camera");
            theCam.GetComponent<CammeraFollow>().invertCamera();
        }
        if(pObj.isGrounded){//if player is on the ground, then we should be able to move still
            pObj.Velocity.y = 0;
            hasJumped = false;
            player.transform.position = pObj.physWorld.move(Time.deltaTime, pObj, pObj.isGrounded);
        }
        //if not on the ground, then player must fall
        else player.transform.position = pObj.physWorld.move(Time.deltaTime, pObj, pObj.isGrounded);

        //if we are moving, got to slow us down
        if(pObj.Velocity.x != 0){
            pObj.Velocity.x = pObj.friction(pObj.Velocity.x, 0.5f);
            
        }
        if(pObj.Velocity.z != 0){
            pObj.Velocity.z = pObj.friction(pObj.Velocity.z, 0.5f);
        }
    }

    /* Description: Find the absolute difference between two vectors
    Parameters: Vector3 a: The vector we are subtracting from
                Vector3 b: The vector we are subtracting with, basically b-a
    Return: The total difference between the vectors b and a in the form of a float
    */
    public float findVectorDiff(Vector3 a, Vector3 b){ 
        float vectDiffX = Math.Abs(b.x - a.x);
        float vectDiffY = Math.Abs(b.y - a.y);
        float vectDiffZ = Math.Abs(b.z - a.z);
        return vectDiffX + vectDiffY + vectDiffZ;
    }

    /* Description: Sets up all the achievements for the outside area of the game, and the counters to figure out if they are accomplished.
    */
    private void setUpAchievements(){
        doubleJumpAchieve = false;
        speedUpMovementAcheive = false;
        teleportAchieve = false;
        shooterAcheive = false;
        dashMoveAchieve = false;
        jumpCount = 0;
        amountWalked = 0;
        thrownCount = 0;
        doubleJumpCount = 0;
        teleportCount = 0;
        speedBoostCount = 0;
        shootAmount = 0;
    }
}
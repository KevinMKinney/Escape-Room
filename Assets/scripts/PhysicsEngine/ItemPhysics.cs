using System.Collections;
using System.Collections.Generic;
using UnityEngine;//everything before this is auto generated
using System.Linq;//need this for functional refactoring
using System;//need this for math functions

/* Description: This class helps with giving items some way to interact with other items and the enviorment 
    */
public class ItemPhysics : MonoBehaviour //unity generated
{
    private ItemObject item;//the item itself
    public bool isBouncable;//for to determine if item should bounce
    public float weight;//to find the mass
    public bool isStaticStat;//for when things shouldn't move
    private GameObject[] allObjects;//for collisions
    bool isPickedUp;

    /* Description: Getter method to get the item object with this specfic info
    Return: Returns the item object of the specfic of gameobject we have given the item physics to
    */
    public ItemObject getItem(){
        return item;
    }

    /*Description: Set the status for if item is picked up, changing how an item will be 
    */
    public void setPickedUpItem(bool b){
        isPickedUp = b;
    }

    /*Description: get the status picked up item
        Return: Returns the status of wheter or not an item is picked up or not
    */
    public bool getPickedUpItem(){
        return isPickedUp;
    }

    /* Description: Sets all the intial values and finds all collidabel objects before the first fram
    */
    void Start(){//auto generated line, things inside is not though
        item = new ItemObject(transform.position, new Vector3(0, 0, 0), new Vector3(0, 0, 0), weight, isStaticStat);
        Vector3 mP = GameObject.Find("MeshObj").transform.position;
        Vector3[] mV = FindObjectOfType<MapDisplay>().meshFilter.mesh.vertices;
        Vector3 bounds = GameObject.Find("MeshObj").transform.localScale;
        item.setMeshInfoUp(mV, mP, bounds);
        item.setDynamicStat(true);
        
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.tag == "Item" && 
        !ItemObject.itemEqual(x.GetComponent<ItemPhysics>().getItem(), item)).ToArray();
        item.physWorld.setFriction(weight, 1);
        isPickedUp = false;
        
    }

    /* Description: Updates every frame and does the intended things to interact with the world, such as sliding 
                    and stopping or staying above the mesh map
    */
    void Update() //unity generated
    {
        
        if(!isPickedUp){//if item is not picked up
            //condition is needed when item is picked up
            transform.position = item.Position;
            collide(allObjects);
            //temporary height of where item is based on mesh
            float temp = item.getHeight(item.bounds, item.meshVertices, item.meshPosition);
            if(item.Position.y - temp < (transform.localScale.y/2) && item.Velocity.y <= 0){
                //use mesh vertices to determine where we should be above of
                item.Position.y = temp + (transform.localScale.y/2) - 0.1f;
                item.isGrounded = true;
            }
            else item.isGrounded = false;//if item is not on the ground, fall
            //bounce stuff
            if(item.isGrounded && isBouncable){
                float tempSpeed = (float) Math.Abs(item.Velocity.y)/1.5f;
                if(tempSpeed > 0.01f){
                    item.Velocity.y = tempSpeed;
                    item.isGrounded = false;
                }
                else{
                    item.Velocity.y=0;
                } 
            }
            //if it doesn't bounce
            else if(item.isGrounded && !isBouncable) item.Velocity.y = 0;
            
            if(!item.isGrounded){//if it is falling
                item.Position = item.physWorld.move(Time.deltaTime, item, item.isGrounded);
            }
            if(item.getDynamicStat()){//if it is moving
                item.Position = item.physWorld.move(Time.deltaTime, item, item.isGrounded);
                //boundary checks for when object is moving
                if(item.Position.x >= ((item.xBounds-0.05f)+item.meshPosition.x)){
                    item.Position.x = (item.xBounds-0.05f)+item.meshPosition.x;
                    item.Velocity.x *= -1;
                }
                else if(item.Position.x <= (item.meshPosition.x + 0.05f)){
                    item.Position.x = (item.meshPosition.x + 0.05f);
                    item.Velocity.x *= -1;
                }

                if(item.Position.z >= ((item.zBounds-0.05f)+item.meshPosition.z)){
                    item.Position.z = (item.zBounds-0.05f)+item.meshPosition.z;
                    item.Velocity.z *= -1;
                }
                else if(item.Position.z <= (item.meshPosition.z + .05f)){
                    item.Position.z = (item.meshPosition.z + .05f);
                    item.Velocity.z *= -1;
                }
                
                if(item.Velocity.x != 0 || item.Velocity.z != 0){
                    item.Velocity.x = item.friction(item.Velocity.x, item.physWorld.getFrictionForce());
                    item.Velocity.z = item.friction(item.Velocity.z, item.physWorld.getFrictionForce());
                }
                else item.setDynamicStat(false);
            }
        }
        else transform.position = item.Position;
    }
   
    
    /*Description: Collision detection and response function, for static and dynamic collisions
    Parameters: GameObject[] a: All Objects that are collidiable with this item, which are just other items that have item physics.
    */
    public void collide(GameObject[] a){
        //filter out items that are just too far away or not able to be picked up
        GameObject[] closeItems = a.Where(x => ItemObject.getDistance(item.Position, x.transform.position) < 0.8 &&
                                            !x.GetComponent<ItemPhysics>().getPickedUpItem()).ToArray();
        if(closeItems.Length > 0 && item.getDynamicStat()){//if we are close to objects and are moving
            //filter out items that are not moving before collisions when one item is static and the other is dynamic
            
            closeItems = closeItems.Where(i=> !i.GetComponent<ItemPhysics>().getItem().getDynamicStat()).ToArray();
            //change the position of collided items to help with math
            
            closeItems.Select(i=> i.GetComponent<ItemPhysics>().getItem().Position += item.Velocity * Time.deltaTime);
           
            foreach(GameObject i in closeItems){//have to use foreach loop because ain't changing the value from it
                ItemObject.getPushed(i.GetComponent<ItemPhysics>().getItem(), item.Velocity*0.5f);
                item.Velocity *= -0.5f;//make item goes backward
            }
        }       
    }
}
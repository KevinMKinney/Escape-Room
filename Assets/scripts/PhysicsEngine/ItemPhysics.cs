using System.Collections;
using System.Collections.Generic;
using UnityEngine;//everything before this is auto generated
using System.Linq;//need this for functional refactoring
using System;//need this for math functions

public class ItemPhysics : MonoBehaviour
{
    private ItemObject item;//the item itself
    public bool isBouncable;//for to determine if item should bounce
    public float weight;//to find the mass
    public bool isStaticStat;//for when things shouldn't move
    private GameObject[] allObjects;//for collisions
    bool isPickedUp;

    //to get the item
    public ItemObject getItem(){
        return item;
    }

    

    //to determine if items are equal or not, for filtering
    public bool itemEqual(ItemObject a, ItemObject b){
        if(a != null && b!= null){
            if(a.Position == b.Position && a.mass == b.mass){
                return true;
            }
            else return false;
        }
        else return false;
    }

    //set the status for if item is picked up
    public void setPickedUpItem(bool b){
        isPickedUp = b;
    }

    //get the status picked up item
    public bool getPickedUpItem(){
        return isPickedUp;
    }
    void Start(){//auto generated line, things inside is not though
        item = new ItemObject(transform.position, new Vector3(0, 0, 0), new Vector3(0, 0, 0), weight, isStaticStat);
        Vector3 mP = FindObjectOfType<MapDisplay>().textureRender.transform.position;
        Vector3[] mV = FindObjectOfType<MapDisplay>().meshFilter.mesh.vertices;
        Vector3 bounds = FindObjectOfType<MapDisplay>().textureRender.transform.localScale;
        item.setMeshInfoUp(mV, mP, bounds);
        item.setDynamicStat(true);
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().
        Where(x => x.tag == "Item" && !itemEqual(x.GetComponent<ItemPhysics>().getItem(), item)).ToArray();
        isPickedUp = false;
        
    }

    void Update()
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
                    item.Velocity.x = item.friction(item.Velocity.x, 0.05f);
                    item.Velocity.z = item.friction(item.Velocity.z, 0.05f);
                }
                else item.setDynamicStat(false);
            }
        }
        else transform.position = item.Position;
    }
   
    
    //collision detection and response function, for static and dynamic collisions
    public void collide(GameObject[] a){
        //filter out items that are just too far away
        GameObject[] closeItems = a.Where(x => ItemObject.getDistance(item.Position, x.transform.position) < 0.8).ToArray();
        if(closeItems.Length > 0 && item.getDynamicStat()){//if we are close to objects and are moving
            //filter out items that are not moving before collisions when one item is static and the other is dynamic
            
            closeItems = closeItems.Where(i=> !i.GetComponent<ItemPhysics>().getItem().getDynamicStat()).ToArray();
            //change the position of collided items to help with math
            
            closeItems.Select(i=> i.GetComponent<ItemPhysics>().getItem().Position += item.Velocity * Time.deltaTime);
            
            foreach(GameObject i in closeItems){//have to use foreach loop because ain't changing the value from it
                ItemObject.getPushed(i.GetComponent<ItemPhysics>().getItem(), item.Velocity*0.5f);
                //i.GetComponent<ItemPhysics>().getPushed();
                item.Velocity *= -0.5f;//make item goes backward
            }
        }       
    }
}
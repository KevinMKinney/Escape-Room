using System.Collections;
using System.Collections.Generic;
using UnityEngine;//everything before this is auto generated
using System.Linq;//need this for functional refactoring
using System;//need this for math functions

public class PlayerPhysics : MonoBehaviour, Collisions
{
    private GameObject player;
    private GameObject pickedUpItem;
    public float weight;
    private PlayerObject pObj;
    private GameObject[] allObjects;//for collisions

    
    // Start is called before the first frame update
    void Start()//auto generated by unity
    {
        player = GameObject.Find("Player");
        pObj = new PlayerObject(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), weight );
        
        //set up the mesh to be used
        Vector3 mP = FindObjectOfType<MapDisplay>().textureRender.transform.position;
        Vector3[] mV = FindObjectOfType<MapDisplay>().meshFilter.mesh.vertices;
        Vector3 bounds = FindObjectOfType<MapDisplay>().textureRender.transform.localScale;
        pObj.setMeshInfoUp(mV, mP, bounds);

        //put the player past the border of the mesh
        player.transform.position = pObj.meshPosition + new Vector3(0.05f, 0, 0.05f);
        pObj.Position = player.transform.position;
        //filter this out for things that are not items to collide with, static amount of items
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().
                                                            Where(item => item.tag == "Item").ToArray();
        pickedUpItem = null;
       
    }

    void Update()//this was generated by unity
    {
        collide(allObjects);//collisions with items
        //need to set up picking up and dropping items
        //for picking up items in game
        if((Input.GetKey(KeyCode.P))){
            GameObject[] closeObjs = allObjects.Where(x => pObj.getDistance(pObj.Position, x.transform.position) < 2).ToArray();
            if(closeObjs.Length != 0){
                //move item far away and then store it to drop later
                Debug.Log("Hello");
                //make item disappear
                pickedUpItem = closeObjs[0];
                closeObjs[0].GetComponent<ItemPhysics>().getItem().Position.x = 20;
            }
        }
        //for dropping items
        //if user hits d key and has something to drop
        if((Input.GetKey(KeyCode.D)) && pickedUpItem != null){
            pickedUpItem = null;
        }

        //move the player based on the key inputs and borders
        pObj.move(pObj.zBounds, pObj.xBounds, pObj.meshPosition);
        //looks at location at all time to determine height, based on the vertices
        float temp = pObj.getHeight(pObj.bounds, pObj.meshVertices, pObj.meshPosition);
        if(pObj.Position.y - temp < 1 && pObj.Velocity.y <= 0){
             //use mesh vertices to determine where we should be above of   
            pObj.Position.y = temp + .9f;
            pObj.isGrounded = true;
        }
        //if player wants to jump, then jump
        if((Input.GetKey("space") && Input.anyKey) && pObj.isGrounded){
            pObj.jump();
        }
        if(pObj.isGrounded){
            pObj.Velocity.y = 0;
            player.transform.position = Dynamics.move(Time.deltaTime, pObj, pObj.isGrounded);
        }
        else player.transform.position = Dynamics.move(Time.deltaTime, pObj, pObj.isGrounded);

        //if we are moving, got to slow us down
        if(pObj.Velocity.x != 0){
            pObj.Velocity.x = friction(pObj.Velocity.x);
        }
        if(pObj.Velocity.z != 0){
            pObj.Velocity.z = friction(pObj.Velocity.z);
        }
    }

    //Slowing down the player, simple for now
    public float friction(float v){
        if(v < 0){
           return v + 0.5f;
        }
        else return v - 0.5f;
    }

    //help with dealing with collisions with items
    public void collide(GameObject[] a){
        //filter items that are close to player and are static, or unmovable
        GameObject[] closeObjs = allObjects.Where(x => pObj.getDistance(pObj.Position, x.transform.position) < 1.5 &&
                                                    !x.GetComponent<ItemPhysics>().getItem().getStaticStat()).ToArray();
        /*then we have to use the push method for the item. Can't use select or aggregate because 
        it is a void type method*/
        foreach(GameObject item in closeObjs){
            item.GetComponent<ItemPhysics>().getPushed((pObj.Velocity*5));
        }
    }
}
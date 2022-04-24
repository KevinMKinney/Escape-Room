using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;//need this for functional refactoring
using System;//need this for math functions


public class Object{
    public Vector3 Position;//position vector
    public Vector3 Velocity;//velocity vector
    public Vector3 Force;//force vector, usually all zeroes
    public bool isGrounded;//if object is on the ground
    public float mass; //mass of the object, used for collisions and gravity

    public Vector3[] meshVertices;//for object to mesh interaction
    public Vector3 meshPosition;
    public Vector3 bounds;//for object to mesh interaction, and boundaries
    //both are for boundaries
    public float zBounds;//boundary of z in terms of mesh
    public float xBounds;//boundary of x in terms of mesh

    public Dynamics physWorld = new Dynamics();

    //set the mesh info up for the objects to use
    public void setMeshInfoUp(Vector3[] _mV, Vector3 mP, Vector3 _bounds){
        meshVertices = _mV;
        meshPosition=mP;
        bounds = _bounds;
        zBounds = boundsLength(bounds.z);
        xBounds = boundsLength(bounds.x);
    }


    /* Helps get the height that object needs to be based on map
    */
    public float getHeight(Vector3 mapSize, Vector3[] mv, Vector3 meshPos){
        //set the z and x values to fit the vertexes, meaning set z and x to zero when they are at begining
        float zSetToMapPos = (Position.z-meshPos.z);
        float xSetToMapPos = (Position.x-meshPos.x);

        //error with going out of boundaries, need a way to stop that from happening
        double zCoord = Math.Floor(zSetToMapPos/mapSize.z);
        double xCoord = Math.Floor(xSetToMapPos/mapSize.x);

        //y value for the vertex up one in x
        float y01 = mv.First(i => i.x == (xCoord+1) && i.z == zCoord).y;

        //the y value for the vertex up one in z
        float y10 = mv.First(i => i.x == (xCoord) && i.z == (zCoord+1)).y;

        //the y value for the vertex player is at now
        float y00 = mv.First(i => i.x == xCoord && i.z == zCoord).y;

        //the z and x position in terms of the mape size's scale
        float zScalePos = (zSetToMapPos/(mapSize.z)-(float)zCoord);
        float xScalePos = (xSetToMapPos/(mapSize.x)-(float)xCoord);

        //get the new y position based on the math
        float newy = (zScalePos*(y10-y00)) + (xScalePos*(y01 - y00))+y00;
        
        return (mapSize.y * newy + meshPos.y);
    }
    
    //Calculates the bound length of the mesh, x and z
    public static float boundsLength(float num){
        if(num > 0) return (num - 1)*num;
        return 0;
    } 

    //to get the distance between two objects, for collisions
    public static double getDistance(Vector3 object1, Vector3 object2){
        double distanceX = Math.Pow((object2.x - object1.x), 2);
        double distanceY = Math.Pow((object2.y - object1.y), 2);
        double distanceZ = Math.Pow((object2.z - object1.z), 2);
        return Math.Sqrt(distanceX+distanceY+distanceZ);
    }

    public float friction(float v, float f){
        if(Math.Abs(v) >= 0.5f){
            if(v < 0) return v + f;
            return v - f;   
        }
        return 0;
    }
}
//we have a PlayerObject, which is an object, but does more things
public class PlayerObject: Object, Collisions{
    //player object constructor
    public PlayerObject(Vector3 _p, Vector3 _v, Vector3 _f, float _m){
        Position = _p;
        Velocity = _v;
        Force = _f;
        mass = _m;
    }

     //used to help player jump
    public void jump(){
        Velocity.y = 10;
        isGrounded = false;
    }

    /*
        Info: Helps calculate where things should move and be at

        Parameters:
        arrow is the type of keyboard input
        Vector is the new velocity vector
        i is the current location
        j is the bound
    */
    public void moveHelper(Vector3 newV, float i, float j){
        
        /*bool isPos = false;//to detrmine where we should check, uper bounds or lower bounds
        if(newV.z >= 0 && newV.x >= 0){
            isPos = true;
        }
        //change the position of the player, using velocity
        if(((isPos && i < j) || (!isPos && i > j))){
            //Position += (newV * Time.deltaTime); //old method, bad
            
        }*/
        Velocity.z = newV.z;
        Velocity.x = newV.x;
        
        
    }
    
    //used to help player move in game
    public void move(float zB, float xB, Vector3 meshPos, Vector3 moveAngle, Vector3 posR){
        //get bounds for movement
        float zUpBounds = (zB-0.05f) + meshPos.z;
        float zLowBounds = meshPos.z + 0.05f;
        float xUpBounds = (xB-0.05f + meshPos.x);
        float xLowBounds = meshPos.x + 0.05f;

        //move up 
        if(Input.GetKey(KeyCode.UpArrow)) moveHelper(moveAngle *2, Position.z, zUpBounds);
        //move back
        if(Input.GetKey(KeyCode.DownArrow)) moveHelper(moveAngle * -2, Position.z, zLowBounds);
        //move right
        if(Input.GetKey(KeyCode.RightArrow))  moveHelper(posR * 2, Position.x, xUpBounds);
        //move left
        if(Input.GetKey(KeyCode.LeftArrow)) moveHelper(posR * -2, Position.x, xLowBounds);

        //if we are past a bound, then we must be put back to the other area
        if(Position.x >= xUpBounds) Position.x = xUpBounds;
        else if(Position.x <= xLowBounds) Position.x = xLowBounds;
        if(Position.x >= zUpBounds) Position.z = zUpBounds;
        else if(Position.z <= zLowBounds) Position.z = zLowBounds;
    }
 
        
    /*public float friction(float v, float f){
        if(v < 0){
           return v + f;
        }
        else if(v == 0) return 0;
        else return v - f;
    
    }*/

    //help with dealing with collisions with items
    public void collide(ItemObject[] i){
        //filter items that are close to player and are static, or unmovable
        ItemObject[] closeObjs = i.Where(x => PlayerObject.getDistance(Position, x.Position) < 1.5 &&
                                                    !x.getStaticStat()).ToArray();
        
        /*then we have to use the push method for the item. Can't use select or aggregate because 
        it is a void type method*/
        foreach(ItemObject item in closeObjs){
           ItemObject.getPushed( item, (Velocity*5));
        }
    }

}


//make a item object class, that has collisions mechanics
public class ItemObject: Object{
    bool isDynamic;//to tell if it is moving somewhere
    float coefficientFriction;
    bool isStatic; //to determine if it can move at all or not
    //item object constructor
     public ItemObject(Vector3 _p, Vector3 _v, Vector3 _f, float _m, bool _s){
        Position = _p;
        Velocity = _v;
        Force = _f;
        mass = _m;
        isStatic = _s;
    }
    
    //getter and setters for private item variables
    public void setDynamicStat(bool b){
        isDynamic = b;
    }
    public bool getDynamicStat(){
        return isDynamic;
    }
    public bool getStaticStat(){
        return isStatic;
    }

    

    //to push the item
    public static void getPushed(ItemObject item, Vector3 newV){
        item.setDynamicStat(true);
        //only x and z though, y is special case
        item.Velocity.x = newV.x;
        item.Velocity.z = newV.z;
    }

    //Item objects equal method help locate specfic items we are looking for, since every item will
    //be different by position or weight, we will use that as a constant for when looking at items
    public static bool itemEqual(ItemObject a, ItemObject b){
        if(a.Position == b.Position && a.mass == b.mass){
            return true;
        }
        else return false;
    }

    public void collide(ItemObject[] a){
        //filter out items that are just too far away
        ItemObject[] closeItems = a.Where(x => ItemObject.getDistance(Position, x.Position) < 0.8).ToArray();
        if(closeItems.Length > 0 && getDynamicStat()){//if we are close to objects and are moving
            //filter out items that are not moving before collisions when one item is static and the other is dynamic
            
            closeItems = closeItems.Where(i=> !i.getDynamicStat()).ToArray();
            //change the position of collided items to help with math
            
            closeItems.Select(i=> i.Position += Velocity * Time.deltaTime);
            
            foreach(ItemObject i in closeItems){//have to use foreach loop because ain't changing the value from it
                ItemObject.getPushed(i, Velocity*0.5f);
                //i.GetComponent<ItemPhysics>().getPushed();
                Velocity *= -0.5f;//make item goes backward
            }
        }       
    }

    
}

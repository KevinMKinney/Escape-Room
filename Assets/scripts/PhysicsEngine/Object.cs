using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;//need this for functional refactoring
using System;//need this for math functions

//this is a class to help store information about objects, such as items and the 
//a tutorial for desinging a physics was used: https://blog.winter.dev/2020/designing-a-physics-engine/
//It was done in C++ and I took inspiration from it
public class Object{
    public Vector3 Position;
    public Vector3 Velocity;
    public Vector3 Force;
    public bool isGrounded;
    public float mass; 

    public Vector3[] meshVertices;//for object to mesh interaction
    public Vector3 meshPosition;
    public Vector3 bounds;//for object to mesh interaction, and boundaries
    //both are for boundaries
    public float zBounds;
    public float xBounds;

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
        return (mapSize.y * newy);
    }
    
    //Calculates the bound length of the mesh, x and z
    public static float boundsLength(float num){
        return (num - 1)*num;
    } 

    //to get the distance between two objects, for collisions
    public double getDistance(Vector3 object1, Vector3 object2){
        double distanceX = Math.Pow((object2.x - object1.x), 2);
        double distanceY = Math.Pow((object2.y - object1.y), 2);
        double distanceZ = Math.Pow((object2.z - object1.z), 2);
        return Math.Sqrt(distanceX+distanceY+distanceZ);
    }
}
//we have a PlayerObject, which is an object, but does more things
public class PlayerObject: Object{
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
    public void moveHelper(KeyCode arrow, Vector3 newV, float i, float j){
        bool isPos = false;//to detrmine where we should check, uper bounds or lower bounds
        if(newV.z >= 0 && newV.x >= 0){
            isPos = true;
        }
        //change the position of the player, using velocity
        if(Input.GetKey(arrow) && ((isPos && i < j) || (!isPos && i > j))){
            //Position += (newV * Time.deltaTime);
            if(newV.z != 0) Velocity.z = newV.z;
            else Velocity.x = newV.x;
            
        }
        //if we are past a bound, then we must be put back to the other area
        else if((isPos && i >= j) || (!isPos && i <= j)){
            Vector3 newPos = Position;
            if(newV.z != 0) newPos.z = j;
            else newPos.x = j;
            Position = newPos;
        }
    }
    
    //used to help player move in game
    public void move(float zB, float xB, Vector3 meshPos){
        //get bounds for movement
        float zUpBounds = (zB-0.05f) + meshPos.z;
        float zLowBounds = meshPos.z + 0.05f;
        float xUpBounds = (xB-0.05f + meshPos.x);
        float xLowBounds = meshPos.x + 0.05f;

        //move up 
        moveHelper(KeyCode.UpArrow, new Vector3(0, 0, 2), Position.z, zUpBounds);
        //move back
        moveHelper(KeyCode.DownArrow, new Vector3(0, 0, -2), Position.z, zLowBounds);
        //move right
        moveHelper(KeyCode.RightArrow, new Vector3(2, 0, 0), Position.x, xUpBounds);
        //move left
        moveHelper(KeyCode.LeftArrow, new Vector3(-2, 0, 0), Position.x, xLowBounds);
    }
}


//make a item object class, that has collisions mechanics
public class ItemObject: Object{
    bool isDynamic;//to tell if it is moving somewhere
    float coefficientFriction;
    bool isStatic; //to determine if it can move at all or not
     public ItemObject(Vector3 _p, Vector3 _v, Vector3 _f, float _m, bool _s){
        Position = _p;
        Velocity = _v;
        Force = _f;
        mass = _m;
        isStatic = _s;
    }
    
    public void setDynamicStat(bool b){
        isDynamic = b;
    }
    public bool getDynamicStat(){
        return isDynamic;
    }
    public bool getStaticStat(){
        return isStatic;
    }
}

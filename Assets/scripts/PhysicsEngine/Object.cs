using System.Collections;
using System.Collections.Generic;
using UnityEngine;//unity generated above this
using System.Linq;//need this for functional refactoring
using System;//need this for math functions

/* Description: This class has all the information that any object need to interact with the world, and some methods that any object would need.
    */
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

    /* Description: Set the mesh info up for the objects to use
    Parameters: Vector3[] _mV: The mesh vertices that are needed to find height
                Vector3 mP: Where the mesh is position at in terms of the world
                Vector3 _bounds: The local scale of the mesh to help determine the bounds of the mesh so the player doesn't leave the mesh
    */
    public void setMeshInfoUp(Vector3[] _mV, Vector3 mP, Vector3 _bounds){
        meshVertices = _mV;
        meshPosition=mP;
        bounds = _bounds;
        zBounds = boundsLength(bounds.z);
        xBounds = boundsLength(bounds.x);
    }


    
    /* Description: elps get the height that object needs to be based on map
    Parameters: Vector3 mapSize: The size of the to calculate the what mesh vertex we will be looking at, and the scale of the mesh to find z and 
                                x in terms of their position/size on the mesh
                Vector3[] mv: The mesh vertices to help determine where the object should be at in terms of height
                Vector3 meshPos: The mesh position to help determine where in the mesh vertices we are looking at
    Return: Returns the height for the particular area where the player is
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
    

    /* Description: Calculates the bound length of the mesh, x and z
    Parameters: float num: The bound number we want to find, either x or z
    Return: The bound length of either x or z, and 0 if it's smaller than or equal to 0
    */
    public static float boundsLength(float num){
        if(num > 0) return (num - 1)*num;
        return 0;
    } 

    //
    /* Description: To get the distance between two objects, for collisions
    Parameters: Vector3 object1: The 1st object we will be looking at with against other objects
                Vector3 object2: the 2nd object we will be looking at, subtracting with against 1st object
    Return: Returns the total distance between the two objects based off their position
    */
    public static double getDistance(Vector3 object1, Vector3 object2){
        double distanceX = Math.Pow((object2.x - object1.x), 2);
        double distanceY = Math.Pow((object2.y - object1.y), 2);
        double distanceZ = Math.Pow((object2.z - object1.z), 2);
        
        return Math.Sqrt(distanceX+distanceY+distanceZ);
    }

    /* Description: Helps subtract some speed off by using some kind of friction force value
    Parameters: float v: The velocity value that will be slashed off if it's too high
                float f: The friction value that is being subtracted
    Return: Returns new speed that will be found after friction 
    */
    public float friction(float v, float f){
        if(Math.Abs(v) >= 0.5f){
            if(v < 0) return v + f;
            return v - f;   
        }
        return 0;
    }
}


/* Description: A player object class, that has collisions mechanics and other methods to help out, such as move */
public class PlayerObject: Object, Collisions{
    //player object constructor
    /* Description: 
   Parameters: Vector3 _p : The position vector
                Vector3 _v: the velocity vector
                Vector3 _f: the force vector
                float _m: The mass of the item
    */
    public PlayerObject(Vector3 _p, Vector3 _v, Vector3 _f, float _m){
        Position = _p;
        Velocity = _v;
        Force = _f;
        mass = _m;
    }

    /* Description: Used to help player jump
    */     
    public void jump(){
        Velocity.y = 10;
        isGrounded = false;
    }

    /* Description: Helped with figuring out where to move, change the velcoities based on key, where the bounds are and make sure no bounds
    were left
    Parameters: Vector3 newV: The new velocity for the move
                float i: The current value of the player
                float j: The bound value
    */
    public void moveHelper(Vector3 newV, float i, float j){
            bool isPos = false;//to detrmine where we should check, uper bounds or lower bounds
            if(newV.z >= 0 && newV.x >= 0){
                isPos = true;
            }
            //change the position of the player, using velocity
            if(((isPos && i < j) || (!isPos && i > j))){
                //Position += (newV * Time.deltaTime); //old method, bad
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
   

    /* Description: Used to help player move in game
    Parameters: float zB: The z upper mesh bounds
                float xB: The x upper mesh bound
                Vector3 meshPos: the mesh position
                Vector3 moveAngle: The forward angle for the player, where player is looking at
                Vector3 posR: The angle at which is to the right of the player, help player move right or left based off player visual
                int speed: The speed of the player, set by the player
    */
    public void move(float zB, float xB, Vector3 meshPos, Vector3 moveAngle, Vector3 posR, int speed){
        //get bounds for movement
        float zUpBounds = (zB-0.05f) + meshPos.z;
        float zLowBounds = meshPos.z + 0.05f;
        float xUpBounds = (xB-0.05f + meshPos.x);
        float xLowBounds = meshPos.x + 0.05f;

        //Helps calculate where things should move and be at
        //move up 
        if(Input.GetKey(KeyCode.UpArrow)) Velocity = new Vector3(moveAngle.x * speed, 0, moveAngle.z*speed);
        //move back
        if(Input.GetKey(KeyCode.DownArrow)) Velocity = new Vector3(moveAngle.x * -speed, 0, moveAngle.z*-speed);
        //move right
        if(Input.GetKey(KeyCode.RightArrow))  Velocity = new Vector3(posR.x * speed, 0, posR.z*speed);
        //move left
        if(Input.GetKey(KeyCode.LeftArrow)) Velocity = new Vector3(posR.x * -speed, 0, posR.z*-speed);

        //if we are past a bound, then we must be put back to the other area
        if(Position.x >= xUpBounds) Position.x = xUpBounds;
        else if(Position.x <= xLowBounds) Position.x = xLowBounds;

        if(Position.x >= zUpBounds) Position.z = zUpBounds;
        else if(Position.z <= zLowBounds) Position.z = zLowBounds;
    }
 
    /* Description: help with dealing with collisions with items
    Parameters: ItemObject[] i: All items that the object could collide with
    */
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



/* Description: An item object class, that has collisions mechanics and other methods to help out */
public class ItemObject: Object{
    bool isDynamic;//to tell if it is moving somewhere
    float coefficientFriction;
    bool isStatic; //to determine if it can move at all or not
    //
    /*Descrption:Item object constructor
    Parameters: Vector3 _p : The position vector
                Vector3 _v: the velocity vector
                Vector3 _f: the force vector
                float _m: The mass of the item
                bool _s: whether or not the item is static or not
    */
     public ItemObject(Vector3 _p, Vector3 _v, Vector3 _f, float _m, bool _s){
        Position = _p;
        Velocity = _v;
        Force = _f;
        mass = _m;
        isStatic = _s;
    }
    
    
    /* Description: Set whether an item is dynamic(can be moved) or not
    Parameters: bool b: The value that we are changing the dynamic stat of item to
    */

    public void setDynamicStat(bool b){
        isDynamic = b;
    }
    /* Description: Get whether an item is dynamic(can be moved) or not
    Return: Return whether an item is dynamic(can be moved) or not
    */
    public bool getDynamicStat(){
        return isDynamic;
    }

    /* Description: Get whether an item is static(moving) or not
    Return: Return whether an item is static(moving) or not
    */
    public bool getStaticStat(){
        return isStatic;
    }

    

    
    /* Description: //to push the item
    Parameters:  ItemObject item: the item we will be pushing 
                 Vector3 newV: The new vector we will be using when pushed
    */
    public static void getPushed(ItemObject item, Vector3 newV){
        item.setDynamicStat(true);
        //only x and z though, y is special case
        item.Velocity.x = newV.x;
        item.Velocity.z = newV.z;
    }

   
    /* Description:  Item objects equal method help locate specfic items we are looking for, since every item will
                    be different by position or weight, we will use that as a constant for when looking at items
    Parameters: ItemObject a: One item we will be looking at to compare to b
                ItemObject b: The other item to compare to b
    Return: Returns if the two items are equal or not
    */
    public static bool itemEqual(ItemObject a, ItemObject b){
        if(a.Position == b.Position && a.mass == b.mass){
            return true;
        }
        else return false;
    }

    /*Description: Collision detection and response function, for static and dynamic collisions, using item objects instead of game objects.
    Same idea as item physics collision, but this is easier to test for item collisions
    Parameters: GameObject[] a: All Objects that are collidiable with this item, which are just other items that have item physics.
    */
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
                Velocity *= -0.5f;//make item goes backward
            }
        }       
    }

    
}

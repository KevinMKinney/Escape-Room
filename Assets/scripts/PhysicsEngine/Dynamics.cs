using System.Collections;//i added all of this
using System.Collections.Generic;
using UnityEngine;

/*Description: Dynamcis class for when dynamics is involved. Certain methods and algoritms to help figure out the math
    A tutorial for desinging a physics was used: https://blog.winter.dev/2020/designing-a-physics-engine/
    It was done in C++ and I took inspiration from it, it was changed to help with my needs
*/
public class Dynamics{
    private static Vector3 gravity = new Vector3(0, -9.81f, 0);//for calculating gravity, was part of intial tutorial
    private float FrictionForce;//setting the friction force to be added for x and z
    
    /* Description: This is used to calculate movement and dynamics, depending where it's at in the world, different from tutorial. 
    Parameters: float time: The time factor to help determine velocity and position
                Object o: The object whose position that's being changed by this method, could be player or item
                bool onGround: Helps determine if we should factor in gravity or not. If it's on the gorund, then no, otherwise gravity's involved
    Return: A vector of 3 values that show what new position should be
    */
    public Vector3 move(float time, Object o, bool onGround){
        if(!onGround){
            o.Force += (o.mass * gravity);//get the force on the object
            o.Velocity += (o.Force/o.mass)*time;//get the velcoities
            o.Force = new Vector3(0, 0, 0);
        }
        o.Position += o.Velocity*time;//should always get a new position based on velocity
        return o.Position;//return this new position
    }
    
    

    /* Description: Sets what the friction force based off what the 
    Parameters: float mass: The mass of the object, need it for normal force part of equation, or the opposite of gravity force
                float coefFriction: The coefficeint of friction to help determine what the friction should be for thios equation
    */
    public void setFriction(float mass, float coefFriction){
        FrictionForce = ((mass * -9.81f) * coefFriction); 
    }
    /* Description: Returns what the the friction force is
    Return: The friction force of the item that will be used for the object
    */
    public float getFrictionForce(){
        return FrictionForce;
    }
}

//this is used to calculate movement and dynamics, different from tutorial
interface Collisions{
    void collide(ItemObject[] a);
}
using System.Collections;//i added all of this
using System.Collections.Generic;
using UnityEngine;

//Dynamcis class for when dynamics is involved.
//A tutorial for desinging a physics was used: https://blog.winter.dev/2020/designing-a-physics-engine/
//It was done in C++ and I took inspiration from it, it was changed to help with my needs
public class Dynamics{
    private static Vector3 gravity = new Vector3(0, -9.81f, 0);//for calculating gravity, was part of intial tutorial
    private float FrictionForce;//setting the friction force to be added for x and z
    //this is used to calculate movement and dynamics, different from tutorial
    public Vector3 move(float time, Object o, bool onGround){
        if(!onGround){
            o.Force += (o.mass * gravity);//get the force on the object
            o.Velocity += (o.Force/o.mass)*time;//get the velcoities
            o.Force = new Vector3(0, 0, 0);
        }
        o.Position += o.Velocity*time;//should always get a new position based on velocity
        return o.Position;//return this new position
    }
    
    

    //in order to find the friction force
    public void setFriction(float mass, float coefFriction){
        FrictionForce = ((mass * -9.81f) * coefFriction); 
    }
    public float getFrictionForce(){
        return FrictionForce;
    }
}

//an interface for objects when they collide, they must have different reactions to this
interface Collisions{
    
    void collide(ItemObject[] a);
}
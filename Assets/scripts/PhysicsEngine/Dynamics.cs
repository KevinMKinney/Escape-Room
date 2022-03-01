using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dynamcis class for when dynamics is involved.
//A tutorial for desinging a physics was used: https://blog.winter.dev/2020/designing-a-physics-engine/
//It was done in C++ and I took inspiration from it, it was changed to help with my needs
public class Dynamics{
    private static Vector3 gravity = new Vector3(0, -9.81f, 0);//for calculating gravity

    private Vector3 FrictionForce;
    //this is used to calculate movement and dynamics
    public static Vector3 move(float time, Object o, bool onGround){
        if(!onGround){
            o.Force += (o.mass * gravity);//get the force on the object
            o.Velocity += (o.Force/o.mass)*time;//get the velcoities
            o.Force = new Vector3(0, 0, 0);
        }
        o.Position += o.Velocity*time;
        return o.Position;
    }

    //in order to find the friction force
    public void setFriction(float normalForce, float coefFriction){
        FrictionForce = (normalForce*coefFriction) * new Vector3(1, 0, 1);
    }
}

//an interface for objects when they collide, they must have different reactions to this
interface Collisions{
    float friction(float v);
    void collide(GameObject[] a);
}
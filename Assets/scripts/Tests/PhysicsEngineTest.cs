using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    //acceptance test
    [Test]
    public void GetPushedChangesValue(){
        //With items get pushed, certain things should happen, such as changing veloicty and 
        //if it's dynamic/moving or not

        //setting up item to test out getPushed method
        Vector3 zVec = new Vector3(0, 0, 0);
        ItemObject i = new ItemObject(zVec, zVec, zVec, 1, false);
        i.setDynamicStat(false);

        //Test 1 is to see if it changes the velocity of the item when it hits an item
        Vector3 test1 = new Vector3(1, 2, 3);
        ItemObject.getPushed(i, test1);
        Assert.IsTrue(i.Velocity.x == 1 && i.Velocity.z == 3 && i.Velocity.y == 0);
        Assert.IsTrue(i.getDynamicStat());

        //Test 2 is to see if it changes it again
        Vector3 test2 = new Vector3(0, 1000, 4);
        ItemObject.getPushed(i, test2);
        Assert.IsTrue(i.Velocity.x == 0 && i.Velocity.z == 4 && i.Velocity.y == 0);
        Assert.IsTrue(i.getDynamicStat());
    }

    //acceptance test
    [Test]
    public void PlayerAndItemsCollisionTests(){
        //When a player gets close to an item, the item collides with that item, changing it's velocity,
        //this tests to see if that's what is happening here, with a few conditions
        ItemObject sphere = new ItemObject(new Vector3(1, 1, 2), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        ItemObject cube1 = new ItemObject(new Vector3(10, 5, 10), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        ItemObject cube2 = new ItemObject(new Vector3(1, 1, 2), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, true);
        Dynamics dynamics = new Dynamics();
        ItemObject[] allObjects = {sphere, cube1, cube2};
        PlayerObject pObj = new PlayerObject(new Vector3(0, 1, 2), new Vector3(2, 0, 3), new Vector3(0, 0, 0), 1.0f);
        
        pObj.collide(allObjects);//call the collide method

        //they all should have the velocity of (10, 0, 15) if they were to collide
        Vector3 collideVelo = new Vector3(10, 0, 15);
        //the sphere was collided and was given velocity
        Assert.IsTrue(sphere.Velocity == collideVelo);
        //the cube was not close enough to collide, so velocity given
        Assert.IsFalse(cube1.Velocity == collideVelo);
        //the was close enough, but can't be moved, so no velocity should be given
        Assert.IsFalse(cube2.Velocity == collideVelo);
        //Then we add the some friction to this new speeding item using the item friction unit
    }

    //acceptance test
    [Test]
    public void PlayerFrictionTest(){
        //tests to see if certain situations of players velocities will return the right result
        PlayerObject pObj =  new PlayerObject(new Vector3(1, 100, 3), new Vector3(2, 0, 2), new Vector3(0, 0, 0), 2.0f);
        //test input above 0 
        float test1 = 2;
        test1 = pObj.friction(test1, 0.5f);
        Assert.IsTrue(test1 == 1.5f);

        //test input below 0
        float test2 = -2;
        test2 = pObj.friction(test2, 0.5f);
        Assert.IsTrue(test2 == -1.5f);

        //test input when it's at 0
        float test3 = 0;
        test3 = pObj.friction(test3, 0.5f);
        Assert.IsTrue(test3 == 0);
    }
   

    
    

    //acceptance test
    [Test]
    public void EqualItemsTest(){
        //Should return true when we have objects with the same place and same weight
        ItemObject item1 = new ItemObject(new Vector3(1, 2, 3), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        ItemObject item2 = new ItemObject(new Vector3(1, 2, 3), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);

        bool test1 = ItemObject.itemEqual(item1, item2);
        Assert.IsTrue(test1);
    }

    //acceptance test
    //This one is part of the equal items test though
    [Test]
    public void NotEqualItemsTest(){
        //Tests to see if we have different items if it returns false or true, should be false

        ItemObject item1 = new ItemObject(new Vector3(1, 2, 3), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        //different from item1 by position
        ItemObject item2 = new ItemObject(new Vector3(3, 2, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        //different from item1 by weight
        ItemObject item3 = new ItemObject(new Vector3(1, 2, 3), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 2.0f, false);
        //different from item1 by position and weight
        ItemObject item4 = new ItemObject(new Vector3(3, 2, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 2.0f, false);

        bool test1 = ItemObject.itemEqual(item1, item2);
        Assert.IsFalse(test1);

        bool test2 = ItemObject.itemEqual(item1, item3);
        Assert.IsFalse(test2);

        bool test3 = ItemObject.itemEqual(item1, item4);
        Assert.IsFalse(test3);
    }

    //acceptance test
    [Test]
    public void ObjectBoundsLengthTest(){
        //it should give the boundlength for a mesh by doing a calculation, this tests to
        //see if the right output comes out from the input
        float test1 = Object.boundsLength(10);
        Assert.IsTrue(test1 == 90);

        float test2 = Object.boundsLength(20);
        Assert.IsTrue(test2 == 380);

        //In cases where given input is zero or negative, we should just get back zero
        float test3 = Object.boundsLength(0);
        Assert.IsTrue(test3 == 0);

        float test4 = Object.boundsLength(-100);
        Assert.IsTrue(test4 == 0);
    }

    //acceptance test
    [Test]
    public void SamePlaceDistanceTest(){
        //should get 0 distance when position vectors are at the same place
        double test1 = Object.getDistance(new Vector3(2, 3, 3), new Vector3(2,3,3));
        Assert.IsTrue(test1 == 0);

        double test2 = Object.getDistance(new Vector3(-2, -3, -3), new Vector3(-2,-3,-3));
        Assert.IsTrue(test2 == 0);
    }

    //acceptance test
    //This is part of SamePlaceDistanceTest, but with different inputs to get different outputs
    [Test]
    public void DifferentPlaceDistanceTest(){
        //All positive values for input, should get the distance of 3
        double test1 = Object.getDistance(new Vector3(4, 3, 9), new Vector3(6,5,10));
        Assert.IsTrue(test1 == 3);

        //All negative Vectors here, should have the same result as test1
        double test2 = Object.getDistance(new Vector3(-4, -3, -9), new Vector3(-6,-5,-10));
        Assert.IsTrue(test2 == 3);

        //A mix of negative and positive values here, with different values, should give a different result
        double test3 = Object.getDistance(new Vector3(-1, 6, -4), new Vector3(1,10,-8));
        Assert.IsTrue(test3 == 6);
    }

    
    //This test, ItemFrictionWithBigVelocities, along with ItemFrictionWithSmallVelocities, test after,
    //achieves branch coverage
    //The method being tested here is:
        /*public float friction(float v, float f){
            if(Math.Abs(v) >= 0.5f){
                if(v < 0) return v + f;
                return v - f;   
            }
            return 0;
        }*/
    [Test]
    public void ItemFrictionWithBigVelocities(){
        //if the velocity is big, then when friction gets done with the item it shouldn't be zero,
        //if velocity is positive, then it should go down and be less than the orginal velocity
        ItemObject item = new ItemObject(new Vector3(1, 1, 2), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        float temp = item.friction(20, 0.05f);//big and positive branch
        
        Assert.IsTrue(temp == 19.95f);
        

        //technically decrease the negative velocity by making it go towards zero,
        //but the velocity should then decrease by going up, which would be bigger than the
        //orginal velocity
        float temp2 = item.friction(-20, 0.05f);//big and negative branch
        
        Assert.IsTrue(temp2 == -19.95f);

        
    }

    [Test]
    public void ItemFrictionWithSmallVelocities(){
        //if it's super small, then it should be equal to zero, no matter if it's
        //positive or negative
        ItemObject item = new ItemObject(new Vector3(1, 1, 2), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        float temp3 = item.friction(0, 0.05f);//small branch
        Assert.IsTrue(temp3 == 0);
        float temp4 = item.friction(0.45f, 0.05f);
        Assert.IsTrue(temp4 == 0);
        float temp5 = item.friction(-0.45f, 0.05f);
        Assert.IsTrue(temp5 == 0);
    }

    //This test, moveHelperInBoundsTest, along with moveHelperOutBoundsTest, the test after, 
    //achieves branch coverage
    //The method being tested here is:
        /*public void moveHelper(Vector3 newV, float i, float j){
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
        */
    [Test]
    public void moveHelperInBoundsTest(){
        //This sees what happens when the player moves and is in the bounds, and with certain velocities,
        //such as negative/positive velocities and z/x velocitity components 
        PlayerObject pObj = new PlayerObject(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f);
        Vector3 posXTest = new Vector3(2, 0, 0);
        Vector3 negXTest = new Vector3(-1, 0, 0);
        Vector3 posZTest = new Vector3(0, 0, 3);
        Vector3 negZTest = new Vector3(0, 0, -4);

        pObj.moveHelper(posXTest, 12, 25); //the positive branch, then in bounds branch, then x change branch
        Assert.IsTrue(pObj.Velocity.x == 2);//should change the x velocity to 2

        pObj.moveHelper(posZTest, 13, 50); //the positive branch, then in bounds branch, then z change branch
        Assert.IsTrue(pObj.Velocity.z == 3 && pObj.Velocity.x == 2);//should keep the x velocity to 2 and z to 3

        pObj.moveHelper(negXTest, 25, 24);//the negative branch, then in bounds branch, then x change branch
        Assert.IsTrue(pObj.Velocity.x == -1);//should change the x velocity to -1

        pObj.moveHelper(negZTest, 59, 22); //the negative branch, then in bounds branch, then z change branch
        Assert.IsTrue(pObj.Velocity.z == -4 && pObj.Velocity.x == -1);

        
    } 

    [Test]
    public void moveHelperOutBoundsTest(){
        //Tries to hit the branch when the player is out of the set bounds of the mesh/map
        //This does this with certain velocities, negatives and z or x velcoities
        PlayerObject pObj = new PlayerObject(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f);
        Vector3 posXTest = new Vector3(2, 0, 0);
        Vector3 negXTest = new Vector3(-1, 0, 0);
        Vector3 posZTest = new Vector3(0, 0, 3);
        Vector3 negZTest = new Vector3(0, 0, -3);

        //Moving backwards, and accidently get out of bounds, should move back to bounds
        pObj.moveHelper(negXTest, 12, 25);//the negative branch, then out bounds branch, then x change branch
        Assert.IsTrue(pObj.Position.x == 25 && pObj.Velocity.x == 0);//should just change the x position to bound, velcoity stays same

        pObj.moveHelper(negZTest, 13, 50);//the negative branch, then out bounds branch, then z change branch
        Assert.IsTrue(pObj.Position.z == 50 && pObj.Velocity.z == 0);//should just change the z position to bound, velcoity stays same

        //Moving forward and get out of bounds, should move back into the bounds
        pObj.moveHelper(posXTest, 57, 40);//the positive branch, then out bounds branch, then x change branch
        Assert.IsTrue(pObj.Position.x == 40 && pObj.Velocity.x == 0);//same thing as previous two asserts

        pObj.moveHelper(posZTest, 100, 65);//the positive branch, then out bounds branch, then z change branch
        Assert.IsTrue(pObj.Position.z == 65 && pObj.Velocity.z == 0);//should just change the z position to bound, velcoity stays same

    }
    
    
    
    

    //The units being tested here are the collide, item friction, and move,
    // and the approach used was Bottom-Up
    [Test]
    public void ItemCollisionAndMovementTest(){
        //When a player gets close to an item, the item collides with that item, changing it's velocity,
        //this tests to see if that's what is happening here, with a few conditions
        ItemObject sphere = new ItemObject(new Vector3(1, 1, 2), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, false);
        Dynamics dynamics = new Dynamics();
        ItemObject[] allObjects = {sphere};
        PlayerObject pObj = new PlayerObject(new Vector3(0, 1, 2), new Vector3(2, 0, 3), new Vector3(0, 0, 0), 1.0f);
        
        pObj.collide(allObjects);//call the collide unit, player should collide with item

        //Then we add the some friction to this new speeding item using the item friction unit
        float newZSpeed = sphere.friction(sphere.Velocity.z, 0.05f);
        float newXSpeed = sphere.friction(sphere.Velocity.x, 0.05f);

        //the move unit is called to make the item move based on new velocity after collision
        dynamics.move(0.05f, sphere, true);//then after collision and friction applied, it moves
        //changing it's position, to something around there
        Assert.IsTrue(sphere.Position == new Vector3(1.4975f,1,2.7475f));
    }


    //The units being tested here are setFriction, getFrictionForce, player object friction, and
    //move function and the approach used was Bottom-Up
    [Test]
    public void PlayerMovingTests(){
        //dynamics will change the player objects information when something occurs, such as any move
        PlayerObject pObj =  new PlayerObject(new Vector3(1, 100, 3), new Vector3(2, 0, 2), new Vector3(0, 0, 0), 2.0f);
        Dynamics dynamics = new Dynamics();
        
        //set and get friction force units
        dynamics.setFriction(pObj.mass, 0.025f);//should set the dynamics world friction based on the player object
        float test1 = dynamics.getFrictionForce();

        //Cause some friction, using player friction unit on the player, so they will have to slow 
        //down make the friction force a little smaller, since it's a little too big
        float newXSpeed = pObj.friction(pObj.Velocity.x, test1);
        float newZSpeed = pObj.friction(pObj.Velocity.z, test1);

        //Then after the friction, move the player, since there is still some velocity
        //Use the move unit to see what happens when these units are worked toegther
        dynamics.move(2.0f, pObj, true);//is on the ground, so no falling/gravity force at play

        //Should be somewhere around here after the move, math might be a little off
        Assert.IsTrue(pObj.Position == new Vector3(4.02f, 100, 6.02f));
        
    }

}

 /*Player Jump Test, not black box
     [Test]
    public void JumpVelocityPositiveTest()//test to see that velocity is positive when player jumps
    {
        // Use the Assert class to test conditions
        PlayerObject pObj = new PlayerObject(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0.4f);
        pObj.jump();
        Assert.IsTrue(pObj.Velocity.y > 0);
    }*/

/*
    //This is a maybe
    // A Test for constructing items
    [Test]
    public void ConstructItemTest(){
        ItemObject iObj = new ItemObject(new Vector3(10, 20, 3), new Vector3(0, 0, 0), new Vector3(0, 0, 0), 2.0f, false);
        Assert.IsTrue(iObj != null && iObj.Position != null);
        Assert.IsTrue(iObj.Velocity != null && iObj.Force != null);

        //Assert.IsTrue(iObj.getStaticStat());
        //just wanted to put this in to see what happens when it fails, because it's based
        //on whether isStatic is true or not, which is false at this case
    }*/
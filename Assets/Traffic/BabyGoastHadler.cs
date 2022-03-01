using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BabyGoastHadler
{
    static int goasts = 2;
    public static void Die()
    {
        goasts--;
    }
    public static void HandleGoastCollision(goastPickup RequestObject)
    {
        if (goasts < 5)
        {
            goasts += RequestObject.Clone();
        }
    }
}

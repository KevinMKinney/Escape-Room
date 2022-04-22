using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Animation SlideDoor;
    public Animation whampow;
    bool done;
    void TargitHit(int damage)
    {
        if (!done)
        {
            done = true;
            SlideDoor.Play();
            whampow.Play();
        }
    }
}

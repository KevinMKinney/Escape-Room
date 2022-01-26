using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public bool PlayAnnim;
    public AnimationClip annim;
    public bool on;
    public GameObject turnon;
    public bool inon;
    public GameObject inturnon;
    public void Correct() 
    {
        if (on) 
        {
            turnon.SetActive(true);
        }
        if (PlayAnnim)
        {
            GetComponent<Animation>().Play();
        }
    }
    public void InCorrect()
    {
        if (inon)
        {
            inturnon.SetActive(true);
        }
    }
}


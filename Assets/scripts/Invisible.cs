using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour
{
    public bool isInvisible;


    // Brad added the following constructor to avoid the "null object reference" error if
    // isInvisible was not set within the inspector window
    public void Start()
    {
        isInvisible = true;
    }
}

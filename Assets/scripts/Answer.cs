using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour
{
    public bool isright = false;
    bool done = true;
    public int close = 9;
    public void CheckAnswer()
    {
        if (done)
        {
            done = false;
            if (isright)
            {
                transform.parent.GetComponent<Question>().Correct();
            }
            else
            {
                transform.parent.GetComponent<Question>().InCorrect();
            }
        }
    }

    
}

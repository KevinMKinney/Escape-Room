using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool open = true;
    bool canopen = true;
    // Update is called once per frame
    void Update()
    {
        if ((!open) && (canopen))
        {
            canopen = false;
            open = true;
            transform.parent.GetComponent<Animation>().Play("door2");
            StartCoroutine("wait");
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3.45f);
        open = true;
        canopen = true;
    }
}

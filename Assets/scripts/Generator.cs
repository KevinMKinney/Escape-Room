using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [HideInInspector]
    public bool hasStarted = false;
    [HideInInspector]
    public bool hasGas = false;
    public Vector3 shakeRange = new Vector3(0.1f,0.1f,0.1f); 
    public int speed = 2;
   IEnumerator StartShaking()
    {
        GameObject main = transform.GetChild(1).gameObject;
        Vector3 vector3 = main.transform.position;
        Vector3 lastPosition = Vector3.zero;
        int i = speed;
        Vector3 newPosition = Vector3.zero;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            i++;
            if (i >= speed)
            {
                lastPosition = newPosition;
                newPosition = Random.insideUnitSphere;
                newPosition.x *= shakeRange.x;
                newPosition.y *= shakeRange.y;
                newPosition.z *= shakeRange.z;
                main.transform.position = newPosition + vector3;
                i = 0;
            }
            else
            {
                main.transform.position = vector3 + Vector3.Lerp(lastPosition, newPosition, (float)i/speed);
            }
        }
    }
    public void StartGenerator()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine("StartShaking");
        }
    }
}

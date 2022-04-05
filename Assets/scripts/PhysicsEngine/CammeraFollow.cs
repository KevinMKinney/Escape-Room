using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CammeraFollow : MonoBehaviour
{
    public GameObject player;//mine

    void Start()
    {
        player = GameObject.Find("Player");//mine
        transform.position = player.transform.position;//mine
    }

    void FixedUpdate()//mine
    {
        transform.position = GameObject.Find("Player").transform.position;//mine
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);//next two from a website
        transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
    }
}

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
        Cursor.visible=false;

    }

    void FixedUpdate()//mine
    {
        transform.position = GameObject.Find("Player").transform.position;//mine
        transform.Rotate(0, Input.GetAxis("Mouse X")*0.2f, 0);
        transform.Rotate(-Input.GetAxis("Mouse Y")*0.2f, 0, 0);
        player.transform.rotation = transform.rotation;
    }
}

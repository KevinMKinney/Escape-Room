using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CammeraFollow : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.position = player.transform.position;
        
    }

    void FixedUpdate()
    {
        transform.position = GameObject.Find("Player").transform.position;
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);
    }
}

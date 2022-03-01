using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationControllor : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 target;
    GameObject player;
    public bool reacheddestionation;
    public float stopDistance;
    public float rotationSpeed;
    public float movementSpeed;
    goastPickup goast;
    bool randomSpeed;
    public void SetDestionation(Vector3 destionation)
    {
        Vector3 vect = destionation;
        vect.y -= (1 - transform.localScale.y);
        target = vect;
        reacheddestionation = false;
    }
    private void Start()
    {
        goast = GetComponent<goastPickup>();
        target = Vector3.zero;
        if (!randomSpeed)
        movementSpeed = Random.Range(0.9f, 2.9f);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void inharitSpeed(float speed)
    {
        randomSpeed = true;
        movementSpeed = speed + Random.Range(-0.05f, 0.05f);
    }
    // Update is called once per frame
    void Update()
    {
        if (goast.anger < 10)
        {
            if (target != null)
            {
                Vector3 destionationdirection = target - transform.position;
                //destionationdirection.y = 0;
                float destionationdistance = destionationdirection.magnitude;
                if (destionationdistance >= stopDistance)
                {
                    reacheddestionation = false;
                    Quaternion targetrotation = Quaternion.LookRotation(destionationdirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                }
                else
                {
                    reacheddestionation = true;
                }
                // Vector3 rot = transform.eulerAngles;
                //rot.x = 0;
                // transform.eulerAngles = rot;
            }
        }
        else
        {

            CharacterController charecter = player.GetComponent<CharacterController>();
            Vector3 destm = player.transform.position;
            destm.y += 1;
            Vector3 destionationdirection = destm - transform.position;
            
            //destionationdirection.y = 0;
            float destionationdistance = destionationdirection.magnitude;

            if (destionationdistance >= 0.5)
            {
                Quaternion targetrotation = Quaternion.LookRotation(destionationdirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                // CharacterController charecter = player.GetComponent<CharacterController>();
                
                foreach (pick_up item in FindObjectsOfType<pick_up>())
                {
                    if (item.pickedup == 2)
                    {
                        item.pickedup = 5;
                        goast.Pickup(item);
                        goast.anger = 9;
                    }
                }
            }
        }
    }
}

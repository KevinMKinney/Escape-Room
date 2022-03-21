using UnityEngine;


/* 
 * Lines 23 - 28 written this semester.
 */
public class pick_up : MonoBehaviour
{
    public int pickedup = 5;
    public Transform playerCam;
    public Rigidbody rb;
    public int rotateY = 0;
    public int rotateX = 0;
    public Vector3 trans = new(1, -1, 2);
    public raycast raycast;
    public bool firstPickUp;
    [HideInInspector]
    public bool constrainHeldItem = true;
    void Start()
    {
        raycast = FindObjectOfType<raycast>();
    }

    void Update()
    {
        //If the object is in the void than teleport, it to a possible spawn location.
        if (transform.position.y < -20)
        {
            // Get a random spawn location and set the location of the current object to its location.
            transform.position = HelpfulInfo.SpawnPlaces.RandomElement().transform.position;
        }

        if (pickedup == 2 && constrainHeldItem)
        {
            transform.position = playerCam.position;
            transform.Translate(trans, playerCam);
            transform.rotation = playerCam.rotation;
            transform.Rotate(rotateX, 0, rotateY);
        }


        if (Input.GetMouseButton(0))
        {
            if (!raycast.canthrow)
            {
                if ((pickedup == 0) || (pickedup == 1))
                {
                    pickedup = 1;
                    transform.position = playerCam.position;
                    transform.Translate(trans, playerCam);
                    transform.rotation = playerCam.rotation;
                    transform.Rotate(rotateX, 0, rotateY);
                }
                if (pickedup == 2)
                {
                    pickedup = 4;
                    rb.isKinematic = false;
                    GetComponent<Collider>().enabled = true;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.AddForce(raycast.transform.forward * 500);

                }
            }
        }
        else
        {
            if (pickedup == 4)
            {
                pickedup = 5;
            }
            if (pickedup == 1)
            {
                pickedup = 2;
                transform.position = playerCam.position;
                transform.Translate(1, -1, 2, playerCam);
                transform.rotation = playerCam.rotation;
                transform.Rotate(0, 0, -90);
                firstPickUp = true;
                rb.isKinematic = true;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}

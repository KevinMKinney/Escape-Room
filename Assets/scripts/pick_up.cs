using UnityEngine;
public class pick_up : MonoBehaviour
{
    public int pickedup = 5; 
    public Transform playerCam;
    public Rigidbody rb;
    public int rotate = -90;
    public Vector3 trans = new Vector3(1,-1,2);
    public raycast raycast;
    public bool firstPickUp;
    void Start()
    {
        raycast = FindObjectOfType<raycast>();
    }

    void Update()
    {
        Debug.Log(pickedup);
        if (pickedup==2)
        {
            transform.position = playerCam.position;
            transform.Translate(trans, playerCam);
            transform.rotation = playerCam.rotation;
            transform.Rotate(0, 0, rotate);
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
                    transform.Rotate(0, 0, rotate);
                }
                if (pickedup == 2)
                {
                    pickedup = 4;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.AddRelativeForce(-500, 0, 1000);

                }
            }
        }
        else
        {
            if(pickedup==4)
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
            }
        }
    }
}

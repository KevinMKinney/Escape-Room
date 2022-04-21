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

    // ************************
    // *** brad added below ***
    private Inventory inventory;
    private UIControl uiControl;
    private Item item;
    private NoteList noteList;
    private bool addedToInventory;
    public bool dropInitiated;
    // *** brad added above ***
    // ************************

    void Start()
    {
        raycast = FindObjectOfType<raycast>();

        // ************************
        // *** brad added below ***
        inventory = GameObject.Find("Items").GetComponent<Inventory>();
        uiControl = GameObject.Find("UIPanel").GetComponent<UIControl>();
        noteList = GameObject.FindGameObjectWithTag("NoteList").GetComponent<NoteList>();
        item = this.GetComponent<Item>();
        addedToInventory = false;
        dropInitiated = false;
        // *** brad added above ***
        // ************************
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

            // ************************
            // *** brad added below ***
            if (!addedToInventory)
            {
                inventory.AddItem(item);
                inventory.EquipItem(inventory.GetItemIndex(item));
                noteList.PrependNoteToList("Picked up " + item.ItemName);
                addedToInventory = true;
            }
        }

        if (dropInitiated)
        {
            dropInitiated = false;
            addedToInventory = false;
            pickedup = 4;
            rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(raycast.transform.forward * 500);
        }
        // *** brad added above ***
        // ************************


        if (Input.GetMouseButton(0) && !uiControl.visible)
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
                    inventory.DropItem(inventory.GetItemIndex(item));

                    // ************************
                    // *** brad added below ***
                    addedToInventory = false;
                    dropInitiated = false;
                    // *** brad added above ***
                    // ************************
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

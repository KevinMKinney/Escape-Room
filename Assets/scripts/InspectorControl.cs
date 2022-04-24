using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorControl : MonoBehaviour
{
    Item inspectedItem;
    Vector3 originalScale;
    Vector3 currentScale;
    int scaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        inspectedItem = this.GetComponent<Item>();
        originalScale = this.transform.localScale;
        currentScale = originalScale;
        scaleFactor = 0;

        // disable this script for now...
        this.GetComponent<InspectorControl>().enabled = false;
    }

    public void ResetScaleFactor()
    {
        scaleFactor = 0;
        currentScale = originalScale;
    }

    public void SetInspectorPosition()
    {
        Transform playerCam = GameObject.Find("FirstPersonCharacter").GetComponent<Transform>();
        Vector3 trans = new Vector3(0,0,2);
        transform.position = playerCam.position;
        transform.Translate(trans, playerCam);
        transform.localScale = originalScale;
        scaleFactor = 0;
    }

    public void RemoveInspectorPosition()
    {
        Transform playerCam = GameObject.Find("FirstPersonCharacter").GetComponent<Transform>();
        Vector3 trans = new Vector3(1, -1, 2);
        transform.position = playerCam.position;
        transform.Translate(trans, playerCam);
        transform.localScale = originalScale;
        scaleFactor = 0;
    }

    public void RotateUp()
    {
        inspectedItem.transform.Rotate(1f, 0f, 0f, Space.Self);
    }

    public void RotateDown()
    {
        inspectedItem.transform.Rotate(-1f, 0f, 0f, Space.Self);
    }

    public void RotateRight()
    {
        inspectedItem.transform.Rotate(0f, 1f, 0f, Space.Self);
    }

    public void RotateLeft()
    {
        inspectedItem.transform.Rotate(0f, -1f, 0f, Space.Self);
    }

    public void ZoomIn()
    {
        if (scaleFactor <= 200)
        {
            scaleFactor++;
            currentScale.x += 0.01f;
            currentScale.y += 0.01f;
            currentScale.z += 0.01f;
        }

        inspectedItem.transform.localScale = currentScale;
    }

    public void ZoomOut()
    {
        if (scaleFactor >= 0)
        {
            scaleFactor--;
            currentScale.x -= 0.01f;
            currentScale.y -= 0.01f;
            currentScale.z -= 0.01f;
        }

        inspectedItem.transform.localScale = currentScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left ctrl"))
        {
            // zoom controls:
            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                ZoomIn();
            }

            if (Input.GetKey("down") || Input.GetKey("s"))
            {
                ZoomOut();
            }
        } else
        {
            // rotation controls:
            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                RotateUp();
            }

            if (Input.GetKey("right") || Input.GetKey("d"))
            {
                RotateRight();
            }

            if (Input.GetKey("down") || Input.GetKey("s"))
            {
                RotateDown();
            }

            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                RotateLeft();
            }
        }
    }
}
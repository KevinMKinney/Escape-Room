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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left ctrl"))
        {
            // zoom controls:
            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                if (scaleFactor <= 100)
                {
                    scaleFactor++;
                    currentScale.x += 0.01f;
                    currentScale.y += 0.01f;
                    currentScale.z += 0.01f;
                }

                inspectedItem.transform.localScale = currentScale;
            }

            if (Input.GetKey("down") || Input.GetKey("s"))
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
        } else
        {
            // rotation controls:
            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                inspectedItem.transform.Rotate(1f, 0f, 0f, Space.Self);
            }

            if (Input.GetKey("right") || Input.GetKey("d"))
            {
                inspectedItem.transform.Rotate(0f, 1f, 0f, Space.Self);
            }

            if (Input.GetKey("down") || Input.GetKey("s"))
            {
                inspectedItem.transform.Rotate(-1f, 0f, 0f, Space.Self);
            }

            if (Input.GetKey("left") || Input.GetKey("a"))
            {
                inspectedItem.transform.Rotate(0f, -1f, 0f, Space.Self);
            }
        }
    }
}

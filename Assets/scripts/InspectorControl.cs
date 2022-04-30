using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorControl : MonoBehaviour
{
    // attributes:
    Item inspectedItem;
    Vector3 originalScale;
    Vector3 currentScale;
    int scaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes
        inspectedItem = this.GetComponent<Item>();
        originalScale = this.transform.localScale;
        currentScale = originalScale;
        scaleFactor = 0;

        // disable this script for now...
        this.GetComponent<InspectorControl>().enabled = false;
    }

    /// <summary>
    /// ResetScaleFactor() does exactly that, it resets the scale factor
    /// to 0 and returns the currentScale back to the original scale
    /// </summary>
    public void ResetScaleFactor()
    {
        scaleFactor = 0;
        currentScale = originalScale;
    }

    /// <summary>
    /// SetInspectorPosition() repositions the inspected item to the center of the
    /// inspector screen
    /// </summary>
    public void SetInspectorPosition()
    {
        // locate the player camera
        Transform playerCam = GameObject.Find("FirstPersonCharacter").GetComponent<Transform>();

        // position the item directly in front of the player camera
        Vector3 trans = new Vector3(0,0,2);
        transform.position = playerCam.position;
        transform.Translate(trans, playerCam);
        transform.localScale = originalScale;
        scaleFactor = 0;
    }

    /// <summary>
    /// RemoveInspectorPosition() places the inspected item in the default 'equipped'
    /// position
    /// </summary>
    public void RemoveInspectorPosition()
    {
        // locate the player camera
        Transform playerCam = GameObject.Find("FirstPersonCharacter").GetComponent<Transform>();

        // place the inspected item in the default 'equipped' position
        Vector3 trans = new Vector3(1, -1, 2);
        transform.position = playerCam.position;
        transform.Translate(trans, playerCam);
        transform.localScale = originalScale;
        scaleFactor = 0;
    }

    #region RotationFunctions
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
    #endregion

    #region ZoomFunctions
    public void ZoomIn()
    {
        // zoom within the scale window (0 - 200)
        if (scaleFactor <= 200)
        {
            scaleFactor++;
            currentScale.x += 0.01f;
            currentScale.y += 0.01f;
            currentScale.z += 0.01f;
        }

        // update the scale of the item in the game world
        inspectedItem.transform.localScale = currentScale;
    }

    public void ZoomOut()
    {
        // allow zooming out until scale factor is 0 (original scale)
        if (scaleFactor >= 0)
        {
            scaleFactor--;
            currentScale.x -= 0.01f;
            currentScale.y -= 0.01f;
            currentScale.z -= 0.01f;
        }

        // update the scale of the item in the game world
        inspectedItem.transform.localScale = currentScale;
    }
    #endregion

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
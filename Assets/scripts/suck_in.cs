using UnityEngine;

public class suck_in : MonoBehaviour
{
  
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    
    void Update()
    {
        
        if (Input.GetKey(KeyCode.F11))
        {

            Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            Application.Quit();
            
        }
        if (Input.GetKey(KeyCode.F12))
        {


            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

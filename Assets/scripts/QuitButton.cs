using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class QuitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // attributes:
    private GameObject quitButton;

    // Start is called before the first frame is updated...
    private void Start()
    {
        // declaration of the attributes:
        quitButton = GameObject.Find("QuitButton");
    }

    /// <summary>
    /// OnPointerEnter() handles the font-color change when the mouse hovers over the QuitButton
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerEnter(PointerEventData data)
    {
        quitButton.GetComponent<TMP_Text>().color = new Color32(214, 48, 49, 255);
    }

    /// <summary>
    /// OnPointerExitf() handles the font-color change when the mouse no longer hovers over
    /// the quit button
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerExit(PointerEventData data)
    {
        this.gameObject.GetComponent<TMP_Text>().color = new Color32(0, 0, 0, 255);
    }


    /// <summary>
    /// OnPointerClick() handles the quitting of the game when the QuitButton is pressed.
    /// NOTE: Application.Quit() does not work in Edit Mode, the application therefore
    ///       must be built in order for the QuitButton to actually end the game.
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerClick(PointerEventData data)
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}

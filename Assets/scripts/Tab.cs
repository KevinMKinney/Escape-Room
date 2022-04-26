using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // attributes:
    public GameObject tab;
    public GameObject noteBookGameObject;
    public NoteBook noteBook;
    public int tabIndex;
    public bool tabHovered;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        tab = this.gameObject;
        tabHovered = false;
        noteBookGameObject = GameObject.Find("NoteBook");
        noteBook = noteBookGameObject.GetComponent<NoteBook>();
    }

    // OnPointerClick() handles clicking on a tab...
    public void OnPointerClick(PointerEventData eventData)
    {
        // setting the notebook and the tab that has been clicked will
        // make the clicked tab appear in front of the notebook, which will
        // subsequently appear in front of the previously clicked tab...
        noteBookGameObject.transform.SetAsLastSibling();
        tab.transform.SetAsLastSibling();
        noteBook.CurrentTab = tabIndex;
    }

    // OnPointerEnter() handles the mouse hovering over a tab.
    // when a tab is hovered, the tab will move several pixels to the right
    // to give the user a visual queue that the tab has indeed been hovered over...
    public void OnPointerEnter(PointerEventData eventData)
    {
        tabHovered = true;
        MoveRight();
    }

    // OnPointerExit() handles the mouse no longer hovering over a tab.
    // when the tab is no longer being hovered over, it moves to the left several
    // pixels, back to it's original position
    public void OnPointerExit(PointerEventData eventData)
    {
        tabHovered = false;
        MoveLeft();
    }

    // MoveRight() moves the tab this script is attached to the right several pixels
    public void MoveRight()
    {
        tab.transform.position += new Vector3(5, 0, 0);
    }

    // MoveLeft() moves the tab this script is attached to the left several pixels
    public void MoveLeft()
    {
        tab.transform.position -= new Vector3(5, 0, 0);
    }

}

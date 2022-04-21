using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tab;
    public GameObject noteBookGameObject;
    public NoteBook noteBook;
    public int tabIndex;
    public bool tabHovered;

    // Start is called before the first frame update
    void Start()
    {
        tab = this.gameObject;
        tabHovered = false;
        noteBookGameObject = GameObject.Find("NoteBook");
        noteBook = noteBookGameObject.GetComponent<NoteBook>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        noteBookGameObject.transform.SetAsLastSibling();
        tab.transform.SetAsLastSibling();
        noteBook.CurrentTab = tabIndex;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabHovered = true;
        MoveRight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabHovered = false;
        MoveLeft();
    }

    public void MoveRight()
    {
        tab.transform.position += new Vector3(5, 0, 0);
    }

    public void MoveLeft()
    {
        tab.transform.position -= new Vector3(5, 0, 0);
    }

}

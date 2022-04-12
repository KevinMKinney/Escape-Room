using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour, IPointerClickHandler
{
    public GameObject tab;
    public GameObject noteBookGameObject;
    public NoteBook noteBook;
    public int tabIndex;

    // Start is called before the first frame update
    void Start()
    {
        tab = this.gameObject;
        noteBookGameObject = GameObject.Find("NoteBook");
        noteBook = noteBookGameObject.GetComponent<NoteBook>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        noteBookGameObject.transform.SetAsLastSibling();
        tab.transform.SetAsLastSibling();
        noteBook.CurrentTab = tabIndex;
    }
}

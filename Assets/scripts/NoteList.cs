using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteList : MonoBehaviour
{
    // attributes:
    private List<Note> list = new List<Note>();
    private GameObject noteListObject;
    private float width = 217.3906f;
    private float height = 0;

    // Start is called before the first frame update
    void Start()
    {
        noteListObject = GameObject.Find("NoteList");
        noteListObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    private void updateNoteListHeight()
    {
        noteListObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public GameObject CreateListItem(Note note)
    {
        // create the note container:
        GameObject noteContainer = new GameObject("NoteContainer");
        HorizontalLayoutGroup noteContainerLayout = noteContainer.AddComponent<HorizontalLayoutGroup>();
        noteContainerLayout.childForceExpandHeight = false;
        noteContainerLayout.childForceExpandWidth = false;
        noteContainerLayout.childControlHeight = false;
        noteContainerLayout.childControlWidth = true;

        // set the size of the note container (each note 1/5 the height of the list height)
        RectTransform noteRect = noteContainer.GetComponent<RectTransform>();

        // create the time slot:
        GameObject noteTimeObject = new GameObject("NoteTime");
        noteTimeObject.transform.SetParent(noteContainer.transform);
        TextMeshProUGUI noteTime = noteTimeObject.AddComponent<TextMeshProUGUI>();
        noteTime.text = note.NoteTimeAsText();
        noteTime.color = new Color32(0, 0, 0, 255);
        noteTime.font = GameObject.Find("PageTitle").GetComponent<TMP_Text>().font;
        noteTime.fontSize = 12;
        LayoutElement noteTimeLayout = noteTimeObject.AddComponent<LayoutElement>();
        noteTimeLayout.minWidth = 30;

        // create the note slot:
        GameObject noteTextObject = new GameObject("NoteText");
        noteTextObject.transform.SetParent(noteContainer.transform);
        TextMeshProUGUI noteText = noteTextObject.AddComponent<TextMeshProUGUI>();
        noteText.text = note.NoteText;
        noteText.overflowMode = TextOverflowModes.Ellipsis;
        noteText.color = new Color32(0,0,0,255);
        noteText.font = noteTime.font;
        noteText.fontSize = 12;

        if (note.NoteText.Length > 50)
        {
            // make a larger note space
            noteContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 100);
            noteTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 100);
            height += 100 + noteListObject.GetComponent<VerticalLayoutGroup>().spacing;
        } else
        {
            // make a smaller note space
            noteContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 50);
            noteTextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 50);
            height += 60 + noteListObject.GetComponent<VerticalLayoutGroup>().spacing;
        }

        return noteContainer;
    }

    public void PrependNoteToList(string noteText)
    {
        // create a note and add it to the start of the list
        Note newNote = new Note(noteText);
        list.Insert(0, newNote);

        // create the new gameobject and add to the start of the notelist
        GameObject newNoteContainer = CreateListItem(newNote);
        newNoteContainer.transform.SetParent(noteListObject.transform);
        newNoteContainer.transform.SetAsFirstSibling();
        newNoteContainer.transform.rotation = new Quaternion(0, 0, 0, 0);
        newNoteContainer.transform.localScale = new Vector3(1, 1, 1);

        // increase gameobject height to accomodate new note
        updateNoteListHeight();
    }

    public void AppendNoteToList(string noteText)
    {
        // create a note and add it to the end of the list
        Note newNote = new Note(noteText);
        list.Add(newNote);

        Debug.Log(noteListObject.GetComponent<RectTransform>().rect.height);

        // create the new gameobject and add to the end of the notelist
        GameObject newNoteContainer = CreateListItem(newNote);
        newNoteContainer.transform.SetParent(noteListObject.transform);
        newNoteContainer.transform.SetAsLastSibling();
        newNoteContainer.transform.rotation = new Quaternion(0, 0, 0, 0);
        newNoteContainer.transform.localScale = new Vector3(1, 1, 1);

        // increase gameobject height to accomodate new note
        updateNoteListHeight();
    }
}

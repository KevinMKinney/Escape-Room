using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class NoteBook : MonoBehaviour
{

    // attributes:
    private int currentTab;
    public GameObject pageTitle;
    public GameObject itemPageContent;
    public GameObject notesPageContent;
    public GameObject menuPageContent;
    public NoteList noteList;

    // Start is called before the first frame update
    void Start()
    {
        // declaration of attributes:
        currentTab = 0;
        pageTitle = this.gameObject.transform.GetChild(0).gameObject;
        itemPageContent = GameObject.Find("ItemPageContent").gameObject;
        notesPageContent = GameObject.Find("NotesPageContent").gameObject;
        menuPageContent = GameObject.Find("MenuPageContent").gameObject;
        noteList = GameObject.FindGameObjectWithTag("NoteList").GetComponent<NoteList>();

        // initiate the first tab change (set default starting tab to 'items')
        OnTabChange();
    }

    /// <summary>
    /// OnTabChange() displays the correct notebook page based on the currentTab, and disables
    /// all other pages in the notebook.
    /// </summary>
    public void OnTabChange()
    {
        if (currentTab == 0)
        {
            // currentTab is 'items'
            pageTitle.GetComponent<TMP_Text>().text = "Items";
            itemPageContent.SetActive(true);

            // set other notebook pages to disabled
            if (notesPageContent.activeSelf)
            {
                notesPageContent.SetActive(false);
            }

            if (menuPageContent.activeSelf)
            {
                menuPageContent.SetActive(false);
            }

        } else if (currentTab == 1)
        {
            // currentTab is 'notes'
            pageTitle.GetComponent<TMP_Text>().text = "Notes";

            // activate scrollbar and paint the list
            notesPageContent.SetActive(true);
            noteList.PaintNoteList();
            GameObject.Find("Scrollbar").SetActive(true);

            // set all other pages to disabled
            if (itemPageContent.activeSelf)
            {
                itemPageContent.SetActive(false);
            }

            if (menuPageContent.activeSelf)
            {
                menuPageContent.SetActive(false);
            }

        } else if (currentTab == 2)
        {
            // current tab is 'menu'
            pageTitle.GetComponent<TMP_Text>().text = "Menu";
            menuPageContent.SetActive(true);

            // set all other pages to disabled
            if (itemPageContent.activeSelf)
            {
                itemPageContent.SetActive(false);
            }

            if (notesPageContent.activeSelf)
            {
                notesPageContent.SetActive(false);
            }
        }
    }

    // getter/setter for current tab
    public int CurrentTab
    {
        get
        {
            return currentTab;
        }

        set
        {
            // initiate tab change when currentTab value changes
            currentTab = value;
            OnTabChange();
        }
    }
}

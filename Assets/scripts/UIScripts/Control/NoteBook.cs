using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NoteBook : MonoBehaviour
{
    private int currentTab;
    public GameObject pageTitle;
    public GameObject itemPageContent;
    public GameObject notesPageContent;
    public GameObject menuPageContent;
    // Start is called before the first frame update
    void Start()
    {
        currentTab = 0;
        pageTitle = this.gameObject.transform.GetChild(0).gameObject;
        itemPageContent = GameObject.Find("ItemPageContent").gameObject;
        notesPageContent = GameObject.Find("NotesPageContent").gameObject;
        menuPageContent = GameObject.Find("MenuPageContent").gameObject;
        OnTabChange();
    }

    public void OnTabChange()
    {
        if (currentTab == 0)
        {
            pageTitle.GetComponent<TMP_Text>().text = "Items";
            itemPageContent.SetActive(true);

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
            pageTitle.GetComponent<TMP_Text>().text = "Notes";
            notesPageContent.SetActive(true);

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
            pageTitle.GetComponent<TMP_Text>().text = "Menu";
            menuPageContent.SetActive(true);

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

    public int CurrentTab
    {
        get
        {
            return currentTab;
        }

        set
        {
            currentTab = value;
            OnTabChange();
        }
    }
}

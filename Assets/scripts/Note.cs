using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Note
{
    // attributes:
    private string noteText;
    private float noteTime;

    // constructor:
    public Note(string noteText)
    {
        this.noteText = noteText;
        noteTime = Time.timeSinceLevelLoad;
    }

    public string NoteText
    {
        get { return this.noteText; }
    }

    public float NoteTime
    {
        get { return this.noteTime; }
    }

    public string NoteTimeAsText()
    {
        int minutes = (int)(noteTime / 60);
        int seconds = (int)(noteTime % 60);
        return String.Format("{0}:{1}", minutes, seconds);
    }
}

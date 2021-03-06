using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Note
{
    // attributes:
    private string noteText;
    private float noteTime;
    public bool toAppend;

    // constructor:
    public Note(string noteText)
    {
        this.noteText = noteText;
        noteTime = Time.timeSinceLevelLoad;
        toAppend = false;
    }

    #region Getters/Setters
    public string NoteText
    {
        get { return this.noteText; }
    }

    public float NoteTime
    {
        get { return this.noteTime; }
    }

    #endregion Getters/Setters

    /// <summary>
    /// NoteTimeAsText() returns a string formatted version of the 
    /// time the note was taken in minues:seconds format
    /// </summary>
    public string NoteTimeAsText()
    {
        int minutes = (int)(noteTime / 60);
        int seconds = (int)(noteTime % 60);
        return String.Format("{0}:{1}", minutes, seconds);
    }
}

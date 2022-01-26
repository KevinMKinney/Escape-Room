using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class SaveMe : EditorWindow
{

    private bool autoSaveScene = true;
    private bool showMessage = true;
    private bool isStarted = false;
    private float intervalScene = 1;
    private DateTime lastSaveTimeScene = DateTime.Now;

    private string projectPath;
    private string scenePath;

    [MenuItem("Window/AutoSave")]
    static void Init()
    {
        SaveMe saveWindow = (SaveMe)EditorWindow.GetWindow(typeof(SaveMe));
        saveWindow.Show();

    }
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SaveMe));
    }

    void OnGUI()
    {
        projectPath = Application.dataPath;
        GUILayout.Label("Info:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Saving to:", "" + projectPath);
        EditorGUILayout.LabelField("Saving scene:", "" + scenePath);
        GUILayout.Label("Options:", EditorStyles.boldLabel);
        autoSaveScene = EditorGUILayout.BeginToggleGroup("Auto save", autoSaveScene);
        intervalScene = EditorGUILayout.Slider("Interval (minutes)", intervalScene, 0.1f, 10);
        if (isStarted)
        {
            EditorGUILayout.LabelField("Last save:", "" + lastSaveTimeScene);
        }
        EditorGUILayout.EndToggleGroup();
        showMessage = EditorGUILayout.BeginToggleGroup("Show Message", showMessage);
        EditorGUILayout.EndToggleGroup();
    }


    void Update()
    {
        scenePath = EditorSceneManager.GetActiveScene().name;
        if (autoSaveScene)
        {
            if (DateTime.Now.Second >= (lastSaveTimeScene.Second + (intervalScene * 60)) || DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
            {
                saveScene();
            }
        }
        else
        {
            isStarted = false;
        }

    }

    void saveScene()
    {

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        lastSaveTimeScene = DateTime.Now;
        isStarted = true;
        if (showMessage)
        {
            Debug.Log("AutoSave saved: " + scenePath + " on " + lastSaveTimeScene);
        }
        SaveMe repaintSaveWindow = (SaveMe)EditorWindow.GetWindow(typeof(SaveMe));
        repaintSaveWindow.Repaint();
    }
}

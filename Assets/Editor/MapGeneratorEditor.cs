using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// middleman between Unity inspector and MapGenerator.cs
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector()) {
            //if (mapGen.autoUpdate && (mapGen.currentDisplay == baseMesh)) {
            if (mapGen.autoUpdate) {
                mapGen.generateMap();
            }
        }

        if (GUILayout.Button("Generate")) {
            mapGen.generateMap();
        }
    }
}

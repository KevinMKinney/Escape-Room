using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // variables dev can manipulate for noise & mesh map
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public DisplayMode currentDisplay;
    public enum DisplayMode {
        baseMesh,
        coloredMesh
    };

    public bool autoUpdate;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        //Debug.Log("DM: "+currentDisplay);
        // could just update the mesh instead of rebuilding it?
        Mesh mesh = new Mesh();
        mesh.name = "PerlinMesh";

        mesh.Clear();

        mesh.vertices = MeshMap.GenerateVerticies(noiseMap);
        mesh.triangles = MeshMap.GenerateTriangles(mapWidth, mapHeight);
        mesh.RecalculateNormals();
        float[] steepVal = MeshMap.calculateSteepness(mesh);
        mesh.colors = MeshMap.GenerateColors(mesh, steepVal);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
        display.DrawMeshMap(mesh);
    }

    // purely for fixing base cases (invalid inputs)
    void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
        }
        if (mapHeight < 1) {
            mapHeight = 1;
        }

        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // variables dev can manipulate for noise & mesh map
    [Header("Mesh Settings")]
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    [Range(0,1)]
    public float snowThresh;
    [Range(0,1)]
    public float waterThresh;

    [Header("Water Settings")]
    public float noiseScaleWater;

    public int octavesWater;
    [Range(0,1)]
    public float persistanceWater;
    public float lacunarityWater;

    public int seedWater;

    [Space(10)]
    public bool autoUpdate;

    // the "main" function that handles the generation proccess
    public void GenerateMap() {
        // noisemap gets shape of mesh (see Noise.cs for further information)
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        float[,] noiseMapWater = Noise.GenerateNoiseMap(mapWidth, mapHeight, seedWater, noiseScaleWater, octavesWater, persistanceWater, lacunarityWater, offset);
        //float[,] noiseMapWater = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, 3, .35f, 2.4f, offset);

        // mesh init
        Mesh mesh = new Mesh();
        mesh.name = "PerlinMesh";
        mesh.Clear();

        Mesh meshWater = new Mesh();
        meshWater.name = "WaterMesh";
        meshWater.Clear();

        // assign mesh aspects
        mesh.vertices = MeshMap.generateVerticies(noiseMap);
        mesh.triangles = MeshMap.generateTriangles(mapWidth, mapHeight);
        mesh.RecalculateNormals();
        mesh.uv = MeshMap.generateUVs(mesh);
        //mesh.uv = Unwrapping.GeneratePerTriangleUV(mesh);

        float[] steepVal = MeshMap.calculateSteepness(mesh);
        //mesh.colors = MeshMap.GenerateColors(mesh, steepVal, snowThresh, waterThresh);

        meshWater.vertices = MeshMap.generateVerticies(noiseMapWater);
        meshWater.triangles = MeshMap.generateTriangles(mapWidth, mapHeight);
        meshWater.RecalculateNormals();
        meshWater.uv = MeshMap.generateUVs(mesh);

        // puts mesh and noise in game
        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.DrawNoiseMap(noiseMap);
        display.DrawMeshMap(mesh, noiseMap, meshWater, waterThresh);
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

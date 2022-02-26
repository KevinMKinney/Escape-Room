﻿using System.Collections;
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

    public AnimationCurve meshHeightCurve;

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
        float[,] noiseMap = Noise.generateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, meshHeightCurve, offset);
        float[,] noiseMapWater = Noise.generateFlushNoiseMap(mapWidth, mapHeight, seedWater, noiseScaleWater, octavesWater, persistanceWater, lacunarityWater, offset);
        //float[,] noiseMapWater = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, 3, .35f, 2.4f, offset);

        // mesh init
        Mesh mesh = new Mesh();
        mesh.name = "PerlinMesh";
        mesh.Clear();

        Mesh meshWater = new Mesh();
        meshWater.name = "WaterMesh";
        meshWater.Clear();

        // assign mesh aspects
        mesh.SetVertices(MeshMap.generateVerticies(noiseMap));
        mesh.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, MeshMap.generateUVs(mesh, mapWidth, mapHeight));
        //mesh.uv = Unwrapping.GeneratePerTriangleUV(mesh);

        float[] steepVal = MeshMap.calculateSteepness(mesh, mapWidth, mapHeight);
        //mesh.colors = MeshMap.generateColors(mesh, steepVal, snowThresh, waterThresh);
        mesh.SetColors(MeshMap.generateColors(mesh, mapWidth, mapHeight, steepVal, snowThresh, waterThresh));

        meshWater.SetVertices(MeshMap.generateVerticies(noiseMapWater));
        meshWater.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        meshWater.RecalculateNormals();
        meshWater.SetUVs(0, MeshMap.generateUVs(mesh, mapWidth, mapHeight));

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

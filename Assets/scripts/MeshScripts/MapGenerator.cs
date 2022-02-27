using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

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
    public MeshData generateMap() {
        // noisemap gets shape of mesh (see Noise.cs for further information)
        float[,] noiseMap = Noise.generateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, meshHeightCurve, offset);

        // mesh init
        Mesh mesh = new Mesh();
        mesh.name = "PerlinMesh";
        mesh.Clear();

        // assign mesh aspects
        mesh.SetVertices(MeshMap.generateVerticies(noiseMap));
        mesh.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, MeshMap.generateUVs(mesh, mapWidth, mapHeight));
        //mesh.uv = Unwrapping.GeneratePerTriangleUV(mesh);

        float[] steepVal = MeshMap.calculateSteepness(mesh, mapWidth, mapHeight);
        Color[] meshCols = MeshMap.generateColors(mesh, mapWidth, mapHeight, steepVal, snowThresh, waterThresh);
        mesh.SetColors(meshCols);

        return new MeshData(noiseMap, mesh);
    }

    public Mesh generateWater() {
        // noisemap gets shape of mesh (see Noise.cs for further information)
        float[,] noiseMapWater = Noise.generateFlushNoiseMap(mapWidth, mapHeight, seedWater, noiseScaleWater, octavesWater, persistanceWater, lacunarityWater, offset);

        // mesh init
        Mesh meshWater = new Mesh();
        meshWater.name = "WaterMesh";
        meshWater.Clear();

        meshWater.SetVertices(MeshMap.generateVerticies(noiseMapWater));
        meshWater.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        meshWater.RecalculateNormals();
        meshWater.SetUVs(0, MeshMap.generateUVs(meshWater, mapWidth, mapHeight));

        return meshWater;
    }

    public void drawMeshEditor() {
        MeshData meshData = generateMap();
        Mesh meshWater = generateWater();
        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.DrawNoiseMap(meshData.heightMap);
        display.DrawMeshMap(meshData.mesh, meshData.heightMap, meshWater, waterThresh);
    }

    //public void RequestMeshData(Action)

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

public struct MeshData {
    public float[,] heightMap;
    public Mesh mesh;

    public MeshData(float[,] heightMap, Mesh mesh) {
        this.heightMap = heightMap;
        this.mesh = mesh;
    }
}

﻿using System.Collections;
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
    [Range(0,15)]
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
    [Range(0,15)]
    public int octavesWater;
    [Range(0,1)]
    public float persistanceWater;
    public float lacunarityWater;

    public int seedWater;

    [Header("Entity Settings")]
    [Range(0,1)]
    public float entityMaxThresh;
    public float entityFrequency;
    public float entitySpread;

    public float noiseScaleEntity;
    [Range(0,15)]
    public int octavesEntity;
    [Range(0,1)]
    public float persistanceEntity;
    public float lacunarityEntity;

    public int seedEntity;

    [Space(10)]
    public bool autoUpdate;
    public bool coloredMesh;
    public bool entities;

    // queue for threading purposes
    Queue<meshThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<meshThreadInfo<MeshData>>();

    // the "main" function that handles the generation proccess for land mesh
    public MeshData generateMap() {
        // noisemap gets shape of mesh (see Noise.cs for further information)
        float[,] noiseMap = Noise.generateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        noiseMap = Noise.curveNoise(mapWidth, mapHeight, noiseMap, meshHeightCurve);

        // mesh init
        Mesh mesh = new Mesh();
        mesh.name = "PerlinMesh";
        mesh.Clear();

        // assign mesh aspects
        mesh.SetVertices(MeshMap.generateVerticies(noiseMap));
        mesh.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        mesh.RecalculateNormals();
        mesh.SetUVs(0, MeshMap.generateUVs(mesh, mapWidth, mapHeight));

        if (coloredMesh) {
            float[] steepVal = MeshMap.calculateSteepness(mesh, mapWidth, mapHeight);
            Color[] meshCols = MeshMap.generateColors(mesh, mapWidth, mapHeight, steepVal, snowThresh, waterThresh);
            mesh.SetColors(meshCols);
        }

        return new MeshData(noiseMap, mesh);
    }

    public Mesh generateWater() {
        // noisemap gets shape of mesh (see Noise.cs for further information)
        float[,] noiseMapWater = Noise.generateFlushNoiseMap(mapWidth, mapHeight, seedWater, noiseScaleWater, octavesWater, persistanceWater, lacunarityWater, offset);

        // mesh init
        Mesh meshWater = new Mesh();
        meshWater.name = "WaterMesh";
        meshWater.Clear();

        // assign mesh aspects
        meshWater.SetVertices(MeshMap.generateVerticies(noiseMapWater));
        meshWater.SetTriangles(MeshMap.generateTriangles(mapWidth, mapHeight), 0);
        meshWater.RecalculateNormals();
        meshWater.SetUVs(0, MeshMap.generateUVs(meshWater, mapWidth, mapHeight));

        return meshWater;
    }

    public float[,] generateEntities() {
        // noisemap gets shape of mesh (see Noise.cs for further information)
        float[,] meshMap = Noise.generateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        meshMap = Noise.curveNoise(mapWidth, mapHeight, meshMap, meshHeightCurve);
        float[,] densityMap = Noise.generateNoiseMap(mapWidth, mapHeight, seedEntity, noiseScaleEntity, octavesEntity, persistanceEntity, lacunarityEntity, offset);

        return EntityMap.generateEntityMap(meshMap, densityMap, seedEntity, waterThresh, entityMaxThresh, entityFrequency, entitySpread);
    }

    // function calls methods from MapDisplay
    public void drawMeshEditor() {
        MeshData meshData = generateMap();
        Mesh meshWater = generateWater();
        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.drawNoiseMap(meshData.heightMap);
        display.drawMeshMap(meshData.mesh, meshData.heightMap, meshWater, waterThresh);

        display.removeEntitySpheres();
        if (entities) {
            float[,] entities = generateEntities();
            display.drawEntities(meshData.heightMap, waterThresh, entities);
        }
    }

    // for threading (not implemented yet)
    public void requestMeshData(Action<MeshData> callback) {
        ThreadStart threadStart = delegate {
            meshDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    private void meshDataThread(Action<MeshData> callback) {
        MeshData meshData = generateMap();
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new meshThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void update() {
        if (meshDataThreadInfoQueue.Count > 0) {
            for(int i = 0; i > meshDataThreadInfoQueue.Count; i++) {
                meshThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
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

    // for threading (not implemented yet)
    struct meshThreadInfo<T> {
        public readonly Action<T> callback;
        public readonly T parameter;

        public meshThreadInfo(Action<T> callback, T parameter) {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

public struct MeshData {
    public readonly float[,] heightMap;
    public readonly Mesh mesh;

    public MeshData(float[,] heightMap, Mesh mesh) {
        this.heightMap = heightMap;
        this.mesh = mesh;
    }
}

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

    [Header("Entity Settings")]
    [Range(0,1)]
    public float entityThresh;
    public float entityspread;

    public float noiseScaleEntity;

    public int octavesEntity;
    [Range(0,1)]
    public float persistanceEntity;
    public float lacunarityEntity;

    public int seedEntity;

    [Space(10)]
    public bool autoUpdate;

    Queue<meshThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<meshThreadInfo<MeshData>>();

    // the "main" function that handles the generation proccess
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

    public float[,] generateEntities() {
        float[,] meshMap = Noise.generateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        meshMap = Noise.curveNoise(mapWidth, mapHeight, meshMap, meshHeightCurve);
        float[,] densityMap = Noise.generateNoiseMap(mapWidth, mapHeight, seedEntity, noiseScaleEntity, octavesEntity, persistanceEntity, lacunarityEntity, offset);

        return EntityMap.generateEntityMap(meshMap, densityMap, entityThresh, entityspread);
    }

    public void drawMeshEditor() {
        MeshData meshData = generateMap();
        Mesh meshWater = generateWater();
        float[,] entities = generateEntities();
        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.drawNoiseMap(meshData.heightMap);
        display.drawMeshMap(meshData.mesh, meshData.heightMap, meshWater, waterThresh, entities);
    }

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

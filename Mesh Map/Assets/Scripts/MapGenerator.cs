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

    public bool autoUpdate;

    public float meshWidthScale;
    public float meshHeightScale;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Mesh meshMap = MeshMap.GenerateMeshMap(meshWidthScale, meshHeightScale, noiseMap);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
        display.DrawMeshMap(meshMap);
    }

    // purely for fixing base cases (invalid inputs)
    void OnValidate()
    {
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

        if (meshWidthScale < 0) {
            meshWidthScale = .0001f;
        }
        if (meshHeightScale < 1) {
            meshHeightScale = .0001f;
        }
    }
}

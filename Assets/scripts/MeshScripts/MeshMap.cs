using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

[RequireComponent(typeof(MeshFilter), (typeof(MeshRenderer)))]
public static class MeshMap
{
    // creates vertices for mesh based on pre-made noise
    public static Vector3[] GenerateVerticies(float[,] noiseMap) {
        // initialize variables
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        Vector3[] vertices = new Vector3[mapWidth*mapHeight];

        // Turn noiseMap pixels into vertices (1 pixel = 1 vertex)
        for(int y = 0; y < mapHeight; y++) {
            for(int x = 0; x < mapWidth; x++) {
                Vector3 v = new Vector3(x, (noiseMap[x, y]), y);
                vertices.SetValue(v, (y*mapWidth+x));
            }
        }

        return vertices;
    }

    // creates triangles for mesh
    public static int[] GenerateTriangles(int mapWidth, int mapHeight) {
        // initialize variables
        int index = 0;
        int j = 0;
        int triangleNum = 2 * (mapWidth-1) * (mapHeight - 1);
        int[] triangles = new int[3*triangleNum];

        // Make triangles (makes 2 triangles at a time that are defined clockwise)
        for(int i = 0; i < ((mapWidth) * (mapHeight - 1)); i++) {
            if ((i % mapWidth) != (mapWidth-1)) {
                index = 6*j;
                triangles.SetValue(i, index);
                triangles.SetValue(i+mapWidth, index+1);
                triangles.SetValue(i+1, index+2);
                triangles.SetValue(i+1, index+3);
                triangles.SetValue(i+mapWidth, index+4);
                triangles.SetValue(i+mapWidth+1, index+5);
                j++;
            }
        }
        return triangles;
    }

    // calculates the steepness of triangles based on their normal vectors (the functional way)
    public static float[] calculateSteepness(Mesh mesh) {
        Vector3[] norm = new Vector3[mesh.vertices.Length];
        norm = mesh.normals;
        return norm.Select(x => Vector3.Angle(x, Vector3.up)).ToArray();
    }

    // returns the max value in float array (used for steepness)
    public static float GetMaxSteepness(float[] steepVal) {
        return steepVal.Aggregate((x, y) => Mathf.Max(x, y));
    }

    // creates the colors of the mesh based on its steepness
    public static Color[] GenerateColors(Mesh mesh, float[] steepVal, float snowThresh) {
        // initialize variables
        int size = mesh.vertices.Length;
        Color[] colors = new Color[size];
        // get some random colors
        Color cool1 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color cool2 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        float maxSteep = GetMaxSteepness(steepVal);

        // assign colors
        for (int i = 0; i < size; i++) {
            if (mesh.vertices[i].y >= snowThresh) {
                colors[i] = Color.Lerp(Color.white, cool2, (steepVal[i])/maxSteep);
            } else {
                colors[i] = Color.Lerp(cool1, cool2, (steepVal[i])/maxSteep);
            }
        }
        return colors;
    }
}

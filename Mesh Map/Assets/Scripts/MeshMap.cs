using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public static class MeshMap
{
    public static Mesh GenerateMeshMap(float widthScale, float heightScale, float[,] noiseMap)
    {
        // could also add a normal array (for textures) or a UV array (also for textures but for different reasons)

        // initialize variables
        Mesh mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();

        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);
        int trianlgeNum = 2 * (mapWidth - 1) * (mapHeight - 1);

        Vector3[] vertices = new Vector3[mapWidth*mapHeight];
        int[] triangles = new int[3*trianlgeNum];

        // Base Case
        if (widthScale <= 0)
        {
            widthScale = 0.0001f;
        }
        if (heightScale <= 0)
        {
            heightScale = 0.0001f;
        }

        // Turn noiseMap pixels into vertices (1 pixel = 1 vertex)
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                vertices.SetValue((x*widthScale, noiseMap[x, y], y*widthScale), (y*mapHeight+x));
            }
        }

        // Make triangles from vertices (makes 2 triangles at a time that are defined clockwise)
        for(int i = 0; i < (trianlgeNum/2); i++)
        {
            triangles.SetValue(i, i);
            triangles.SetValue(i+mapWidth, i+1);
            triangles.SetValue(i+1, i+2);
            triangles.SetValue(i+1, i+3);
            triangles.SetValue(i+mapWidth, i+4);
            triangles.SetValue(i+mapWidth+1, i+5);
        }

        // Apply arrays into mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), (typeof(MeshRenderer)))]
public static class MeshMap
{
    public static Vector3[] GenerateVerticies(float xScale, float yScale, float zScale, float[,] noiseMap)
    {
        // initialize variables
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        Vector3[] vertices = new Vector3[mapWidth*mapHeight];

        // Base Case
        if (xScale <= 0)
        {
            xScale = 0.0001f;
        }
        if (yScale <= 0)
        {
            yScale = 0.0001f;
        }
        if (zScale <= 0)
        {
            zScale = 0.0001f;
        }

        // Turn noiseMap pixels into vertices (1 pixel = 1 vertex)
        for(int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                Vector3 v = new Vector3((x*xScale), ((noiseMap[x, y])*yScale), (y*zScale));
                vertices.SetValue(v, (y*mapWidth+x));
            }
        }

        return vertices;
    }

    public static int[] GenerateTriangles(int mapWidth, int mapHeight) {

        int index;
        int triangleNum = 2 * (mapWidth - 1) * (mapHeight - 1);
        int[] triangles = new int[3*triangleNum];

        // Make triangles from vertices (makes 2 triangles at a time that are defined clockwise)
        for(int i = 0; i < (triangleNum/2); i++)
        {
            if ((i % mapWidth) != (mapWidth-1)) {
                index = 6*i;
                triangles.SetValue(i, index);
                triangles.SetValue(i+mapWidth, index+1);
                triangles.SetValue(i+1, index+2);
                triangles.SetValue(i+1, index+3);
                triangles.SetValue(i+mapWidth, index+4);
                triangles.SetValue(i+mapWidth+1, index+5);
            }
        }

        return triangles;
    }
}

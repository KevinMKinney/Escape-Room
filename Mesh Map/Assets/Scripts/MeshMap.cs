using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;

[RequireComponent(typeof(MeshFilter), (typeof(MeshRenderer)))]
public static class MeshMap
{
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

    public static int[] GenerateTriangles(int mapWidth, int mapHeight) {

        int index = 0;
        int j = 0;
        // the actual amount of triangles is 2 * (mapWidth-1) * (mapHeight - 1)
        int triangleNum = 2 * (mapWidth-1) * (mapHeight - 1);
        int[] triangles = new int[3*triangleNum];

        // Make triangles from vertices (makes 2 triangles at a time that are defined clockwise)
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
                //index += 6;
            }
        }
        return triangles;
    }


    public static float[] calculateSteepness(Mesh mesh) {
        Vector3[] norm = new Vector3[mesh.vertices.Length];
        norm = mesh.normals;
        return norm.Select(x => Vector3.Angle(x, Vector3.up)).ToArray();
    }

    /*
    public static float GetSteepness(Mesh mesh, int index) {

        Vector3 normal = mesh.normals[index];
        float angle = Vector3.Angle(normal, Vector3.up);
        return angle;

    }*/

    public static Color[] GenerateColors(Mesh mesh, float[] steepVal) {

        int size = mesh.vertices.Length;
        Color[] colors = new Color[size];

        Color cool1 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color cool2 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        float maxSteep = 4f;

        /*
        float maxSteep = 0f;
        float temp;
        for (int i = 0; i < size; i++) {
          temp = GetSteepness(mesh, i);
            if (temp > maxSteep) {
              maxSteep = temp;
            }
        } */

        for (int i = 0; i < size; i++) {
            //Debug.Log("steep is "+GetSteepness(mesh, i)+" at "+i);
            colors[i] = Color.Lerp(cool1, cool2, (steepVal[i])/maxSteep);
        }

        return colors;
    }
}

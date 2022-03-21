using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using System;

//[RequireComponent(typeof(MeshFilter), (typeof(MeshRenderer)))]
public static class MeshMap
{
    // creates vertices for mesh based on pre-made noise
    public static Vector3[] generateVerticies(float[,] noiseMap) {
        // initialize variables
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        if (mapWidth <= 1 || mapHeight <= 1) {
            throw new Exception();
            //return null;
        }

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
    public static int[] generateTriangles(int mapWidth, int mapHeight) {

        if (mapWidth <= 1 || mapHeight <= 1) {
            throw new Exception();
            //return null;
        }

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

    public static Vector2[] generateUVs(Mesh mesh, int mapWidth, int mapHeight) {
        int size = mapWidth*mapHeight;
        Vector2[] UVs = new Vector2[size];

        for (int i = 0; i < size; i++) {
            UVs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);
        }
        return UVs;
    }

    // calculates the steepness of triangles based on their normal vectors (the functional way)
    public static float[] calculateSteepness(Mesh mesh, int mapWidth, int mapHeight) {
        Vector3[] norm = new Vector3[mapWidth*mapHeight];
        norm = mesh.normals;
        return norm.Select(x => Vector3.Angle(x, Vector3.up)).ToArray();
    }

    // returns the max value in float array
    public static float getMaxVertex(Vector3[] vertices) {
        float[] array = vertices.Select(x => x.y).ToArray();
        return array.Aggregate((x, y) => Mathf.Max(x, y));
    }

    // returns the min value in float array
    public static float getMinVertex(Vector3[] vertices) {
        float[] array = vertices.Select(x => x.y).ToArray();
        return array.Aggregate((x, y) => Mathf.Min(x, y));
    }

    // creates the colors of the mesh based on its steepness
    public static Color[] generateColors(Mesh mesh, int mapWidth, int mapHeight, float[] steepVal, float snowThresh, float waterThresh) {
        // initialize variables
        int size = mapWidth*mapHeight;
        Color[] colors = new Color[size];
        // color init
        //Color col1 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //Color col2 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Color col1 = Color.green;
        Color col2 = new Color32(160, 82, 45, 1);

        //float maxSteep = steepVal.Aggregate((x, y) => Mathf.Max(x, y));
        float maxSteep = Mathf.Max(steepVal);

        // assign colors
        for (int i = 0; i < size; i++) {
            if (mesh.vertices[i].y <= waterThresh) {
                colors[i] = col2;
            } else {
                if (mesh.vertices[i].y >= snowThresh) {
                    colors[i] = Color.Lerp(Color.white, col2, (steepVal[i])/maxSteep);
                } else {
                    colors[i] = Color.Lerp(col1, col2, (steepVal[i])/maxSteep);
                }
            }

        }
        return colors;
    }

    // WIP - function for testing computational time
    public static void getTime(int a) {
        Debug.Log("Time at "+a+" is: "+Time.time);
    }
}

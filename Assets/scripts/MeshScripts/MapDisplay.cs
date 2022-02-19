﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    // properties for unity objects
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilterWater;
    public MeshRenderer meshRendererWater;
    public ComputeShader meshComp;

    // creates a texture based on pre-made noise
    public void DrawNoiseMap(float[,] noiseMap) {
        // initialize variables
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];
        for(int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawMeshMapOld(Mesh meshMap, float[,] noiseMap, Mesh waterMesh, float waterThresh) {

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        GameObject meshObj = GameObject.Find("MeshObj");
        GameObject waterObj = GameObject.Find("WaterObj");
        meshObj.transform.position = transform.position;
        waterObj.transform.position = transform.position;
        float waterPos = ((MeshMap.GetMaxVertex(meshMap.vertices) - MeshMap.GetMinVertex(meshMap.vertices)) * waterThresh * meshObj.transform.localScale.y) + transform.position.y;
        //Debug.Log("WP: "+waterPos);
        waterObj.transform.position = new Vector3(waterObj.transform.position.x, waterPos, waterObj.transform.position.z);

        meshFilter.mesh = meshMap;
        //meshObj.Renderer.material.setTexture(heightMap);
        meshFilterWater.mesh = waterMesh;

    }

    public void DrawMeshMap(Mesh meshMap, float[,] noiseMap) {

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        GameObject meshObj = GameObject.Find("MeshObj");
        GameObject waterObj = GameObject.Find("WaterObj");
        meshObj.transform.position = transform.position;
        //waterObj.transform.position = transform.position;
        //float waterPos = ((MeshMap.GetMaxVertex(meshMap.vertices) - MeshMap.GetMinVertex(meshMap.vertices)) * waterThresh * meshObj.transform.localScale.y) + transform.position.y;
        //Debug.Log("WP: "+waterPos);
        //waterObj.transform.position = new Vector3(waterObj.transform.position.x, waterPos, waterObj.transform.position.z);


        int vectorSize = sizeof(float)*3;
        int colorSize = sizeof(float)*4;
        int totalSize = vectorSize*colorSize;

        ComputeBuffer meshBuffer = new ComputeBuffer(width*height, totalSize);
        meshBuffer.SetData(meshMap.vertices);

        meshComp.SetBuffer(0, "verticies", meshBuffer);
        meshComp.SetFloat("Resolution", width*height);
        meshComp.Dispatch(0, width/8, height/8, 1);

        meshBuffer.GetData(meshMap.vertices);

        meshBuffer.Dispose();

        meshFilter.mesh = meshMap;
        //meshObj.Renderer.material.setTexture(heightMap);
        //meshFilterWater.mesh = waterMesh;

    }
}

using System.Collections;
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
    public void drawNoiseMap(float[,] noiseMap) {
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

    public void drawMeshMap(Mesh meshMap, float[,] noiseMap, Mesh waterMesh, float waterThresh, float[,] entities) {

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        GameObject meshObj = GameObject.Find("MeshObj");
        GameObject waterObj = GameObject.Find("WaterObj");
        meshObj.transform.position = transform.position;
        waterObj.transform.position = transform.position;
        float waterPos = ((MeshMap.getMaxVertex(meshMap.vertices) - MeshMap.getMinVertex(meshMap.vertices)) * waterThresh * meshObj.transform.localScale.y) + transform.position.y;
        waterPos -= 50; // <--temp?
        waterObj.transform.position = new Vector3(waterObj.transform.position.x, waterPos, waterObj.transform.position.z);

        meshRenderer.sharedMaterial.SetFloat("_Size", width);

        /*
        MaterialsPropertyBlock properties = new MaterialsPropertyBlock();
        for (int i = 0; i < width*height; i++) {
            properties.setColor("_Color", new Color(Random.value, Random.value, Random.value));
            meshRenderer.SetPropertyBlock(properties);
        } */

        meshRendererWater.sharedMaterial.SetFloat("_Size", width);

        removeEntitySpheres();
        drawEntitySphere(noiseMap, entities);

        meshFilter.mesh = meshMap;
        //meshObj.Renderer.material.setTexture(heightMap);
        meshFilterWater.mesh = waterMesh;

    }

    private void drawEntitySphere(float[,] noiseMap, float[,] entities) {
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);

        GameObject meshObj = GameObject.Find("MeshObj");

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (entities[x, y] == 1) {
                    float _x = x * meshObj.transform.localScale.x + meshObj.transform.position.x;
                    float _y = noiseMap[x, y] * meshObj.transform.localScale.y + meshObj.transform.position.y;
                    float _z = y * meshObj.transform.localScale.z + meshObj.transform.position.z;

                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = new Vector3(_x, _y, _z);
                }
            }
        }
    }

    private void removeEntitySpheres() {
        object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object obj in objects) {
            GameObject o = (GameObject) obj;
            if (o.name == "sphere") {
                Destroy(o);
            }
        }
    }
}

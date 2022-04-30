using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MapDisplay : MonoBehaviour
{
    // properties for unity objects
    public Renderer textureRender;
    // for land mesh
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    // for water mesh
    public MeshFilter meshFilterWater;
    public MeshRenderer meshRendererWater;
    // for entities
    public GameObject treeObj;

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

        // apply texture to plane
        texture.SetPixels(colorMap);
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }

    // puts meshs onto the scene
    public void drawMeshMap(Mesh meshMap, float[,] noiseMap, Mesh waterMesh, float waterThresh) {
        // initialize variables
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        GameObject meshObj = GameObject.Find("MeshObj");
        GameObject waterObj = GameObject.Find("WaterObj");
        meshObj.transform.position = transform.position;
        waterObj.transform.position = transform.position;
        float waterPos = ((MeshMap.getMaxVertex(meshMap.vertices) - MeshMap.getMinVertex(meshMap.vertices)) * waterThresh * meshObj.transform.localScale.y) + transform.position.y;
        waterPos -= 30; // <-- water is just slighty too high
        waterObj.transform.position = new Vector3(waterObj.transform.position.x, waterPos, waterObj.transform.position.z);

        meshRenderer.sharedMaterial.SetFloat("_Size", width);
        meshRendererWater.sharedMaterial.SetFloat("_Size", width);

        meshFilter.mesh = meshMap;
        //meshObj.Renderer.material.setTexture(heightMap);
        meshFilterWater.mesh = waterMesh;

    }

    // function that handles what entities should be drawn (would probably add more later)
    public void drawEntities(float[,] noiseMap, float waterThresh, float[,] entities, int seed) {
        //drawEntitySphere(noiseMap, entities, waterThresh); // for testing
        drawEntityTree(noiseMap, entities, waterThresh, seed);
    }

    // function that handles what entities should be deleted
    public void cleanUpEntities() {
        removeEntities("Sphere");
        removeEntities("Tree(Clone)");
    }

    // temp function for dev
    private void drawEntitySphere(float[,] noiseMap, float[,] entities, float waterThresh) {
        // initialize variables
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);
        int scale = 30;
        GameObject meshObj = GameObject.Find("MeshObj");

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (entities[x, y] >= 1) {
                    // position vals for new object
                    float _x = x * meshObj.transform.localScale.x + meshObj.transform.position.x;
                    float _y = noiseMap[x, y] * meshObj.transform.localScale.y + meshObj.transform.position.y;
                    float _z = y * meshObj.transform.localScale.z + meshObj.transform.position.z;
                    // creating object
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // changing new object's settings
                    sphere.transform.position = new Vector3(_x, _y, _z);
                    sphere.transform.localScale = new Vector3(scale, scale, scale);
                    sphere.GetComponent<Renderer>().sharedMaterial.color = new Color(1,0.1f,0.1f,1);
                }
            }
        }
    }

    // clean up GameObjects named n (bit of a crude implementation)
    private void removeEntities(string n) {
        object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object obj in objects) {
            GameObject o = (GameObject) obj;
            if (o.name == n) {
                DestroyImmediate(o);
                //Destroy(o, 0);
            }
        }
    }

    // function that places trees on mesh (TODO: make new tree objects a child object & find a way to change seed in tree component)
    private void drawEntityTree(float[,] noiseMap, float[,] entities, float waterThresh, int seed) {
        // check for invalid input(s)
        if (treeObj == null) {
            throw new Exception();
        }

        // initialize variables
        int mapWidth = noiseMap.GetLength(0);
        int mapHeight = noiseMap.GetLength(1);
        GameObject meshObj = GameObject.Find("MeshObj");
        //Transform treeParent = treeObj.transform;
        System.Random prng = new System.Random(seed);
        GameObject t;

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                if (entities[x, y] >= 1) {
                    float _x = x * meshObj.transform.localScale.x + meshObj.transform.position.x;
                    float _y = noiseMap[x, y] * meshObj.transform.localScale.y + meshObj.transform.position.y;
                    float _z = y * meshObj.transform.localScale.z + meshObj.transform.position.z;

                    t = Instantiate(treeObj,  new Vector3(_x, _y, _z), Quaternion.identity);
                    // should set tree as child, but unity will break
                    //t.transform.parent = treeParent;
                    /*
                    var tData = t.data as TreeEditor.TreeData;
                    var root = tData.root;
                    root.seed = prng.Next(-100000, 100000);
                    */
                }
            }
        }
    }

    // WIP (suppose to put a portal-thing that acts as spawnpoint for player)
    public void drawPortal(float[,] noiseMap, int portalPointX, int portalPointZ) {
        int scale = 30;
        GameObject meshObj = GameObject.Find("MeshObj");
        float _x = portalPointX * meshObj.transform.localScale.x + meshObj.transform.position.x;
        float _y = noiseMap[portalPointX, portalPointZ] * meshObj.transform.localScale.y + meshObj.transform.position.y;
        float _z = portalPointZ * meshObj.transform.localScale.z + meshObj.transform.position.z;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(_x, _y, _z);
        sphere.transform.localScale = new Vector3(scale, scale, scale);
        sphere.GetComponent<Renderer>().sharedMaterial.color = new Color(0.1f,1,0.1f,1);
    }
}

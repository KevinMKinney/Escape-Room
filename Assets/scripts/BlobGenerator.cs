using System.Collections.Generic;
using UnityEngine;

/* 
 * Written this semester.
 */

/** <summary>
 * The MeshInfo struct stores info about a mesh.
 * It can be combined with other MeshInfo structs to make one mesh.
 * </summary>
 */
struct MeshInfo
{
    //Basic Mesh Data.
    public int[] triangles;
    public Vector3[] vertices;
    public Vector2[] uvs;

    ///<summary>Constructs the object using the specified arrays</summary>
    public MeshInfo(int[] triangles, Vector3[] vertices, Vector2[] uvs)
    {
        //Set it's properties to the passed in arrays.
        this.triangles = triangles;
        this.vertices = vertices;
        this.uvs = uvs;
    }

    /**<summary>Removes a vertex from the mesh data</summary>
     * <param name="index">The index of the vertex to delete</param>
     */
    public void DeleteVertex(int index)
    {
        /* Find any triangles referencing vertices with an index higher than the index of the vertex being deleted.
         * Subtract one from thair index because the indexs higher than index will be shifted when an element is removed.
         */
        for (int i = 0; i < triangles.Length; i++)
        {
            if (triangles[i] > index)
            {
                triangles[i]--;
            }
        }
        //Create new arrays for vertices and uvs, one shorter than the orginal becuase of the removed element.
        Vector3[] newVertices = new Vector3[vertices.Length - 1];
        Vector2[] newUvs = new Vector2[uvs.Length - 1];
        //Counter for the index of the element of these arrays we are on.
        int j = 0;
        //Loop through all the vertices copy them to to our new arrays, except for the element at index.
        for (int i = 0; i < vertices.Length; i++)
        {
            //i is the index for the element in the old arrays, j is for the index in the new arrays. Skip this element if the element is the vertex that needs to be deleted.
            if (i != index)
            {
                //Copy the vertex and it's uv counterpart.
                newVertices[j] = vertices[i];
                newUvs[j] = uvs[i];
                j++;
            }
        }
        //Use the new Vertices and Uvs instead of the old ones.
        vertices = newVertices;
        uvs = newUvs;
    }

    ///<summary>Removes any duplicate vertices</summary>
    public void Collapse()
    {
        //End Delete represents indexs that need to be deleted.
        List<int> endDelete = new();
        //No Delete represents indexs that are in use and cannot be deleted.
        List<int> noDelete = new();
        //Loop through all of the vertices.
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            //Loop through all of the vertices (again)
            for (int j = 0; j < vertices.Length; j++)
            {
                Vector3 vertex2 = vertices[j];
                //If the vertices are in the same position, but the indexs are not the same, and we have not already removed this duplicate pair.
                if (i != j && vertex == vertex2 && !endDelete.Contains(j) && !endDelete.Contains(i) && !noDelete.Contains(i) && !noDelete.Contains(j))
                {
                    //Find any triangles that referance the duplicate vertex and set the referance to the first vertex.
                    for (int k = 0; k < triangles.Length; k++)
                    {
                        if (triangles[k] == j)
                        {
                            triangles[k] = i;
                        }
                    }
                    //i needs to be saved, but j needs to be deleted.
                    noDelete.Add(i);
                    endDelete.Add(j);
                }
            }
        }

        //Stores indexs that have already been deleted.
        List<int> ints = new();
        //Now that we have the vertices that need to be deleted, Delete them while keeping indexs the same.
        foreach (int item in endDelete)
        {
            int newItem = item;
            //If the index is more than any of the previous deleted indexs than the index needs to be shifted to keep the referance on the right vertices, since all of the indexes get shifted after each deletion.
            foreach (int item2 in ints)
            {
                if (item > item2)
                {
                    newItem--;
                }
            }
            //Delete the correct vertex with one last check to make sure the vertice is not in use.
            DeleteVertex(newItem);
            //Add index to indexs already used.
            ints.Add(item);
        }
    }

    /**<summary> This operator combines the data from two MeshInfo objects and returns it.</summary>
     * Using the operator keyword we define our own operator +.
     * When two MeshInfo objects are added using the + sign this function will be called with 'a' being the object on the left and 'b' the object on the right.
     * Because we overide the + operator, when a variable uses += the varable will be set to the result of this operation.
     */
    public static MeshInfo operator +(MeshInfo a, MeshInfo b)
    {
        //The mesh info object to return.
        MeshInfo info = new MeshInfo(new int[a.triangles.Length + b.triangles.Length], new Vector3[a.vertices.Length + b.vertices.Length], new Vector2[a.uvs.Length + b.uvs.Length]);

        //Copy uvs from object 'a' to the return object.
        for (int i = 0; i < a.uvs.Length; i++)
        {
            info.uvs[i] = a.uvs[i];
        }
        //Add uvs from object 'b' to the return object.
        for (int i = 0; i < b.uvs.Length; i++)
        {
            info.uvs[i + a.uvs.Length] = b.uvs[i];
        }
        //Copy vertices from object 'a' to the return object.
        for (int i = 0; i < a.vertices.Length; i++)
        {
            info.vertices[i] = a.vertices[i];
        }
        //Add vertices from object 'b' to the return object.
        for (int i = 0; i < b.vertices.Length; i++)
        {
            info.vertices[i + a.vertices.Length] = b.vertices[i];
        }
        //Copy triangles from object 'a' to the return object.
        for (int i = 0; i < a.triangles.Length; i++)
        {
            info.triangles[i] = a.triangles[i];
        }

        //Add triangles from object 'b' to the return object, but offset the position of the triangle's referances because the vertices will be offset in the list.
        for (int i = 0; i < b.triangles.Length; i++)
        {
            info.triangles[i + a.triangles.Length] = b.triangles[i] + a.vertices.Length;
        }

        //Return the return object.
        return info;
    }
}


/** <summary>
 * Generates blob-looking meshes using cube-sphere algrothim.
 * </summary>
 */
//This one line was generated by unity.
public static class BlobGenerator
{
    /// <summary>Takes the cross product of two Vector3s as if they are matrixes.</summary>
    static Vector3 CrossProduct(Vector3 i, Vector3 j)
    {
        float product1 = i.y * j.z;
        float product2 = i.z * j.y;
        float difference1 = product1 - product2;
        float product3 = i.z * j.x;
        float product4 = i.x * j.z;
        float difference2 = product3 - product4;
        float product5 = i.x * j.y;
        float product6 = i.y * j.x;
        float difference3 = product5 - product6;
        return new Vector3(difference1, difference2, difference3);
    }

    /** <summary>
     * Generates one side of a cube. It normalizes the points so it will look like a side of a sphere.
     * </summary>
     * <param name="one">The normal in which the side will face</param>
     * <param name="design">How high resolution the face will be. Should be more than one</param>
     * <returns>The MeshInfo object generated</returns>
     */
    static MeshInfo SideCube(Vector3 one, int design)
    {
        //Get the vectors so that we know what direction to generate the points at.
        Vector3 vector1 = new Vector3(one.y, one.z, one.x);
        Vector3 vector2 = CrossProduct(one, vector1);
        //Create the vertex, uv, and triangle arrays.
        Vector3[] vertices = new Vector3[design * design];
        Vector2[] uvs = new Vector2[design * design];
        int[] triangles = new int[(design - 1) * (design - 1) * 6];

        //The index for the triangle array.
        int tMaker = 0;
        //Loop over a grid like pattern.
        for (int y = 0; y < design; y++)
        {
            for (int x = 0; x < design; x++)
            {
                //Find the proper vertex index.
                int vMaker = x + y * design;
                //v is the uv position we are at on the cube-sphere.
                Vector2 v = new Vector2(x, y) / (design - 1f);
                //loca is the position our vertex will go.
                Vector3 loca = one + vector1 * (2 * v.x - 1) + vector2 * (2 * v.y - 1);
                //Set the correct vertex to the position we calculated. The normalised vector turnes the cub into a sphere.
                vertices[vMaker] = loca.normalized;
                //Set the correct uv to the uv we calculated.
                uvs[vMaker] = v;
                //If we are not at the edge of our mesh proceed with creating the face.
                if (x != design - 1 && y != design - 1)
                {
                    //First triangle.
                    triangles[tMaker + 0] = vMaker;
                    triangles[tMaker + 1] = vMaker + design + 1;
                    triangles[tMaker + 2] = vMaker + design;

                    //Second Triangle.
                    triangles[tMaker + 3] = vMaker;
                    triangles[tMaker + 4] = vMaker + 1;
                    triangles[tMaker + 5] = vMaker + design + 1;
                    //These triangles form a box, otherwise known as a quad or face.

                    //Increment the triangle index.
                    tMaker += 6;
                }
            }
        }
        //Return a new MeshInfo object with the data we generated.
        return new MeshInfo(triangles, vertices, uvs);
    }
    /** <summary>
     * Generates a full sphere using the cube-sphere algrothim.
     * </summary>
     * <param name="design">How high resolution the sphere will be. Should be more than one</param>
     * <returns>The six MeshInfo objects generated, one for every side</returns>
     */
    static MeshInfo[] GenSideCube(int design)
    {
        //The mesh info array for every side of the cube-sphere.
        MeshInfo[] veryMeshInfo = new MeshInfo[6];
        //All the directions the faces will generate facing.
        Vector3[] sidesOfCube =
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };
        //For every side of the cube, generate the side.
        for (int i = 0; i < sidesOfCube.Length; i++)
        {
            veryMeshInfo[i] = SideCube(sidesOfCube[i], design);
        }
        //Return the list of MeshInfo
        return veryMeshInfo;
    }
    /** <summary>
     * Assembles a sphere mesh using the cube-sphere algrothim. It then applies some noise over the top of the sphere, making the sphere look like a blob.
     * </summary>
     * <returns>A blob-looking mesh</returns>
     */
    public static Mesh GenerateBlobMesh()
    {
        //The generated mesh.
        Mesh mesh = new Mesh();
        //Create an empty info object.
        MeshInfo info = new MeshInfo(new int[0], new Vector3[0], new Vector2[0]);

        //Generate all of the sides of the cube.
        foreach (MeshInfo meshInfo in GenSideCube(15))
        {
            //Add each side's MeshInfo to the globel MeshInfo object.
            info += meshInfo;
        }

        //Remove unnecessary vertices.
        info.Collapse();

        //Set our mesh's properties to the properties we generated.
        mesh.vertices = info.vertices;
        mesh.triangles = info.triangles;
        mesh.uv = info.uvs;
        //Tell unity to generate normals on our mesh.
        mesh.RecalculateNormals();

        //Create an copy array of the mesh's vertices.
        Vector3[] newVerts = mesh.vertices;

        //Loop over all the vertices.
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            //Reduce the size of the cube-sphere.
            newVerts[i] -= mesh.normals[i] * 0.5f;
            //Add perlin noise to the top of cube-sphere using the uvs.
            newVerts[i] += mesh.normals[i] * Mathf.PerlinNoise(mesh.uv[i].x * 2, mesh.uv[i].y * 2) * 0.5f;
        }

        //Set new vertices to the mesh.
        mesh.vertices = newVerts;
        
        //Recalulate and optimize the mesh.
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();
        mesh.OptimizeIndexBuffers();
        mesh.OptimizeReorderVertexBuffer();

        //Return the result.
        return mesh;
    }

    /** <summary>
     * Returns a gameobject rendering a generated blob mesh.
     * </summary>
     * <returns>A blob-looking game object</returns>
     */
    public static GameObject GenerateBlobGameObject()
    {
        //Create the game object with a mesh filter and mesh render.
        GameObject sphere = new("Blob", new System.Type[] { typeof(MeshFilter), typeof(MeshRenderer) });
        //Generate a blob mesh.
        Mesh mesh = GenerateBlobMesh();
        //Make the game object render the mesh.
        sphere.GetComponent<MeshFilter>().mesh = mesh;
        //Return the gameobject.
        return sphere;
    }
}
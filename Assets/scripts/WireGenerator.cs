using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* 
 * Written this semester, starting on line 13.
 * Generates a wire mesh using UnityEngine�s Mesh API.
 * It then makes the current GameObject render the procedurally generated mesh.
 */
//This one line was generated by unity
public class WireGenerator : MonoBehaviour
{
    //The mesh that will be generated
    public Mesh mesh;
    //How big the wire will be
    public float radius = 1;
    //How smooth the curve will be, smaller values makes the curve look pointy.
    public float smoothness = 7;
    //How long the wire will be
    public int length = 20;

    //Math function to get the height of a point given x. When fed liner values it creates the curve of the wire, making it look cylindrical. See:
    //https://graphtoy.com/?f1(x,t)=sqrt(8%5E2-x%5E2)%20
    float GetHeight(float x)
    {
        return Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x, 2));
    }

    //When the game starts generate the mesh
    void Start()
    {
        //Create an empty mesh with empty uvs, triangles, and vertices.
        mesh = new Mesh();
        List<Vector3> verts = mesh.vertices.ToList();
        List<int> tris = mesh.triangles.ToList();
        List<Vector2> uvs = mesh.uv.ToList();

        //Loop over a grid of points.
        for (float i = -radius; i <= radius; i+= radius/smoothness)
        {
            for (int j = 0; j < length; j++)
            {
                //Add a new position to the vertices array. i and j are the coordinets in the grid. It will pass i to the GetHeight making a curve shape.
                verts.Add(new Vector3(i, GetHeight(i), j));
                //Set proper Texture Coordinates
                uvs.Add(new Vector2(i,j));
            }
        }


        /*
         * Right now we have a list of vertices, or positions, that form a long cylindrical shape (like a wire). 
         * But because the mesh does not have any triangles nothing will be rendered.
         * 
         * Loop over the some of the vertices and create faces so the wire will look solid.
         */
        for (int i = 0; i < verts.Count - length - 1; i++)
        {
            //Skip the end points or extra triangles will be added, and no one likes that!
            if ((i+1) % length != 0)
            {
                //Now create a face beteewn the points of i, i+1, i+length, i + length+1
                //First triangle
                tris.Add(i);
                tris.Add(i+1);
                tris.Add(i+length);

                //Second triangle
                tris.Add(i+length);
                tris.Add(i + 1);
                tris.Add(i+1+length);
                //These triangles form a box, otherwise known as a quad or face.
            }
        }
        
        //Set our mesh's properties to the properties we generated.
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        //Tell unity to display our mesh.
        GetComponent<MeshFilter>().mesh = mesh;
     }
}

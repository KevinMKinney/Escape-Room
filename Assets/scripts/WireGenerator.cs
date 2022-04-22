using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WireGenerator : MonoBehaviour
{
    public Mesh mesh;
    public float radius = 1;
    public float smoothness = 5;
    public int length = 20;
    float GetHeight(float x)
    {
        return Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x, 2));

    }

    void Start()
    {
        mesh = new Mesh();
        List<Vector3> verts = mesh.vertices.ToList();
        List<int> tris = mesh.triangles.ToList();
        List<Vector2> uvs = mesh.uv.ToList();

        for (float i = -radius; i <= radius; i+= radius/smoothness)
        {
            for (int j = 0; j <= length; j++)
            {
                verts.Add(new Vector3(i + 10, GetHeight(i), j));
                uvs.Add(new Vector2(i,j));
            }
        }

        for(int i = 0; i < verts.Count - length; i++)
        {
            if(i % length != 0)
            {
                tris.Add(i);
                tris.Add(i+1);
                tris.Add(i+length);

                tris.Add(i+length);
                tris.Add(i + 1);
                tris.Add(i+1+length);
            }
        }
        
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        GetComponent<MeshFilter>().mesh = mesh;
     }
}

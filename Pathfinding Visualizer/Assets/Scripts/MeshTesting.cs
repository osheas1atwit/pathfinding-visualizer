using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTesting : MonoBehaviour
{
    void Start()
    {

        Debug.Log("Mesh Testing");

        Mesh mesh = new Mesh();


        // Build required data-structures for triangle mesh
        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        // Set vertices of polygon (triangle)
        vertices[0] = new Vector3(  0,   0);
        vertices[1] = new Vector3(  0, 100);
        vertices[2] = new Vector3(100, 100);

        // Set order of indexes of vertices to draw polygon (v1 -> v2 -> v3 -> v1)
        // Note: always set clockwise, otherwise mesh will be facing backwards
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
        
    }
}

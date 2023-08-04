using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTesting : MonoBehaviour
{

    Material m_Material;
    
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;

        Debug.Log("Mesh Testing");

        Mesh mesh = new Mesh();


        // Build required data-structures for triangle mesh
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        // Set vertices of polygon (2 triangles -> 1 quad)
        vertices[0] = new Vector3(  0,   0);
        vertices[1] = new Vector3(  0, 10);
        vertices[2] = new Vector3(10, 10);
        vertices[3] = new Vector3(10,   0);



        // UV index contains texture position that should match to vertex with same index
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);


        // Set order of indexes of vertices to draw polygon (v1 -> v2 -> v3 -> v1)
        // Note: always set clockwise, otherwise mesh will be facing backwards
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
        
    }

    private void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            Debug.Log("1");
            m_Material.color = Color.white;
            //m_Material.SetColor("_Color", Color.white);
        }

        if (Input.GetKeyDown("2"))
        {
            Debug.Log("2");
          m_Material.color = Color.black;
            //m_Material.SetColor("_Color", Color.white);
        }

        if (Input.GetKeyDown("3"))
        {
            Debug.Log("3");
         m_Material.color = Color.green;
            //m_Material.SetColor("_Color", Color.white);
        }
    }
}

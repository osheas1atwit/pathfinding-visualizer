using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMesh : MonoBehaviour
{

    Material m_Material;

    private LogicGrid grid;
    private Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(LogicGrid grid)
    {
        this.grid = grid;
        UpdateCellVisual();
    }

    private void UpdateCellVisual()
    {
        CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for(int x = 0; x < grid.GetWidth(); x++)
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;

                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                AddQuad(vertices, uv, triangles, index, grid.GetWorldPosition(x, y), quadSize, Vector2.zero);
            }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void Start()
    {
        /**
        m_Material = GetComponent<Renderer>().material;

        Debug.Log("Mesh Testing");

        Mesh mesh = new Mesh();


        // Build required data-structures for square mesh
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        // Set vertices of polygon (2 triangles -> 1 quad)
        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 10);
        vertices[2] = new Vector3(10, 10);
        vertices[3] = new Vector3(10, 0);

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
        */
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


    // CREDIT FOR BELOW FUNCTIONS: CODEMONKEY 
    // https://www.youtube.com/watch?v=mZzZXfySeFQ
    public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {
        vertices = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }


    private void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)
    {
        vertices[index * 4] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

        Debug.Log(vertices[0]);
        Debug.Log(vertices[1]);
        Debug.Log(vertices[2]);
        Debug.Log(vertices[3]);

        uvs[(index * 4)] = Uv;
        uvs[(index * 4) + 1] = Uv;
        uvs[(index * 4) + 2] = Uv;
        uvs[(index * 4) + 3] = Uv;

        triangles[(index * 6) + 0] = (index * 4) + 0;
        triangles[(index * 6) + 1] = (index * 4) + 1;
        triangles[(index * 6) + 2] = (index * 4) + 2;
        triangles[(index * 6) + 3] = (index * 4) + 2;
        triangles[(index * 6) + 4] = (index * 4) + 3;
        triangles[(index * 6) + 5] = (index * 4) + 0;
    }

    /*    private void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)
        {
            vertices[index * 4] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);
            vertices[(index * 4) + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
            vertices[(index * 4) + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
            vertices[(index * 4) + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

            Debug.Log(vertices[0]);
            Debug.Log(vertices[1]);
            Debug.Log(vertices[2]);
            Debug.Log(vertices[3]);

            uvs[(index * 4)] = Uv;
            uvs[(index * 4) + 1] = Uv;
            uvs[(index * 4) + 2] = Uv;
            uvs[(index * 4) + 3] = Uv;

            triangles[(index * 6) + 0] = (index * 4) + 0;
            triangles[(index * 6) + 1] = (index * 4) + 1;
            triangles[(index * 6) + 2] = (index * 4) + 2;
            triangles[(index * 6) + 3] = (index * 4) + 2;
            triangles[(index * 6) + 4] = (index * 4) + 3;
            triangles[(index * 6) + 5] = (index * 4) + 0;
        }*/
}

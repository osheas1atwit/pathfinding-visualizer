using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMesh : MonoBehaviour
{
    private LogicGrid grid;
    private Mesh mesh;

    Vector3[] m_vertices;
    Vector2[] m_uv;
    int[] m_triangles;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(LogicGrid grid)
    {
        this.grid = grid;
        UpdateCellVisual();

        // Subscribe to event
        grid.OnGridValueChanged += GridValueChanged;
    }

    private void GridValueChanged(object sender, LogicGrid.OnGridValueChangedEventArgs e)
    {
        Debug.Log(e.x + " " + e.y);
        UpdateCellVisual();
    }

    // TODO: implement this so we can update a single cell, given an x, y coordinate
    private void UpdateCellVisual(int x, int y)
    {
        //Debug.Log("Updated in Isolation 8)");
        Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();
        int index = x * grid.GetHeight() + y;

        int gridValue = grid.GetValue(x, y);
        float gridValueNormalized = gridValue / 3f;
        Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

        MeshUtils.AddToMeshArrays(m_vertices, m_uv, m_triangles, index, grid.GetWorldPosition(x, y) + (quadSize * .5f), 0f, quadSize, gridValueUV, gridValueUV);

    }

    // Update all cells at once
    private void UpdateCellVisual()
    {
        Debug.Log("Updated as a group");

        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for(int x = 0; x < grid.GetWidth(); x++)
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;

                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                int gridValue = grid.GetValue(x, y);
                float gridValueNormalized = gridValue / 3f;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                
                
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + (quadSize * .5f), 0f, quadSize, gridValueUV, gridValueUV);
            }

        m_vertices = vertices;
        m_uv = uv;
        m_triangles = triangles;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void Start()
    {
        
    }

    private void Update()
    {

    }
}

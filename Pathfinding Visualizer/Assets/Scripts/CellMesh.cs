using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellMesh : MonoBehaviour
{
    private LogicGrid grid;
    private Mesh mesh;

    Vector3[] m_vertices;
    Vector2[] m_uv;
    int[] m_triangles;

    Stack<Node> animateData;

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
        // if(e.x == -1 && e.y == -1), grid is being reset
            
        //Debug.Log(e.x + " " + e.y);
        UpdateCellVisual();
    }

    // TODO: implement this so we can update a single cell, given an x, y coordinate
/*    private void UpdateCellVisual(int x, int y)
    {
        //Debug.Log("Updated in Isolation 8)");
        Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();
        int index = x * grid.GetHeight() + y;

        int gridValue = grid.GetValue(x, y);
        float gridValueNormalized = gridValue / 3f;
        Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

        MeshUtils.AddToMeshArrays(m_vertices, m_uv, m_triangles, index, grid.GetWorldPosition(x, y) + (quadSize * .5f), 0f, quadSize, gridValueUV, gridValueUV);

    }*/

    // Update all cells at once
    private void UpdateCellVisual()
    {
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


    public void Animate(Stack<Node> result)
    {
        animateData = result;
        StartCoroutine("AnimateGrid");
    }


    IEnumerator AnimateGrid()
    {
        int count = animateData.Count;
        for (int i = 0; i < count; i++)
        {
            Node n = animateData.Pop();
            //Debug.Log("Agent at: " + n.agent.x + ", " + n.agent.y);
            Debug.Log(n.lastMove);

            grid.SetValue(n.agent.x, n.agent.y, 3);
            grid.SetValue(n.parent.agent.x, n.parent.agent.y, 0);

            yield return new WaitForSeconds(nMain.speed);

            UpdateCellVisual();
        }
    }

    void Start()
    {
        
    }

    private void Update()
    {

    }
}

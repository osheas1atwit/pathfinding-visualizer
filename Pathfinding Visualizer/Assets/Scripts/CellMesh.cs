using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMesh : MonoBehaviour
{
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

        // Subscribe to event
        grid.OnGridValueChanged += GridValueChanged;
    }

    private void GridValueChanged(object sender, LogicGrid.OnGridValueChangedEventArgs e)
    {
        Debug.Log("FIRE!!");
        UpdateCellVisual();
    }

    private void UpdateCellVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for(int x = 0; x < grid.GetWidth(); x++)
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;

                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                int gridValue = grid.GetValue(x, y);
                float gridValueNormalized = gridValue / 4f;
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);
                
                
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + (quadSize * .5f), 0f, quadSize, gridValueUV, gridValueUV);
            }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    void Start()
    {
        
    }


    /*                MeshRenderer myRenderer = GetComponent<MeshRenderer>();
                Material m_material;
                if (myRenderer != null)
                {
                    m_material = myRenderer.material;

                    if(gridValue == -1)
                    {
                        m_material.color = Color.black; 
                    }
                    if (gridValue == 0)
                    {
                        m_material.color = Color.white;
                    }
                    if (gridValue == 1)
                    {
                        m_material.color = Color.green;
                    }
                    if (gridValue == 2)
                    {
                        m_material.color = Color.cyan;
                    }*//*
                }
*/
    private void Update()
    {
/*        if(Input.GetKeyDown("1"))
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
        }*/
    }
}

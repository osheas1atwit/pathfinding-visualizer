using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField] private CellMesh cellMesh;
    
    private LogicGrid world;
    int placementValue;

    bool astarSelected;


    // Coordinates for pathfinding
    public Vector2Int agent = new Vector2Int(-1, -1);
    public List<Vector2Int> obstacles = new List<Vector2Int>();
    public List<Vector2Int> samples = new List<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        // Create world
        int width = 4;
        int height = 4;
        float cellSize = 10f;
        Vector3 origin = new Vector3(-10, -10);

        world = new LogicGrid(width, height, cellSize, origin);
        
        // Set default placement to Obstacle
        placementValue = 1;

        cellMesh.SetGrid(world);

        Debug.Log("Loaded. Placing Mode: Obstacle");

    }

    // Update contains keyboard shortcut options
    void Update()
    {
        // Change Placement Mode Keyboard Shortcuts:
        ////////////////////////////////
        // Place Obstacle in grid
        if (Input.GetKeyDown("o"))
        {
            Debug.Log("Placing Mode: Obstacle");
            placementValue = 1;
        }
        // Place Sample in cell
        if(Input.GetKeyDown("s"))
        {
            Debug.Log("Placing Mode: Sample");
            placementValue = 2;
        }
        // Place agent in cell
        if (Input.GetKeyDown("a"))
        {
            Debug.Log("Placing Mode: Agent");
            placementValue = 3;
        }
        // Reset Canvas
        if (Input.GetKeyDown("r"))
        {
            ResetGrid();
        }

        if (Input.GetKeyDown("g"))
        {
            StartAlgorithm();
        }

        //////////////////////////

        // Update Cell:
        // Place value in cell based on selected placementValue with left click
        if (Input.GetMouseButton(0))
        {
            world.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), placementValue);
        }
        // Clear cell with right click
        if (Input.GetMouseButton(1))
        {
            world.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), 0);
        }
    }

    public void SetModeObstacle()
    {
        placementValue = 1;
    }
    public void SetModeSample()
    {
        placementValue = 2;
    }
    public void SetModeAgent()
    {
        placementValue = 3;
    }
    public void ResetGrid()
    {
        world.reset();
        agent = new Vector2Int(-1, -1);
        obstacles = new List<Vector2Int>();
        samples = new List<Vector2Int>();
    }
   
    public void StartAlgorithm()
    {
        Vector2Int coord = new Vector2Int();

        // Collect information about world
        for (int x = 0; x < world.GetWidth(); x++)
            for(int y = 0; y < world.GetHeight(); y++)
            {
                int index = x * world.GetHeight() + y;
                int value = world.GetValue(x, y);

                //Debug.Log("x: " + x + " y: " + y + " | value: " + value);

                // Add coordinate to obstacle array
                if (value == 1)
                {
                    coord.x = x;
                    coord.y = y;

                    if (!obstacles.Contains(coord))
                        obstacles.Add(coord);
                }
                // Add coordinate to sample array
                if (value == 2)
                {
                    coord.x = x;
                    coord.y = y;

                    if (!samples.Contains(coord))
                        samples.Add(coord);
                }
                // Add coordinate to obstacle array
                if (value == 3)
                {
                    agent.x = x;
                    agent.y = y;
                }
            }

        if (agent.x == -1 && agent.y == -1)
        {
            Debug.Log("NO AGENT!");
            return;
        }

        if (samples.Count == 0)
        {
            Debug.Log("NO SAMPLES!");
            return;
        }
        

        if ( true )
        {
            Pathfinder path = new Pathfinder(world, agent, obstacles, samples, 0, 0);
        }
    }

}

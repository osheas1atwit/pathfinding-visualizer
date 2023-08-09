using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nMain : MonoBehaviour
{

    [SerializeField] private CellMesh cellMesh;

    private LogicGrid world;
    int placementValue;

    public float speed = 3.0f;

    int heuristic;
    int algorithm;
    bool astarSelected;
    bool idsSelected;
    bool dfsSelected;


    // Coordinates for pathfinding
    public Vector2Int agent = new Vector2Int();
    public HashSet<Vector2Int> obstacles = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> samples = new HashSet<Vector2Int>();

    // Start is called before the first frame update
    void Start()
    {
        // Create world
        int width = 32;
        int height = 16;
        float cellSize = 5f;
        Vector3 origin = new Vector3(-77, -34);

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
        if (Input.GetKeyDown("s"))
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
        agent = new Vector2Int();
        obstacles = new HashSet<Vector2Int>();
        samples = new HashSet<Vector2Int>();
    }

    public void StartAlgorithm()
    {
        Vector2Int coord = new Vector2Int();
        bool agentFound = false;

        // Collect information about world
        for (int x = 0; x < world.GetWidth(); x++)
            for (int y = 0; y < world.GetHeight(); y++)
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
                // Add coordinate to agent array
                if (value == 3)
                {
                    agent.x = x;
                    agent.y = y;
                    agentFound = true;
                }
            }

        if (!agentFound)
        {
            Debug.Log("NO AGENT!");
            return;
        }

        if (samples.Count == 0)
        {
            Debug.Log("NO SAMPLES!");
            return;
        }

        if (true)
        {
            Pathfinder path = new Pathfinder(world, agent, obstacles, samples, algorithm, heuristic);
            Stack<Node> result = path.result;
            Debug.Log("GO ANIMATE");
            cellMesh.Animate(result);
        }
    }


    public void AdjustSpeed(float newSpeed)
    {
        speed = newSpeed;
        Debug.Log(speed);
    }

    public void AstarToggle(bool tickOn)
    {
        astarSelected = tickOn;
        Debug.Log(astarSelected);
        algorithm = 0;
        Debug.Log(algorithm);

    }

    public void DFSToggle(bool tickOn)
    {
        dfsSelected = tickOn;
        Debug.Log(dfsSelected);
        algorithm = 1;
        Debug.Log(algorithm);
    }

    public void IDSToggle(bool tickOn)
    {
        idsSelected = tickOn;
        Debug.Log(idsSelected);
        algorithm = 2;
        Debug.Log(algorithm);

    }

    public void h0Toggle(bool tickOn)
    {
        
        heuristic = 0;
        Debug.Log(heuristic);

    }

    public void h1Toggle(bool tickOn)
    {
        heuristic = 1;
        Debug.Log(heuristic);
    }

    public void h2Toggle(bool tickOn)
    {
        
        heuristic = 2;
        Debug.Log(heuristic);

    }

}

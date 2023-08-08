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
    public Vector2 agent = new Vector2();
    public List<Vector2> obstacles = new List<Vector2>();
    public List<Vector2> samples = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        // Create world
        int width = 5;
        int height = 5;
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

        // Make functionality for when "start" button is pressed.
            // should call some sort of function that will take in the world
            // and perform the selected algorithm

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
    }
   
    public void StartAlgorithm()
    {
        Vector2 coord = new Vector2();

        // Collect information about world
        for (int x = 0; x < world.GetWidth(); x++)
            for(int y = 0; y < world.GetHeight(); y++)
            {
                int index = x * world.GetHeight() + y;
                int value = world.GetValue(x, y);

                Debug.Log("x: " + x + " y: " + y + " | value: " + value);

                // Add coordinate to obstacle array
                if (value == 1)
                {
                    //coord = new int[2][];

                    coord.x = x;
                    coord.y = y;

                    if (!obstacles.Contains(coord))
                        obstacles.Add(coord);
                }
                // Add coordinate to sample array
                if (value == 2)
                {
                    //coord = new int[2][];

                    coord.x = x;
                    coord.y = y;

                    samples.Add(coord);
                }
                // Add coordinate to obstacle array
                if (value == 3)
                {
                    agent[0] = x;
                    agent[1] = y;
                }
            }


        if ( true )
        {
            Pathfinder astar = new Pathfinder(world, agent, obstacles, samples, 0, 0);
            astar.go();
        }
    }

}

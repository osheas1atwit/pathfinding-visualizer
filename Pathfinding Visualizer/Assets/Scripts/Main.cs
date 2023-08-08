using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField] private CellMesh cellMesh;
    
    private LogicGrid world;
    char placementMode;
    int placementValue;


    // Start is called before the first frame update
    void Start()
    {
        // Create world
        int width = 100;
        int height = 100;
        float cellSize = 5f;
        Vector3 origin = new Vector3(-110, -110);

        world = new LogicGrid(width, height, cellSize, origin);
        
        // Set default placement to Obstacle
        placementValue = 1;
        placementMode = 'o';

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
            placementMode = 'o';
            placementValue = 1;
        }
        // Place Sample in cell
        if(Input.GetKeyDown("s"))
        {
            Debug.Log("Placing Mode: Sample");
            placementMode = 's';
            placementValue = 2;
        }
        // Place agent in cell
        if (Input.GetKeyDown("a"))
        {
            Debug.Log("Placing Mode: Agent");
            placementMode = 'a';
            placementValue = 3;
        }
        // Reset Canvas
        if (Input.GetKeyDown("r"))
        {
            ResetGrid();
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

    public void StartAStar()
    {
        AStar astar = new AStar(world, 0);
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
   


}

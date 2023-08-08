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
        world = new LogicGrid(100, 100, 5f, new Vector3(-110, -110));
        
        // Set default placement to Obstacle
        placementValue = 1;
        placementMode = 'o';

        cellMesh.SetGrid(world);

        Debug.Log("Loaded. Placing Mode: Obstacle");

    }

    // Update is called once per frame
    void Update()
    {
        // Change Placement Modes:

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

        }


        // Update Cell:
        // Change cell to obstacle
        if (Input.GetMouseButton(0))
        {
            world.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), placementValue);
        }
        // Clear cell
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


}

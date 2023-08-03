using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    private LogicGrid world;
    char placementMode;
    int placementValue;

    // Start is called before the first frame update
    void Start()
    {
        // Create world
        world = new LogicGrid(5, 5, 10f, new Vector3(0, 0));
        
        // Set default placement to Obstacle
        placementValue = -1;
        placementMode = 'o';
        Debug.Log("Loaded. Placing Mode: Obstacle");

    }

    // Update is called once per frame
    void Update()
    {
        // Change Placement Modes:
        // Place Agent in grid
        // TODO change modes via UI button
        if(Input.GetKeyDown("a"))
        {
            Debug.Log("Placing Mode: Agent");
            placementMode = 'a';
            placementValue = 2;
        }
        // Place Sample in grid
        if(Input.GetKeyDown("s"))
        {
            Debug.Log("Placing Mode: Sample");
            placementMode = 's';
            placementValue = 1;
        }
        // Place Obstacle in grid
        if(Input.GetKeyDown("o"))
        {
            Debug.Log("Placing Mode: Obstacle");
            placementMode = 'o';
            placementValue = -1;
        }

        
        // Update Cell:
        // Change cell to obstacle
        if (Input.GetMouseButtonDown(0))
        {
            world.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), placementValue);
        }
        // Clear cell
        if (Input.GetMouseButtonDown(1))
        {
            world.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), 0);
        }

        // Make functionality for when "start" button is pressed.
            // should call some sort of function that will take in the world
            // and perform the selected algorithm

    }
}

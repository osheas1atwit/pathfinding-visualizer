using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// INSPIRED BY "CODE MONKEY" ON YOUTUBE
public class LogicGrid
{
    private int width;
    private int height;
    private float cellSize;

    private int[,] gridArray;

    public LogicGrid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        // Used to place cell values in middle of cell
        Vector3 textOffset = new Vector3(cellSize/2, cellSize/2); 

        // Visualize the grid
        for(int x = 0; x < gridArray.GetLength(0); x++)
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                // Create text that shows the value of the number in the given cell.
                CodeMonkey.Utils.UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + textOffset, 20, Color.white, TextAnchor.MiddleCenter);

                // Give the cells a visual representation (must enable gizmos to see)
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            }

        // Draw top-line for whole grid
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        // Draw right-line for whole grid
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

    }

    // enables us to scale grid. cells would otherwise be 1x1 (very small)
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * this.cellSize;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// INSPIRED BY "CODE MONKEY" ON YOUTUBE
public class LogicGrid
{
    private int width;
    private int height;
    private float cellSize;

    private Vector3 originPosition;

    // Actual Grid Array
    private int[,] gridArray;
    // Debug Grid Array used for updating the text-objects 
    private TextMesh[,] debugTextArray;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }



    public LogicGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        // Used to place cell values in middle of cell
        Vector3 textOffset = new Vector3(cellSize/2, cellSize/2); 

        // Visualize the grid
        for(int x = 0; x < gridArray.GetLength(0); x++)
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                // Create text that prints the value of a given cell inside of that cell.
                debugTextArray[x, y] = CodeMonkey.Utils.UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + textOffset, 20, Color.white, TextAnchor.MiddleCenter);

                // Give the cells a visual representation (must enable gizmos to see)
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            }

        // Draw top-line for whole grid, draw right-line for whole grid
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }


    // Enables us to convert the array's (x,y) pair into the user's world-space (also used to scale grid in constructor)
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * this.cellSize + this.originPosition;
    }

    private void getXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }


    // Set cell value based on world-location (enables user interaction)
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        getXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    // Set cell value based on array index
    public void SetValue(int x, int y, int value)
    {
        if((x >= 0 && x < width) && (y >= 0 && y < height))
        {
            // Set the value in the real grid
            gridArray[x, y] = value;

            // Set the value in our debug mesh so that our Text on-screen gets updated
            debugTextArray[x, y].text = gridArray[x, y].ToString();

            if (OnGridValueChanged != null)
                OnGridValueChanged(this, new OnGridValueChangedEventArgs
                {
                    x = x,
                    y = y
                });
        }
    }

    // Get cell's value based on array index
    public int GetValue(int x, int y)
    {
        if ((x >= 0 && x < width) && (y >= 0 && y < height))
        {
            return gridArray[x, y];
        }
        return 0;
    }

    // Takes a world position, converts to array index, returns value held
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        getXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public int GetHeight()
    {
        return this.height;
    }

    public int GetWidth()
    {
        return this.width;
    }

    public float GetCellSize()
    {
        return this.cellSize;
    }

    public void reset()
    {
        gridArray = new int[width, height];

        if (OnGridValueChanged != null)
            OnGridValueChanged(this, new OnGridValueChangedEventArgs
            {
                x = -1,
                y = -1
            });
    }
}

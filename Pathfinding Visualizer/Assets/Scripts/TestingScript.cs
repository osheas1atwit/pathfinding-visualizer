using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    private LogicGrid grid;
    // Start is called before the first frame update
    private void Start()
    {
        //Vector3 origin = new Vector3(0, 0);
        grid = new LogicGrid(4, 2, 10f, new Vector3(0, 0));

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            grid.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), 1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            grid.SetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition(), 0);
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log(grid.GetValue(CodeMonkey.Utils.UtilsClass.GetMouseWorldPosition()).ToString());
        }
    }
}

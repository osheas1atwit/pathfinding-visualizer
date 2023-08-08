using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{

    LogicGrid world;
    List<int[][]> obstacles;
    List<int[][]> samples;
    int heuristic; // 0 = h0 | 

    public AStar(LogicGrid world, int heuristic)
    {
        this.world = world;
        this.obstacles = world.obstacles;
        this.samples = world.samples;
        this.heuristic = heuristic;
    }

    public void go()
    {

    }
}

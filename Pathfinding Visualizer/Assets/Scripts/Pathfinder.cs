using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    // used to store all unique states that agent has visited (holds agent and sample coords)
    public static List<State> closed = new List<State>();

    LogicGrid world;
    public static Vector2Int agent;
    public static List<Vector2Int> obstacles;
    public static List<Vector2Int> samples;

    public static int algorithm; // 0 = A* | 1 = dfs | 2 = bfs
    public static int heuristic; // 0 = h0 | 1 = h1  | 2 = h2

    public static int height;
    public static int width;


    public Pathfinder(LogicGrid world, Vector2Int a, List<Vector2Int> o, List<Vector2Int> s, int algo, int heu)
    {
        this.world = world;
        height = world.GetHeight();
        width = world.GetWidth();

        agent = a;
        obstacles = o;
        samples = s;

        algorithm = algo;
        heuristic = heu;
    }

    public void go()
    {

        Debug.Log("Agent: " + agent[0] + "," + agent[1]);

        for(int i = 0; i < obstacles.Count; i++)
        {
            Debug.Log("Obstacle " + i + ": " + obstacles[i].x + ", " + obstacles[i].y);
        }

        for (int i = 0; i < samples.Count; i++)
        {
            Debug.Log("Sample " + i + ": " + samples[i].x + ", " + samples[i].y);
        }

    }
}

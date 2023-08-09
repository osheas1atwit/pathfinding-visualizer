using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Pathfinder 
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

        printInfo();

        StartWork();

    }
    public void printInfo()
    {
        Debug.Log("Agent: " + agent[0] + "," + agent[1]);

        for (int i = 0; i < obstacles.Count; i++)
        {
            Debug.Log("Obstacle " + i + ": " + obstacles[i].x + ", " + obstacles[i].y);
        }

        for (int i = 0; i < samples.Count; i++)
        {
            Debug.Log("Sample " + i + ": " + samples[i].x + ", " + samples[i].y);
        }
    }

    public void StartWork()
    {
        Node initialState = new Node(null, agent, samples, '0', 0, 0);
        List<Node> result = new List<Node>();

        // Run AStar
        if (algorithm == 0)
        {
            result = StartAStar(initialState);
            Debug.Log(" "); 
            for(int i = 0; i < result.Count; i++)
            {
                Node n = result[i];
                Debug.Log("Agent at: " + n.agent.x + ", " + n.agent.y);
                Debug.Log(n.lastMove);
            }
        }
    }

    public static List<Node> StartAStar(Node initialState)
    {
        List<Node> solution = new List<Node>();

        List<Node> open = new List<Node>();

        
        open.Add(initialState);
        int cap = 1000;
        while (true && cap > 0)
        {
            if (!open.Any())
                return solution;

            open = open.OrderBy(Node => Node.fn).ToList();

            Node currentNode = open[0];
            open.RemoveAt(0);


            // check whether we are in goal state
            if (currentNode.samples.Count() == 0)
            {
                Debug.Log("Gottem");
                while (currentNode.parent != null)
                {
                    solution.Add(currentNode);
                    currentNode = currentNode.parent;
                }

                return solution;
            }

            // if not, expand children and continue
            List<Node> children = currentNode.expand(-1);

            if (heuristic == 0)
                h0(children);
           /* else if (heuristic.Equals("h1"))
                h1(children);
            else if (heuristic.Equals("h2"))
                h2(children);*/

            foreach (Node child in children)
            {
                open.Add(child);
            }
            cap--;
        }

        return solution;
    }

/*    private Node GetLowestNode(List<Node> NodeList)
    {
        Node lowest = NodeList[0];
        for (int i = 1; i < NodeList.Count; i++)
        {
            if (NodeList[i].fn < lowest.fn)
            {
                lowest = NodeList[i];
            }
        }
        return lowest;
    }*/

    // HEURISTICS FOR ASTAR
    public static List<Node> h0(List<Node> children)
    {
        List<Node> sorted = new List<Node>();

        for (int i = 0; i < children.Count(); i++)
        {
            // estimated moves left: zero
            children[i].heuristic = 0;
            sorted.Add(children[i]);
        }

        //sorted = sorted.OrderBy(Node => Node.fn).ToList();

        return sorted;
    }

/*    public static List<Node> h1(List<Node> children)
    {
        Queue<Node> sorted = new PriorityQueue<>(new NodeComparator());

        for (int i = 0; i < children.size(); i++)
        {
            // estimated moves left: samples left
            children.get(i).heuristic = children.get(i).samples.size();
            sorted.add(children.get(i));
        }
        return children;
    }

    public static Queue<Node> h2(List<Node> children)
    {
        Queue<Node> sorted = new PriorityQueue<>(new NodeComparator());

        if (children.size() == 0)
            return sorted;

        Node child;

        for (int i = 0; i < children.size(); i++)
        {
            // estimated moves left: 
            // distance to nearest sample + 1 (+1 to account for sample action)
            child = children.get(i);
            child.heuristic = Node.distance(child.agent, child.nearestSample()) + 1;

            sorted.add(child);
        }

        return sorted;
    }*/
}

package sampleworld;

import java.util.Scanner;
import java.util.List;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.Stack;
import java.util.Queue;
import java.util.PriorityQueue;
import java.util.LinkedList;
import java.util.concurrent.TimeUnit;

@SuppressWarnings( "javadoc" )
public class SampleWorld
{	
	//////////////////////////////////////////////////////////
	// GLOBAL VARIABLES THAT REPRESENT STATIC WORLD FEATURES//
	//////////////////////////////////////////////////////////
	
	// global variables used to hold world dimensions
	public static int worldCols, worldRows;
	
	// used to store coordinates of all obstacles in our world
	public static List<int[]> obstacles = new ArrayList<int[]>();
	
	// used to store all unique states that agent has visited (holds agent and sample coords)
	public static List<State> closed = new ArrayList<State>();
	
	// used to keep track of number of operations and is used in output
	public static int expansions = 0;
	public static int nodesGenerated = 0;

	// helper variables just to make indexing arrays easier to read
	public static int row = 0;
	public static int col = 1;
	
    public static void main(String[] args)
    {

    	//////////////////////////////
    	// COLLECT BASIC WORLD INFO //
    	//////////////////////////////
    	Scanner input = new Scanner(System.in);
    	input.useDelimiter(System.lineSeparator());

		worldCols = Integer.parseInt(input.next());
		worldRows = Integer.parseInt(input.next());
		
		/////////////////////////////
		// CREATE WORLD STRUCTURES // 
		/////////////////////////////
		char[][] world = new char[worldRows][worldCols]; 
		List<int[]> samples = new ArrayList<int[]>();
		int[] agent = new int[] {-1, -1}; 
		
		////////////////////////////////////
		// SAVE INITIAL STATE AS 2D ARRAY //
		////////////////////////////////////
		for(int rows = 0; rows < worldRows; rows++)
		{
			// grab one row at a time
			char[] line = input.next().toCharArray();			
			
			for(int cols = 0; cols < worldCols; cols++)
			{			
				// check each column for special characters
				if(line[cols] == '@')
				{
					agent = new int[]{rows, cols};
				}
				if(line[cols] == '#')
				{
					obstacles.add(new int[]{rows, cols});
				}
				if(line[cols] == '*')
				{
					samples.add(new int[]{rows, cols});
				}
				
				// then add character to our local world
				world[rows][cols] = line[cols];	
			}
		}
		
		input.close();
				
		//////////////////////////////////////////////////
		// CONSTRUCT INTIAL STATE NODE AND BEGIN SEARCH //
		//////////////////////////////////////////////////
		
		// initial state has no parent and no previous action. '0' is being interpreted as null here
		Node initialState = new Node(null, agent, samples, '0', 0, 0);
		Stack<Node> result = new Stack<Node>();
		
		// select search algorithm to use
        if(args[0].equals("dfs"))
        	result = dfs(initialState, -1);
        	
        if(args[0].equals("ucs"))
        	result = ucs(initialState);
        
        if(args[0].equals("ids"))
        	result = ids(initialState);
        
        if(args[0].equals("astar"))
        	result = astar(initialState, args[1]);
        
        int length = result.size();

        // decide whether to animate
        if(args.length == 2)
        {
            if(args[1].equals("animate"))
        	{
        		try 
        		{
					animate(world, result);
				} 
        		catch (InterruptedException e) 
        		{
					e.printStackTrace();
				}
                System.out.println("Distance traveled: " + length + " steps");
                System.out.println("Nodes Expanded: " + SampleWorld.expansions);        
                System.out.println("Total Nodes Generated: " + SampleWorld.nodesGenerated);
        		return;
        	}		
        }
        else if(args.length == 3)
        {
        	if(args[2].equals("animate"))
        	{
        		try {
        			animate(world, result);
        		} catch (InterruptedException e) {
        			e.printStackTrace();
        		}
                System.out.println("Distance traveled: " + length + " steps");
        		System.out.println("Nodes Expanded: " + SampleWorld.expansions);        
        		System.out.println("Total Nodes Generated: " + SampleWorld.nodesGenerated);
        		return;
        	}
        }
        
        // Print the results and searching stats
        if(length == 0)
        {
        	System.out.println("No solution found :(");
        }
        else
        {        	
        	for(int i = 0; i < length; i++)
        	{
        		Node state = result.pop();
        		System.out.println(state.lastMove);
        	}	
        }
        
        System.out.println("\nDistance traveled: " + length + " steps");
        System.out.println("Nodes Expanded: " + SampleWorld.expansions);        
        System.out.println("Total Nodes Generated: " + SampleWorld.nodesGenerated);
    }
    
    // helper function for me. is fun to watch and also is genuinely useful for debugging
    public static void animate(char[][] world, Stack<Node> result) throws InterruptedException
    {
    	clearScreen();
    	int agentR, agentC;

    	// print out initial state
    	for(int r = 0; r < worldRows; r++)
    	{
    		for(int c = 0; c < worldCols; c++)
    		{
    			System.out.print(world[r][c]);
    		}
    		// only make newline after row if there is another row
    		if(r != worldCols - 1)
    			System.out.println();
    	}
    	TimeUnit.MILLISECONDS.sleep(2000);
    	clearScreen();

    	// read and print each of the agent's moves
    	while(!result.isEmpty())
    	{
	    	Node state = result.pop();
	    	agentR = state.agent[row];
	    	agentC = state.agent[col];
	    	
	    	world[agentR][agentC] = '@';
	    	

	    	// does inverse of each move to replace agent's old location with _
	    	if(state.lastMove == 'U')
	    	{
	    		world[agentR + 1][agentC] = '_';
	    	}
	    	if(state.lastMove == 'D')
	    	{
	    		world[agentR - 1][agentC] = '_';
	    	}
	    	if(state.lastMove == 'L')
	    	{
	    		world[agentR][agentC + 1] = '_';
	    	}	    	
	    	if(state.lastMove == 'R')
	    	{
	    		world[agentR][agentC - 1] = '_';
	    	}
	    	if(state.lastMove == 'S')
	    	{
	    		world[agentR][agentC] = 'S';
	    	}

	    	// TODO try a new loop that just erases the line the agent was/is on 
	    	
	    	// print out new world
	    	for(int r = 0; r < worldRows; r++)
	    	{
	    		for(int c = 0; c < worldCols; c++)
	    		{
	    			System.out.print(world[r][c]);
	    		}
	    		System.out.println();
	    	}
	    	
	    	TimeUnit.SECONDS.sleep(1);
	    	clearScreen();
    	}
    	
    }
    // helper function for animate()
    public static void clearScreen()
    {
    	System.out.println("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");    		
    }

    ///////////////////////////////////
    // SUITE OF SEARCHING ALGORITHMS //
    ///////////////////////////////////
    public static Stack<Node> astar(Node initialState, String heuristic)
    {
    	Stack<Node> solution = new Stack<Node>();
    	Queue<Node> open = new PriorityQueue<Node>();
    	
		open.add(initialState);
    	while(true)
    	{
    		if(open.isEmpty())
    			return solution;
    		
    		Node currentNode = open.remove();
    		
    		// check whether we are in goal state
    		if(currentNode.samples.size() == 0)
    		{
    			while(currentNode.parent != null)
				{
					solution.push(currentNode);
					currentNode = currentNode.parent;
				}
				return solution;
    		}

    		// if not, expand children and continue
			List<Node> children = currentNode.expand(-1);	

			if(heuristic.equals("h0"))
				h0(children);
			else if(heuristic.equals("h1"))
				h1(children);
			else if(heuristic.equals("h2"))
				h2(children);
			
			for(Node child : children)
			{
				open.add(child);
			}		
    	}
    }
    
    public static Stack<Node> ids(Node initialState)
    {
    	Stack<Node> result = new Stack<Node>();
    	int depth = 1;
    	
    	while(true)
    	{
    		result = dfs(initialState, depth);
    		if(result.size() != 0)
    			return result;
			depth++;
			SampleWorld.closed.clear();
    	}    	
    }
    
    public static Stack<Node> ucs(Node initialState)
    {
    	// used to store nodes not visited (generated but not expanded)
    	Queue<Node> open = new LinkedList<Node>();
		open.add(initialState);
		
		Stack<Node> solution = new Stack<Node>();
		while(true)
		{
			if(open.isEmpty())
				return solution;
			
			Node currentNode = open.remove();

			// check if agent is in goal state
			if(currentNode.samples.size() == 0)
			{
				while(currentNode.parent != null)
				{
					solution.push(currentNode);
					currentNode = currentNode.parent;
				}
				return solution;
			}
			
			// otherwise, expand and continue down path
			List<Node> children = currentNode.expand(-1);	
			for(Node child : children)
			{
				open.add(child);
			}
		}
    }
    
    public static Stack<Node> dfs(Node initialState, int depth)
    {	    	
    	// used to store nodes not visited (generated but not expanded)
    	Stack<Node> open = new Stack<Node>();
    	open.push(initialState); 
    	
    	Stack<Node> solution = new Stack<Node>();
    	
    	while(true)
    	{
    		// if open is empty, we have exhausted all of our options and there is no solution
    		if(open.empty())
    			return solution;
    		
    		Node currentNode = open.pop();
    		
    		// check if agent is in goal state
    		if(currentNode.samples.size() == 0)
    		{
    			while(currentNode.parent != null)
    			{
    				solution.push(currentNode);
    				currentNode = currentNode.parent;
    			}
    			return solution;
    		}
    		
    		// otherwise, expand and continue down path
			List<Node> children = currentNode.expand(depth);	
			for(Node child : children)
			{
				open.push(child);
			}		
    	}
    }


    //////////////////////////////////
    // SUITE OF HEURISTIC FUNCTIONS //
    //////////////////////////////////
    
    public static List<Node> h0(List<Node> children)
    {
    	Queue<Node> sorted = new PriorityQueue<>(new NodeComparator());
    	
    	for(int i = 0; i < children.size(); i++)
    	{
    		// estimated moves left: zero
    		children.get(i).heuristic = 0;
			sorted.add(children.get(i));
    	}
    	
		return children;    	
    }
    
    public static List<Node> h1(List<Node> children)
    {
    	Queue<Node> sorted = new PriorityQueue<>(new NodeComparator());
    	
    	for(int i = 0; i < children.size(); i++)
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

    	if(children.size() == 0)
    		return sorted;
    	
    	Node child;
    	  	
    	for(int i = 0; i < children.size(); i++)
    	{	
    		// estimated moves left: 
    		// distance to nearest sample + 1 (+1 to account for sample action)
    		child = children.get(i);
    		child.heuristic = Node.distance(child.agent, child.nearestSample()) + 1;
    					
			sorted.add(child);
    	}

    	return sorted;
    }
}
// end class A1




// Node is used to store all of the current state's details
class Node implements Comparable<Node>
{
	// helper variables just to make indexing arrays easier to read
	public static int row = 0;
	public static int col = 1;
	
	Node parent;
	int[] agent;
	List<int[]> samples;
	char lastMove; // for output/animation
	int distanceTraveled; // for calculating f(n) value
	double heuristic;
	double fn; // distance traveled + heuristic value
	boolean canSample; // for expansion 

	// construct node :)
	public Node(Node parent, int[] agent, List<int[]> samples, char lastMove, int distanceTraveled, double heuristic)
	{
		this.parent = parent;
		this.agent = agent;
		this.samples = new ArrayList<int[]>(samples);
		this.lastMove = lastMove;
		this.distanceTraveled = distanceTraveled;
		this.heuristic = heuristic;
		this.fn = distanceTraveled + heuristic;
	}
	
	// determine valid moves and collect children to be sent back to search
	public List<Node> expand(Integer depth)
	{
		List<Node> children = new ArrayList<Node>();
		if(this.distanceTraveled >= depth && depth != -1)
			return children;
		
		// document expansion of node and get ready to collect this node's children
		SampleWorld.expansions++;
		int[] onSample = new int[2];
	
		// since this state is being currently visited (expanded), put in closed list
		SampleWorld.closed.add(this.getState());
		
		////////////////////////////////////
		// BEGIN CHECKING FOR VALID MOVES //
		////////////////////////////////////
		
		// store coordinates for all potential moves to be checked
		int[] up = 		{ this.agent[row] - 1, this.agent[col] };
		int[] down = 	{ this.agent[row] + 1, this.agent[col] };
		int[] left = 	{ this.agent[row], this.agent[col] - 1 };
		int[] right =  	{ this.agent[row], this.agent[col] + 1 };
		
		// make sure going up doesn't go outside world-bounds or into obstacle
		if(isOpen(up))
		{
			// if move is valid, create that new node/state and document
			Node child = new Node(this, up, this.samples, 'U', this.distanceTraveled+1, this.heuristic);
			SampleWorld.nodesGenerated++;
			
			// make sure that we have not already made that move
			if(!child.getState().inClosed())
				children.add(child);
		}
		
		// same idea but for the different potential moves
		if(isOpen(down))
		{
			Node child = new Node(this, down, this.samples, 'D', this.distanceTraveled+1, this.heuristic);
			SampleWorld.nodesGenerated++;
			
			if(!child.getState().inClosed())
				children.add(child);
		}	
		
		if(isOpen(left))
		{
			Node child = new Node(this, left, this.samples, 'L', this.distanceTraveled+1, this.heuristic);
			SampleWorld.nodesGenerated++;

			if(!child.getState().inClosed())
				children.add(child);
		}

		if(isOpen(right))
		{
			Node child = new Node(this, right, this.samples, 'R', this.distanceTraveled+1, this.heuristic);
			SampleWorld.nodesGenerated++;
			
			if(!child.getState().inClosed())
				children.add(child);
		}			
	
		// CHECK IF CAN SAMPLE
		onSample = canSample();
		if(onSample[0] == 1)
		{			
			Node child = new Node(this, this.agent, this.samples, 'S', this.distanceTraveled+1, this.heuristic);
			child.samples.remove(onSample[1]);					

			SampleWorld.nodesGenerated++;
			children.add(child);
		}
		
		return children;
	}
	
	// helper for expand, verifies whether potential move is legal
	public boolean isOpen(int[] position)
	{
		// check that agent is not trying to move into an obstacle
		for(int i = 0; i < SampleWorld.obstacles.size(); i++)
		{
			if(Arrays.compare(SampleWorld.obstacles.get(i), position) == 0)
				return false;
		}	

		// check that agent is not stepping out of the world
		if(!((position[row] >= 0) && (position[row] <= SampleWorld.worldRows - 1) && (position[col] >= 0) && (position[col] <= SampleWorld.worldCols - 1)))
			return false;
		
		return true;
	}
	
	// returns the coordinates of the sample closest to the agent's current location
	public int[] nearestSample()
	{
		if(samples.size() == 0)
			return agent;
		
		int[] s = samples.get(0);
		double lowest = distance(agent, samples.get(0));
		double distance;
		
		for(int i = 1; i < samples.size(); i++)
		{
			distance = distance(agent, samples.get(i));
			if(distance < lowest)
			{
				lowest = distance;
				s = samples.get(i);
			}
		}
		return s;
	}
	// helper function for nearestSample()
	public static double distance(int[] a, int[] b)
	{	//								 _________________________
		// distance between 2D points = √(y2 - y1)^2 + (x2 - x1)^2
		double total = (((b[row] - a[row])*(b[row] - a[row])) + ((b[col] - a[col])*(b[col] - a[col])));
		total = Math.sqrt(total);
		
		return total;
	}
	
	// helper function for h2
	/////////////////////////////
	// returns two pieces of information if true: 
	// [0] is 1 to indicate agent can sample
	// [1] is the index of the valid sample in the list
	public int[] canSample()
	{
		// default to false
		int[] result = new int[2];
		result[0] = 0;
		result[1] = -1;
		
		for(int i = 0; i < this.samples.size(); i++)
		{		
			if((this.agent[row] == samples.get(i)[row]) && (this.agent[col] == samples.get(i)[col]))
			{
				result[0] = 1;
				result[1] = i;
				return result;			
			}
		}		
		return result;
	}
	
	// returns only the dynamic information directly related to the Node's state.
	// we need a way to extract just this information for the sake of our closed list
	public State getState() 
	{
		return new State(this.agent, this.samples);
	}
	
	// we use to override comparisons for priorityQueues so that our heuristic calculations are actually used
	@Override
	public int compareTo(Node other)
	{
		if(this.fn > other.fn)
			return 1;
		else
			return -1;
	}
	
}
// helper class for Node
class NodeComparator implements Comparator<Node>
{
	// used to sort nodes based on sum of distance traveled + heuristic estimation of work left
	@Override
	public int compare(Node a, Node b)
	{
		if(a.fn > b.fn)
			return 1;
		else
			return -1;
	}
}
// helper class for Node
class State
{
	int[] agent;
	int[][] samples;
	
	public State(int[] agent, List<int[]> samples)
	{
		this.agent = agent;
		this.samples = new int[samples.size()][];
		
		// convert List<int[]> into 2D array of ints [][]
		for(int i = 0; i < samples.size(); i++)
		{
			this.samples[i] = samples.get(i).clone();
		}
	}
	
	public boolean inClosed() 
	{
		for(int i = 0; i < SampleWorld.closed.size(); i++) 
		{
			if(this.equals(SampleWorld.closed.get(i))) 
			{
				return true;
			}
		}
		return false;
	}
	
	public boolean equals(State other)
	{
		if(this.samples.length != other.samples.length)
			return false;
		
		if(Arrays.compare(this.agent, other.agent) != 0)
			return false; 
			
		for(int i = 0; i < this.samples.length; i++)
		{
			if(Arrays.compare(this.samples[i], other.samples[i]) != 0)
				return false;
		}
		
		return true;
	}
}
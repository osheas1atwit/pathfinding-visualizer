using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	class Node : IComparable<Node>
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
		bool canSample; // for expansion 

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
	public List<Node> expand(int depth)
	{
		List<Node> children = new ArrayList<Node>();
		if (this.distanceTraveled >= depth && depth != -1)
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
		int[] up = { this.agent[row] - 1, this.agent[col] };
		int[] down = { this.agent[row] + 1, this.agent[col] };
		int[] left = { this.agent[row], this.agent[col] - 1 };
		int[] right = { this.agent[row], this.agent[col] + 1 };

		// make sure going up doesn't go outside world-bounds or into obstacle
		if (isOpen(up))
		{
			// if move is valid, create that new node/state and document
			Node child = new Node(this, up, this.samples, 'U', this.distanceTraveled + 1, this.heuristic);
			SampleWorld.nodesGenerated++;

			// make sure that we have not already made that move
			if (!child.getState().inClosed())
				children.add(child);
		}

		// same idea but for the different potential moves
		if (isOpen(down))
		{
			Node child = new Node(this, down, this.samples, 'D', this.distanceTraveled + 1, this.heuristic);
			SampleWorld.nodesGenerated++;

			if (!child.getState().inClosed())
			children.add(child);
		}

		if (isOpen(left))
		{
			Node child = new Node(this, left, this.samples, 'L', this.distanceTraveled + 1, this.heuristic);
			SampleWorld.nodesGenerated++;

			if (!child.getState().inClosed())
			children.add(child);
		}

		if (isOpen(right))
		{
			Node child = new Node(this, right, this.samples, 'R', this.distanceTraveled + 1, this.heuristic);
			SampleWorld.nodesGenerated++;

			if (!child.getState().inClosed())
			children.add(child);
		}

		// CHECK IF CAN SAMPLE
		onSample = canSample();
		if (onSample[0] == 1)
		{
			Node child = new Node(this, this.agent, this.samples, 'S', this.distanceTraveled + 1, this.heuristic);
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
	for (int i = 0; i < SampleWorld.obstacles.size(); i++)
	{
	if (Arrays.compare(SampleWorld.obstacles.get(i), position) == 0)
	return false;
	}

	// check that agent is not stepping out of the world
	if (!((position[row] >= 0) && (position[row] <= SampleWorld.worldRows - 1) && (position[col] >= 0) && (position[col] <= SampleWorld.worldCols - 1)))
	return false;

	return true;
	}

	// returns the coordinates of the sample closest to the agent's current location
	public int[] nearestSample()
	{
		if (samples.size() == 0)
			return agent;

		int[] s = samples.get(0);
		double lowest = distance(agent, samples.get(0));
		double distance;

		for (int i = 1; i < samples.size(); i++)
		{
			distance = distance(agent, samples.get(i));
			if (distance < lowest)
			{
				lowest = distance;
				s = samples.get(i);
			}
		}
		return s;
	}
	// helper function for nearestSample()
	public static double distance(int[] a, int[] b)
	{   //								 _________________________
		// distance between 2D points = √(y2 - y1)^2 + (x2 - x1)^2
		double total = (((b[row] - a[row]) * (b[row] - a[row])) + ((b[col] - a[col]) * (b[col] - a[col])));
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

		for (int i = 0; i < this.samples.size(); i++)
		{
			if ((this.agent[row] == samples.get(i)[row]) && (this.agent[col] == samples.get(i)[col]))
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
			if (this.fn > other.fn)
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
		if (a.fn > b.fn)
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
			for (int i = 0; i < samples.size(); i++)
			{
				this.samples[i] = samples.get(i).clone();
			}
		}

		public boolean inClosed()
		{
			for (int i = 0; i < SampleWorld.closed.size(); i++)
			{
				if (this.equals(SampleWorld.closed.get(i)))
				{
					return true;
				}
			}
			return false;
		}

		public boolean equals(State other)
		{
			if (this.samples.length != other.samples.length)
				return false;

			if (Arrays.compare(this.agent, other.agent) != 0)
				return false;

			for (int i = 0; i < this.samples.length; i++)
			{
				if (Arrays.compare(this.samples[i], other.samples[i]) != 0)
					return false;
			}

			return true;
		}
	}

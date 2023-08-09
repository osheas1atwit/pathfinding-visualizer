using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

	public class Node //: IComparable<Node>
	{
		// helper variables just to make indexing arrays easier to read
		public static int row = 0;
		public static int col = 1;

		public Node parent;
		public Vector2Int agent;
		public List<Vector2Int> samples;
		public char lastMove; // for output/animation
		public int distanceTraveled; // for calculating f(n) value
		public double heuristic;
		public double fn; // distance traveled + heuristic value
		//bool canSample; // for expansion 

		// construct node :)
		public Node(Node parent, Vector2Int agent, List<Vector2Int> samples, char lastMove, int distanceTraveled, double heuristic)
		{
			this.parent = parent;
			this.agent = agent;
			this.samples = samples;
			this.lastMove = lastMove;
			this.distanceTraveled = distanceTraveled;
			this.heuristic = heuristic;
			fn = distanceTraveled + heuristic;
		}

		// determine valid moves and collect children to be sent back to search
		public List<Node> expand(int depth)
		{
			List<Node> children = new List<Node>();
			if (this.distanceTraveled >= depth && depth != -1)
				return children;

			// document expansion of node and get ready to collect this node's children
			// SampleWorld.expansions++;
			Vector2Int onSample = new Vector2Int();

			// since this state is being currently visited (expanded), put in closed list
			Pathfinder.closed.Add(this.getState());

			////////////////////////////////////
			// BEGIN CHECKING FOR VALID MOVES //
			////////////////////////////////////

			// store coordinates for all potential moves to be checked
			Vector2Int up    = new Vector2Int( this.agent.y + 1, this.agent.x );
			Vector2Int down  = new Vector2Int( this.agent.y - 1, this.agent.x );
			Vector2Int left  = new Vector2Int( this.agent.y, this.agent.x - 1 );
			Vector2Int right = new Vector2Int( this.agent.y, this.agent.x + 1 );

			// make sure going up doesn't go outside world-bounds or into obstacle
			if (isOpen(up))
			{
				// if move is valid, create that new node/state and document
				Node child = new Node(this, up, this.samples, 'U', this.distanceTraveled + 1, this.heuristic);
				//SampleWorld.nodesGenerated++;

				// make sure that we have not already made that move
				if (!child.getState().inClosed())
					children.Add(child);
			}

			// same idea but for the different potential moves
			if (isOpen(down))
			{
				Node child = new Node(this, down, this.samples, 'D', this.distanceTraveled + 1, this.heuristic);
				//SampleWorld.nodesGenerated++;

				if (!child.getState().inClosed())
					children.Add(child);
			}

			if (isOpen(left))
			{
				Node child = new Node(this, left, this.samples, 'L', this.distanceTraveled + 1, this.heuristic);
				//SampleWorld.nodesGenerated++;

				if (!child.getState().inClosed())
					children.Add(child);
			}

			if (isOpen(right))
			{
				Node child = new Node(this, right, this.samples, 'R', this.distanceTraveled + 1, this.heuristic);
				//SampleWorld.nodesGenerated++;

				if (!child.getState().inClosed())
				children.Add(child);
			}

			// CHECK IF CAN SAMPLE
			onSample = CanSample();
			if (onSample.x == 1)
			{
				Debug.Log("SAMPLING");
				Node child = new Node(this, this.agent, this.samples, 'S', this.distanceTraveled + 1, this.heuristic);
			
				child.samples.RemoveAt(onSample.y);

				//SampleWorld.nodesGenerated++;
				children.Add(child);
			}

			return children;
		}

		// helper for expand, verifies whether potential move is legal
		public bool isOpen(Vector2Int position)
		{
			// check that agent is not trying to move into an obstacle
			for (int i = 0; i < Pathfinder.obstacles.Count(); i++)
			{
				if (Pathfinder.obstacles[i].x.Equals(position.x) && Pathfinder.obstacles[i].y.Equals(position.y))
					return false;
			}

			if ((position.y < 0) || (position.y > Pathfinder.height - 1) || (position.x < 0) || (position.x > Pathfinder.width - 1))
				return false;

			// check that agent is not stepping out of the world
			// if (!((position.y >= 0) && (position.y <= Pathfinder.height - 1) && (position.x >= 0) && (position.x <= Pathfinder.width - 1)))
			//	return false;

			return true;
		}

		// returns the coordinates of the sample closest to the agent's current location
		public Vector2Int nearestSample()
		{
			if (samples.Count == 0)
				return agent;

			Vector2Int s = samples[0];
			double lowest = distance(agent, samples[0]);
			double dist;

			for (int i = 1; i < samples.Count; i++)
			{
				dist = distance(agent, samples[i]);
				if (dist < lowest)
				{
					lowest = dist;
					s = samples[i];
				}
			}
			return s;
		}
		// helper function for nearestSample()
		public static double distance(Vector2Int a, Vector2Int b)
		{   //								 _________________________
			// distance between 2D points = √(y2 - y1)^2 + (x2 - x1)^2
			double total = (((b.y - a.y) * (b.y - a.y)) + ((b.x - a.x) * (b.x - a.x)));
			total = Math.Sqrt(total);

			return total;
		}

		// helper function for h2
		/////////////////////////////
		// returns two pieces of information if true: 
		// [0] is 1 to indicate agent can sample
		// [1] is the index of the valid sample in the list
		public Vector2Int CanSample()
		{
			// default to false
			Vector2Int result = new Vector2Int();
			result.x = 0;
			result.y = -1;

			for (int i = 0; i < this.samples.Count; i++)
			{
				if ((this.agent.y == samples[i].y) && (this.agent.x == samples[i].x))
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
/*		public int CompareTo(Node other)
		{
			if (this.fn > other.fn)
				return 1;
			else
				return -1;
		}*/
	
	}

	// helper class for Node
/*	class NodeComparator : Comparer<Node>
	{
		// used to sort nodes based on sum of distance traveled + heuristic estimation of work left
		public override int Compare(Node a, Node b)
		{
			if (a.fn > b.fn)
			return 1;
			else
			return -1;
		}
	}*/
	// helper class for Node
	public class State
	{
		Vector2Int agent;
		List<Vector2Int> samples;

		public State(Vector2Int agent, List<Vector2Int> samples)
		{
			this.agent = agent;
			this.samples = samples;

			// convert List<int[]> into 2D array of ints [][]
			/*for (int i = 0; i < samples.Count; i++)
			{
				this.samples[i] = samples[i];
			}*/
		}

		public bool inClosed()
		{
			for (int i = 0; i < Pathfinder.closed.Count(); i++)
			{
				if (this.equals(Pathfinder.closed[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool equals(State other)
		{
			if (this.samples.Count != other.samples.Count)
				return false;

			if (!(this.agent.x.Equals(other.agent.x) && this.agent.y.Equals(other.agent.y)))
				return false;

			for (int i = 0; i < this.samples.Count; i++)
			{
				//if (Arrays.compare(this.samples[i], other.samples[i]) != 0)
				
				if (this.samples.SequenceEqual(other.samples))
					return false;
			}

			return true;
		}
	}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class NodeGrid : IEnumerable<Node> {

	public int width;
	public int height;

	private Node[,] _grid;

	private bool[,] _flagged;

	public NodeGrid(int width, int height)
	{
		this.width = width;
		this.height = height;

		_grid = new Node[height, width];
		_flagged = new bool[height, width];

		for (int row = 0; row < height; row++)
		{
			for (int column = 0; column < width; column++)
			{
				_grid[row, column] = new Node(column:column, row:row);
            }
		}
	}

	public void FloodFill(int column, int row, int minimum, int maximum, float chance, NodeManipulation manipulationMethod)
	{
		if (_grid[row, column] == null)
			return;

		Direction[] directions = allDirections;
		Queue<Node> openList = new Queue<Node>();
		int used = 0;

		ClearFlags();

		Node startNode = this[row, column];

		if (startNode == null)
			return;

		openList.Enqueue(startNode);
		_flagged[row, column] = true;

		while (openList.Count > 0 && used < maximum)
		{
			Node current = openList.Dequeue();
			manipulationMethod(current);
			used++;

			// check chance => do not continue
			if (used >= minimum)
			{
				float dice = UnityEngine.Random.Range(0, 1.0f);
				if (dice <= chance)
					break;
			}

			for (int i = 0; i < directions.Length; i++)
			{
				Direction direction = directions[i];

				Node otherNode = GetNodeInDirection(current, direction);

				if (otherNode == null)
					continue;

				if (_flagged[otherNode.row, otherNode.column])
					continue;

				openList.Enqueue(otherNode);
				_flagged[otherNode.row, otherNode.column] = true;
			}
		}
	}

	public void RemoveUnconnectedNodes()
	{
		ClearFlags();

		List<NodeList> islands = new List<NodeList>();

		foreach (var item in this)
		{
			if (!IsFlagged(item))
			{
				islands.Add(GetConnectedNodes(item, HasConnection));
			}
		}

		islands = islands.OrderBy(x => x.Count).Reverse().ToList();

		for (int i = 1; i < islands.Count; i++)
		{
			Delete(islands[i]);
		}
	}

	public bool HasConnection(Node fromNode, Node toNode)
	{
		return fromNode.HasConnection(toNode);
	}

	public NodeList GetConnectedNodes(Node startNode, ExamineNodeConnection examineConnection)
	{
		NodeList gathered = new NodeList();

		if (startNode == null)
			return gathered;

		Queue<Node> openList = new Queue<Node>();
		openList.Enqueue(startNode);
		MarkAsFlagged(startNode);

		while(openList.Count > 0)
		{
			Node current = openList.Dequeue();
			gathered.Add(current);

			foreach (var item in current.connections)
			{
				Node other = item.Value;

				if (!IsFlagged(other))
				{
					openList.Enqueue(other);
					MarkAsFlagged(other);
				}
			}
		}

		return gathered;
	}

	public void Delete(NodeList group)
	{
		foreach (var item in group)
		{
			Delete(item);
		}
	}

	public void Delete(Node node)
	{
		if (node == null)
			return;

		_grid[node.row, node.column] = null;
	}

	public bool IsValid(int column, int row)
	{
		return column >= 0 && column < width && row >= 0 && row < height;
	}

	public Node this[int row, int column]
	{
		get
		{
			if (column < 0 || column >= width || row < 0 || row >= height)
				return null;

			return _grid[row, column];
		}
	}

	// ================================================================================
	//  boolean flags for traversing the grid and remembering nodes
	// --------------------------------------------------------------------------------

	public void ClearFlags()
	{
		for (int row = 0; row < height; row++)
		{
			for (int column = 0; column < width; column++)
			{
				_flagged[row, column] = false;
			}
		}
	}

	private void MarkAsFlagged(Node node)
	{
		_flagged[node.row, node.column] = true;
	}

	private bool IsFlagged(Node node)
	{
		return _flagged[node.row, node.column];
	}

	// ================================================================================
	//  helper methods
	// --------------------------------------------------------------------------------

	public Node GetNodeInDirection(Node fromNode, Direction direction)
	{
		if (fromNode == null)
			return null;

		return this[fromNode.row + direction.RowOffset(), fromNode.column + direction.ColumnOffset()];
	}

	public Direction[] allDirections
	{
		get
		{
			return (Direction[]) Enum.GetValues(typeof(Direction));
        }
	}

	// ================================================================================
	//  iterate over all nodes
	// --------------------------------------------------------------------------------

	public IEnumerator<Node> GetEnumerator()
	{
		for (int row = 0; row < height; row++)
		{
			for (int column = 0; column < width; column++)
			{
				if (_grid[row, column] != null)
					yield return _grid[row, column];
            }
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void NodeManipulation(Node node);

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
				_grid[row, column] = new Node(row, column);
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
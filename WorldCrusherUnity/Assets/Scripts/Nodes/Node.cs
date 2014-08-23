using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public int row, column;

	public float x, y;

	private Dictionary<Direction, Node> connections = new Dictionary<Direction, Node>();

	public Node(int column, int row)
	{
		this.row = row;
		this.column = column;
	}

	public Vector3 position
	{
		get
		{
			return new Vector3(x, y, 0);
		}
	}

	public void Connect(Node other, Direction direction)
	{
		if (HasConnection(other))
		{
			Debug.LogError("Node already connected");
			return;
		}

		connections[direction] = other;
	}

	public bool HasConnection(Direction direction)
	{
		return connections.ContainsKey(direction);
	}

	public bool HasConnection(Node node)
	{
		return connections.ContainsValue(node);
	}

	public override string ToString()
	{
		return string.Format("Node {0}:{1}", column, row);
	}

}
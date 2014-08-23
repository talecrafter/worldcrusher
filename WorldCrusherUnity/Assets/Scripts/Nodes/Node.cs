using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {

	public int row, column;

	public float x, y;

	private Dictionary<Direction, Node> _connections = new Dictionary<Direction, Node>();
	public Dictionary<Direction, Node> connections
	{
		get
		{
			return _connections;
		}
	}

	// ================================================================================
	//  position helper
	// --------------------------------------------------------------------------------

	public Vector3 position
	{
		get
		{
			return new Vector3(x, y, 0);
		}
	}

	public Vector3 positionFront
	{
		get
		{
			return new Vector3(x, y, -0.2f);
		}
	}

	public Vector3 positionBehind
	{
		get
		{
			return new Vector3(x, y, 0.2f);
		}
	}

	// ================================================================================
	//  contructor
	// --------------------------------------------------------------------------------

	public Node(int column, int row)
	{
		this.row = row;
		this.column = column;
	}

	// ================================================================================
	//  public methods
	// --------------------------------------------------------------------------------

	public void Connect(Node other, Direction direction)
	{
		if (other == null)
			return;

		if (HasConnection(other) || HasConnection(direction))
		{
			return;
		}

		if (other.HasConnection(this) || other.HasConnection(direction.Reversed()))
        {
			return;
		}

		_connections[direction] = other;
		other.AttachIncomingConnection(this, direction.Reversed());
	}

	public void AttachIncomingConnection(Node other, Direction direction)
	{
		_connections[direction] = other;
	}

	public bool HasConnection(Direction direction)
	{
		return _connections.ContainsKey(direction);
	}

	public bool HasConnection(Node node)
	{
		return _connections.ContainsValue(node);
	}

	public override string ToString()
	{
		return string.Format("Node {0}:{1}", column, row);
	}

}
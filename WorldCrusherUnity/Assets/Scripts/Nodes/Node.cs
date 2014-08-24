using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// ================================================================================
//  Node Delegates
// --------------------------------------------------------------------------------

public delegate void NodeManipulation(Node node);
public delegate bool ExamineNodeConnection(Node fromNode, Node toNode);

// ================================================================================
//  class Node
// --------------------------------------------------------------------------------

public class Node {

	public int row, column; // position in grid

	public float x, y; // world position

	public FactionType faction = FactionType.Enemy;

	private Dictionary<Direction, Node> _connections = new Dictionary<Direction, Node>();
	public Dictionary<Direction, Node> connections
	{
		get
		{
			return _connections;
		}
	}

	public NodeDisplay display;

	public bool isBorderNode
	{
		get
		{
			foreach (var item in _connections)
			{
				if (item.Value.faction != faction)
				{
					return true;
				}
			}

			return false;
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

	public void SetFaction(FactionType faction)
	{
		this.faction = faction;

		if (display != null)
			display.UpdateFaction();
	}

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

	public void Conquered()
	{
		faction = faction.Other();
		display.UpdateFaction();
		display.HideMarker();
	}

	public void Defended()
	{
		display.HideMarker();
	}

	public void DetachIncomingConnection(Node other, Direction direction)
	{
		if (_connections.ContainsKey(direction))
			_connections.Remove(direction);
	}

	public Node GetConnection(Direction direction)
	{
		return _connections[direction];
	}

	public bool HasConnection(Direction direction)
	{
		return _connections.ContainsKey(direction);
	}

	public bool HasConnection(Node node)
	{
		return _connections.ContainsValue(node);
	}

	// ================================================================================
	//  debug methods
	// --------------------------------------------------------------------------------

	public override string ToString()
	{
		return string.Format("Node {0}:{1}", column, row);
	}

	public void LogConnections()
	{
		foreach (var item in _connections)
		{
			Debug.Log(item.Value + " - " + item.Value.faction);
		}
	}
}
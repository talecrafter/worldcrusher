using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NodeGroup : IEnumerable<Node> {

	private HashSet<Node> _nodes = new HashSet<Node>();

	private HashSet<Node> _flags = new HashSet<Node>();

	public int Count { get { return _nodes.Count; } }

	public void Add(Node node)
	{
		if (!_nodes.Contains(node))
			_nodes.Add(node);
	}

	public bool HasConnection(Node fromNode, Node toNode)
	{
		return fromNode.HasConnection(toNode);
	}

	public Node PickRandom()
	{
		return new NodeList(_nodes).PickRandom();
	}

	public Node PopRandom()
	{
		Node node = new NodeList(_nodes).PickRandom();
		_nodes.Remove(node);
		return node;
	}

	public NodeList GetConnectedNodes(Node startNode, ExamineNodeConnection examineConnection = null, int max = -1)
	{
		NodeList gathered = new NodeList();

		if (startNode == null)
			return gathered;

		Queue<Node> openList = new Queue<Node>();
		openList.Enqueue(startNode);
		MarkAsFlagged(startNode);

		while (openList.Count > 0 && (max < 0 || gathered.Count <= max ))
		{
			Node current = openList.Dequeue();
			gathered.Add(current);

			foreach (var item in current.connections)
			{
				Node other = item.Value;

				if (!IsFlagged(other) && (examineConnection == null || examineConnection(current, other)))
				{
					openList.Enqueue(other);
					MarkAsFlagged(other);
				}
			}
		}

		return gathered;
	}

	// ================================================================================
	//  boolean flags for traversing and remembering nodes
	// --------------------------------------------------------------------------------

	public void ClearFlags()
	{
		_flags.Clear();
	}

	private void MarkAsFlagged(Node node)
	{
		if (!_flags.Contains(node))
			_flags.Add(node);
	}

	private bool IsFlagged(Node node)
	{
		return _flags.Contains(node);
	}

	public IEnumerator<Node> GetEnumerator()
	{
		return _nodes.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
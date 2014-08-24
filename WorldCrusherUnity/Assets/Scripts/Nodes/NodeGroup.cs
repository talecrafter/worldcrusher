using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

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

	public int GetNodeCount(FactionType type)
	{
		int count = 0;

		foreach (var node in _nodes)
		{
			if (node.faction == type)
				count++;
		}

		return count;
	}

	public NodeList GetNodesForFaction(FactionType type)
	{
		NodeList list = new NodeList();

		foreach (var node in _nodes)
		{
			if (node.faction == type)
				list.Add(node);
		}

		return list;
	}

	public Node PopRandom()
	{
		Node node = new NodeList(_nodes).PickRandom();
		_nodes.Remove(node);
		return node;
	}

	public void LooseUnconnectedWorlds()
	{
		List<NodeList> playerIslands = GetIslandsForFaction(FactionType.Player);
		List<NodeList> enemyIslands = GetIslandsForFaction(FactionType.Enemy);

		while (playerIslands.Count > 1 || enemyIslands.Count > 1)
		{
			bool removePlayerIsland = true;
			if (playerIslands.Count == 1)
			{
				removePlayerIsland = false;
            }
			else if (enemyIslands.Count == 1)
			{
				removePlayerIsland = true;
			}
			else
			{
				if (playerIslands.Last().Count > enemyIslands.Last().Count)
				{
					removePlayerIsland = false;
				}
				else if (playerIslands.Last().Count < enemyIslands.Last().Count)
				{
					removePlayerIsland = true;
				}
				else
				{
					int chance = UnityEngine.Random.Range(0, 2);
					if (chance == 0)
						removePlayerIsland = true;
					else
						removePlayerIsland = false;
				}
			}

			if (removePlayerIsland)
			{
				MarkNodesForFaction(playerIslands[playerIslands.Count - 1], FactionType.Enemy);
				playerIslands.RemoveAt(playerIslands.Count - 1);
			}
			else
			{
				MarkNodesForFaction(enemyIslands[enemyIslands.Count - 1], FactionType.Player);
				enemyIslands.RemoveAt(enemyIslands.Count - 1);
			}
		}
	}

	//public void RemoveUnconnectedWorldsForFaction(FactionType faction)
	//{
	//	List<NodeList> islands = GetIslandsForFaction(faction);

	//	//Debug.Log("Faction " + faction.ToString() + ": " + islands.Count + " islands");

 //       for (int i = 1; i < islands.Count; i++)
	//	{
	//		MarkNodesForFaction(islands[i], faction.Other());
	//	}
	//}

	public void MarkNodesForFaction(NodeList list, FactionType faction)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetFaction(faction);
        }
	}

	public List<NodeList> GetIslandsForFaction(FactionType faction)
	{
		ClearFlags();

		List<NodeList> islands = new List<NodeList>();

		foreach (var item in this)
		{
			if (!IsFlagged(item) && item.faction == faction)
			{
				islands.Add(GetConnectedNodes(item, HasSameFaction));
			}
		}

		islands = islands.OrderBy(x => x.Count).Reverse().ToList();

		return islands;		
	}

	public bool HasSameFaction(Node fromNode, Node toNode)
	{
		return fromNode.faction == toNode.faction;
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
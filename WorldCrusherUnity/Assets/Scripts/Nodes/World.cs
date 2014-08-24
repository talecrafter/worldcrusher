using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World {

	public float maxCameraX = 15.0f;
	public float maxCameraY = 15.0f;

	public NodeGroup nodes = new NodeGroup();

	private Node _home;
	public Node home
	{
		get
		{
			return _home;
		}
	}

	private int _turn = 0;

	public void Clear()
	{
		nodes = new NodeGroup();
		_turn = 0;

		Game.Instance.messenger.Message("New Game Started");
	}

	public NodeGroup GetBorderRegions(FactionType faction)
	{
		NodeGroup group = new NodeGroup();
		foreach (var item in nodes)
		{
			if (item.isBorderNode && item.faction == faction)
				group.Add(item);
		}

		return group;
	}

	public void PickStartNode()
	{
		_home = nodes.PickRandom();
	}

	public Node FindRandomPlayerNode()
	{
		return nodes.GetNodesForFaction(FactionType.Player).PickRandom();
	}

	public void NewRound()
	{
		foreach (var node in nodes)
		{
			node.display.NewRound();
		}

		_turn++;
		Game.Instance.messenger.Message("Round " + _turn);

		Game.Instance.player.NewRound();
		Game.Instance.enemy.NewRound();
	}

	public void HideMarker()
	{
		foreach (var node in nodes)
		{
			node.display.HideMarker();
		}
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NodeDisplay : MonoBehaviour {

	public Node node = null;

	public SpriteRenderer mainImage;
	public SpriteRenderer defenseImage;
	public SpriteRenderer attackImage;

	private Dictionary<Node, NodeConnection> _connections = new Dictionary<Node, NodeConnection>();

	public void SetNode(Node node)
	{
		this.node = node;
		node.display = this;

		UpdateFaction();
	}

	public void SetImage(Sprite image)
	{
		mainImage.sprite = image;
	}

	public void NewRound()
	{
		UpdateFaction();
		defenseImage.enabled = false;
		attackImage.enabled = false;
	}

	public void ShowAttack()
	{
		attackImage.enabled = true;
		attackImage.color = node.faction.Other().GetColor();
	}

	public void HideAttack()
	{
		attackImage.enabled = false;
	}

	public void ShowDefense()
	{
		defenseImage.enabled = true;
		defenseImage.color = node.faction.GetColor();
	}

	public void HideDefense()
	{
		defenseImage.enabled = false;
	}

	public void UpdateFaction()
	{
		if (node.faction == FactionType.Player)
		{
			mainImage.color = Color.white;
		}
		else
		{
			if (node.isBorderNode)
			{
				mainImage.color = new Color(0.3f, 0.3f, 0.3f);
			}
			else
			{
				mainImage.color = new Color(0.2f, 0.05f, 0.05f);
			}
		}
	}

	public bool HasConnection(Node otherNode)
	{
		return _connections.ContainsKey(otherNode);
	}

	public void AttachConnection(Node otherNode, NodeConnection connection)
	{
		_connections.Add(otherNode, connection);
	}

	public void HideMarker()
	{
		HideAttack();
		HideDefense();
	}
}
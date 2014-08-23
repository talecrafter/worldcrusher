using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeDisplay : MonoBehaviour {

	public Node node = null;

	public SpriteRenderer mainImage;

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

	public void UpdateFaction()
	{
		mainImage.color = node.faction.GetColor();
	}

	public bool HasConnection(Node otherNode)
	{
		return _connections.ContainsKey(otherNode);
	}

	public void AttachConnection(Node otherNode, NodeConnection connection)
	{
		_connections.Add(otherNode, connection);
	}

}
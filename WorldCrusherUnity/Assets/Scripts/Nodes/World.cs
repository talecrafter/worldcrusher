using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World {

	public NodeGroup nodes = new NodeGroup();

	private Node _home;
	public Node home
	{
		get
		{
			return _home;
		}
	}

	public void Clear()
	{
		nodes = new NodeGroup();
	}

	public void PickStartNode()
	{
		_home = nodes.PickRandom();
	}
}
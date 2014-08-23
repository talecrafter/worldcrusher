using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class World {

	public List<Node> nodes = new List<Node>();

	public void Clear()
	{
		nodes.Clear();
	}
}
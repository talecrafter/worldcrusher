﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeConnection : MonoBehaviour {

	public Node fromNode;
	public Node toNode;

	public override string ToString()
	{
		return "Connection from " + fromNode.ToString() + " to " + toNode.ToString();
	}

}
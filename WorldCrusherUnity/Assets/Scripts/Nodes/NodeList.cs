using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeList : List<Node> {

	public NodeList()
	{

	}

	public NodeList(HashSet<Node> hashSet)
	{
		foreach (var item in hashSet)
		{
			Add(item);
		}
	}

}
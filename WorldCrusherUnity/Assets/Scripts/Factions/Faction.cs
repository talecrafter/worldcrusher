using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Faction {

	public int actions = 3;

	public int actionsLeft
	{
		get
		{
			return actions - attacks.Count - defenses.Count;
		}
	}

	public FactionType type = FactionType.Enemy;

	public NodeList attacks = new NodeList();
	public NodeList defenses = new NodeList();

	public Faction(FactionType type)
	{
		this.type = type;
	}

	public void NewRound()
	{
		attacks.Clear();
		defenses.Clear();
	}

	public void PrepareAttack(Node node)
	{
		if (actionsLeft > 0)
		{
			attacks.Add(node);
			node.display.ShowAttack();
		}
	}

	public void RemoveAttack(Node node)
	{
		if (attacks.Contains(node))
		{
			attacks.Remove(node);
			node.display.HideAttack();
		}
	}

	public void PrepareDefense(Node node)
	{
		if (actionsLeft > 0)
		{
			defenses.Add(node);
			node.display.ShowDefense();
		}
	}

	public void RemoveDefense(Node node)
	{
		if (defenses.Contains(node))
		{
			defenses.Remove(node);
			node.display.HideDefense();
		}
	}

}
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

}
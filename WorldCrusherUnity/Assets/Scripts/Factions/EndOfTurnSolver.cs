using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndOfTurnSolver {

	public List<Attack> attacks = new List<Attack>();

	public void GatherActions()
	{
		attacks.Clear();

		GatherAttacks(Game.Instance.player, Game.Instance.enemy);
		GatherAttacks(Game.Instance.enemy, Game.Instance.player);

		attacks = attacks.Shuffle();
	}

	private void GatherAttacks(Faction attackingFaction, Faction defendingFaction)
	{
		for (int i = 0; i < attackingFaction.attacks.Count; i++)
		{
			Attack attack = new Attack() { node = attackingFaction.attacks[i] };

			if (defendingFaction.defenses.Contains(attack.node))
                attack.isDefended = true;

			attacks.Add(attack);
		}
	}
}

public class Attack
{
	public Node node;
	public bool isDefended = false;
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndOfTurnSolver {

	public List<Attack> attacks = new List<Attack>();
	public List<Attack> defended = new List<Attack>();
	public List<Node> uselessDefended = new List<Node>();

	public void GatherActions()
	{
		attacks.Clear();
		defended.Clear();
		uselessDefended.Clear();

		GatherAttacks(Game.Instance.player, Game.Instance.enemy);
		GatherAttacks(Game.Instance.enemy, Game.Instance.player);

		GatherUselessDefenses(Game.Instance.player, Game.Instance.enemy);
		GatherUselessDefenses(Game.Instance.enemy, Game.Instance.player);

		attacks = attacks.Shuffle();
	}

	public void HideMarkerAtDefendedNodes()
	{
		//Debug.Log("defended: " + defended.Count);
		for (int i = 0; i < defended.Count; i++)
		{
			defended[i].node.display.HideMarker();
		}

		//Debug.Log("useless defended: " + uselessDefended.Count);
		for (int i = 0; i < uselessDefended.Count; i++)
		{
			uselessDefended[i].display.HideMarker();
		}
	}

	private void GatherUselessDefenses(Faction attackingFaction, Faction defendingFaction)
	{
		for (int i = 0; i < defendingFaction.defenses.Count; i++)
		{
			if (!attackingFaction.attacks.Contains(defendingFaction.defenses[i]))
				uselessDefended.Add(defendingFaction.defenses[i]);
		}
	}

	private void GatherAttacks(Faction attackingFaction, Faction defendingFaction)
	{
		for (int i = 0; i < attackingFaction.attacks.Count; i++)
		{
			Attack attack = new Attack() { node = attackingFaction.attacks[i] };

			if (defendingFaction.defenses.Contains(attack.node))
                attack.isDefended = true;

			// only show successfull attacks
			if (!attack.isDefended)
				attacks.Add(attack);
			else
				defended.Add(attack);
		}
	}
}

public class Attack
{
	public Node node;
	public bool isDefended = false;
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemyController : MonoBehaviour {

	private Faction _faction;
	private World _world;



	public void PlaceActions()
	{
		_faction = Game.Instance.enemy;
		_world = Game.Instance.world;

		NodeGroup ownBorder = _world.GetBorderRegions(FactionType.Enemy);
		NodeGroup foreignBorder = _world.GetBorderRegions(FactionType.Player);

		int maxPossibleActions = ownBorder.Count + foreignBorder.Count;

		while (_faction.actionsLeft > 0 && (ownBorder.Count + foreignBorder.Count > 1))
		{
			if (ownBorder.Count == 0 && foreignBorder.Count == 0)
				break;

			if (ownBorder.Count == 0)
			{
				PlaceAttack(foreignBorder.PopRandom());
			}
			else if (foreignBorder.Count == 0)
			{
				PlaceDefense(ownBorder.PopRandom());
			}
			else
			{
				float attackChance = UnityEngine.Random.Range(0, 1.0f);
				if (attackChance > 0.3f)
				{
					PlaceAttack(foreignBorder.PopRandom());
				}
				else
				{
					PlaceDefense(ownBorder.PopRandom());
				}
			}
		}
	}

	private void PlaceDefense(Node node)
	{
		_faction.PrepareDefense(node);
	}

	private void PlaceAttack(Node node)
	{
		_faction.PrepareAttack(node);
	}
}

using UnityEngine;

public enum Faction {

    Player,
	Enemy
}

public static class FactionExtensions
{
	public static Color GetColor(this Faction faction)
	{
		switch (faction)
		{
			case Faction.Player:
				return Color.white;
			case Faction.Enemy:
				return new Color(0.3f, 0.3f, 0.3f);
			default:
				return Color.gray;
		}
	}
}
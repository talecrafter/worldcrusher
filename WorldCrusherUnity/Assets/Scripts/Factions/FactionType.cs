
using UnityEngine;

public enum FactionType {

    Player,
	Enemy
}

public static class FactionTypeExtensions
{
	public static Color GetColor(this FactionType faction)
	{
		switch (faction)
		{
			case FactionType.Player:
				return Game.Instance.interfaceManager.playerColor;
			case FactionType.Enemy:
				return Game.Instance.interfaceManager.enemyColor;

			default:
				return Color.blue;
		}
	}

	public static FactionType Other(this FactionType faction)
	{
		switch (faction)
		{
			case FactionType.Player:
				return FactionType.Enemy;
			case FactionType.Enemy:
				return FactionType.Player;

			default:
				return FactionType.Player;
		}
	}
}
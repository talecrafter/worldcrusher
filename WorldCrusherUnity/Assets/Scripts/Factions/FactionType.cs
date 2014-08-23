
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
				return Color.white;
			case FactionType.Enemy:
				return new Color(0.3f, 0.3f, 0.3f);
			default:
				return Color.gray;
		}
	}
}
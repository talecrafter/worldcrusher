
public enum Direction {

	North = 0,
	East = 1,
	South = 2,
	West = 3

}

public static class DirectionExtensions
{
	public static Direction Reversed(this Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				return Direction.South;
			case Direction.East:
				return Direction.West;
			case Direction.South:
				return Direction.North;
			case Direction.West:
				return Direction.East;
			default:
				return Direction.North;
		}
	}

	public static int ColumnOffset(this Direction direction)
	{
		switch (direction)
		{
			case Direction.East:
				return 1;
			case Direction.West:
				return -1;
			default:
				return 0;
		}
	}

	public static int RowOffset(this Direction direction)
	{
		switch (direction)
		{
			case Direction.North:
				return 1;
			case Direction.South:
				return -1;
			default:
				return 0;
		}
	}
}
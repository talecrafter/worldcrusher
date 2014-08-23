using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, INavigationInput {

	private Node _selected = null;

	private Selection _selectionFocus;

	public void Awake()
	{
		SearchSelectionFocus();
	}

	public void OnLevelWasLoaded()
	{
		SearchSelectionFocus();
	}

	public void SetSelectionToHome()
	{
		Select(Game.Instance.world.home);
	}

	public void Select(Node node)
	{
		_selected = node;
		_selectionFocus.Select(node);
	}

	private void SearchSelectionFocus()
	{
		_selectionFocus = FindObjectOfType<Selection>();
	}

	#region INavigationInput
	// ================================================================================
	//  INavigationInput
	// --------------------------------------------------------------------------------

	public void Down()
	{
		MoveDirection(Direction.South);
	}

	public void Left()
	{
		MoveDirection(Direction.West);
	}

	public void Right()
	{
		MoveDirection(Direction.East);
	}

	public void Up()
	{
		MoveDirection(Direction.North);
	}

	public void Enter()
	{
		
	}

	public void Back()
	{
		
	}

	#endregion

	private void MoveDirection(Direction direction)
	{
		if (_selected.HasConnection(direction))
		{
			Select(_selected.GetConnection(direction));
        }
	}
}
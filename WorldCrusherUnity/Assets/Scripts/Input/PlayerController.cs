using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, INavigationInput {

	private Node _selected = null;

	private Selection _selectionFocus;
	private Faction _faction;

	// ================================================================================
	//  unity methods
	// --------------------------------------------------------------------------------

	public void Awake()
	{
		SearchSelectionFocus();
	}

	public void OnLevelWasLoaded()
	{
		SearchSelectionFocus();
	}

	// ================================================================================
	//  public methods
	// --------------------------------------------------------------------------------

	public void SetSelectionToHome()
	{
		Select(Game.Instance.world.home);
	}

	public void Select(Node node)
	{
		_selected = node;
		_selectionFocus.Select(node);
	}

	public void Use()
	{
		if (_selected.faction == _faction.type)
		{
			if (_faction.defenses.Contains(_selected))
			{
				_faction.RemoveDefense(_selected);
			}
			else if (!_faction.defenses.Contains(_selected))
			{
				if (_faction.actionsLeft > 0)
				{
					_faction.PrepareDefence(_selected);
				}
				else
				{
					Game.Instance.messenger.Message("No Actions Left");
				}
			}
		}
		else
		{
			if (_faction.attacks.Contains(_selected))
			{
				_faction.RemoveAttack(_selected);
			}
			else if (!_faction.attacks.Contains(_selected))
			{
				if (_faction.actionsLeft > 0)
				{
					_faction.PrepareAttack(_selected);
				}
				else
				{
					Game.Instance.messenger.Message("No Actions Left");
				}
			}
		}
	}

	// ================================================================================
	//  private methods
	// --------------------------------------------------------------------------------

	private void SearchSelectionFocus()
	{
		_faction = Game.Instance.player;
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
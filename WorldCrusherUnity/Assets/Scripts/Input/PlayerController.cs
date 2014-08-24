using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	public void ShowSelection(bool doShow)
	{
		_selectionFocus.gameObject.SetActive(doShow);
    }

	public void SetSelectionToHome()
	{
		Select(Game.Instance.world.home);
	}

	public void Select(Node node)
	{
		Game.Instance.audioController.MovementSound();

		_selected = node;
		_selectionFocus.Select(node);
	}

	public void MoveCameraBackToNode()
	{
		Select(Game.Instance.world.FindRandomPlayerNode());
	}

	public void MoveCamera(Vector3 pos)
	{		
		_selectionFocus.transform.position = HoldInBounds(pos);
	}

	internal void TranslateCamera(Vector2 delta)
	{
		float d = Screen.height / (Camera.main.orthographicSize * 2.0f);
		float y = delta.y / d;
		float x = delta.x / d;

		Vector3 newPos = _selectionFocus.transform.position;
		newPos.x += x;
		newPos.y += y;
		MoveCamera(newPos);
	}

	private Vector3 HoldInBounds(Vector3 pos)
	{
		if (pos.x > Game.Instance.world.maxCameraX)
			pos.x = Game.Instance.world.maxCameraX;
		if (pos.x < -Game.Instance.world.maxCameraX)
			pos.x = -Game.Instance.world.maxCameraX;
		if (pos.y > Game.Instance.world.maxCameraY)
			pos.y = Game.Instance.world.maxCameraY;
		if (pos.y < -Game.Instance.world.maxCameraY)
			pos.y = -Game.Instance.world.maxCameraY;

		return pos;
	}

	public void Use()
	{
		Use(_selected);
	}

	public void Use(Node node)
	{
		Game.Instance.audioController.PlacementSound();

		if (node.faction == _faction.type)
		{
			if (_faction.defenses.Contains(node))
			{
				_faction.RemoveDefense(node);
			}
			else if (!_faction.defenses.Contains(node))
			{
				if (_faction.actionsLeft > 0)
				{
					_faction.PrepareDefense(node);
				}
				else
				{
					Game.Instance.messenger.Message("No Actions Left");
				}
			}
		}
		else
		{
			if (_faction.attacks.Contains(node))
			{
				_faction.RemoveAttack(node);
			}
			else if (!_faction.attacks.Contains(node))
			{
				if (_faction.actionsLeft == 0)
				{
					Game.Instance.messenger.Message("No Actions Left");
				}
				else if (!node.isBorderNode)
				{
					Game.Instance.messenger.Message("Not in Range");
				}
				else if (_faction.actionsLeft > 0)
				{
					_faction.PrepareAttack(node);
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputController : MonoBehaviour {

	const float treshold = 0.2f;

	private InputDirection _lastDirection = InputDirection.None;

	private INavigationInput _target = null;

	public void Awake()
	{
		_target = GetComponent<PlayerController>();
	}

	public void Update()
	{
		if (Game.Instance.IsRunning)
		{
			CheckInput();
		}
	}

	public void OnLevelWasLoaded(int levelIndex)
	{
		Reset();
	}

	private void Reset()
	{
		_lastDirection = InputDirection.None;
	}

	private void CheckInput()
	{
		CheckKeyboard();
		CheckMouse();

		if (Input.GetKeyDown(KeyCode.R))
		{
			Game.Instance.Restart();
		}
	}

	private void CheckMouse()
	{
		// look for node under cursor
		CheckMouseSelection();

		// activate focused node
		if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Use"))
		{
			Game.Instance.playerController.Use();
		}
	}

	private static void CheckMouseSelection()
	{		
		Transform hit = Utilities2D.GetHitFromPointer();

		if (hit == null)
			return;

		NodeDisplay selection = hit.GetComponent<NodeDisplay>();
		if (Input.GetMouseButtonDown(0) && selection != null)
		{
			Game.Instance.playerController.Select(selection.node);
		}
	}

	private void CheckKeyboard()
	{
		InputDirection current = GetDirectionFromInput();

		if (current != _lastDirection && _target != null)
		{
			switch (current)
			{
				case InputDirection.Up:
					_target.Up();
					break;
				case InputDirection.Down:
					_target.Down();
					break;
				case InputDirection.Left:
					_target.Left();
					break;
				case InputDirection.Right:
					_target.Right();
					break;
				case InputDirection.None:
					break;
				default:
					break;
			}
		}

		_lastDirection = current;
	}

	private InputDirection GetDirectionFromInput()
	{
		float hInput = Input.GetAxis("Horizontal");
		float vInput = Input.GetAxis("Vertical");

		if (hInput > treshold)
		{
			return InputDirection.Right;
		}
		else if (hInput < -treshold)
		{
			return InputDirection.Left;
		}
		else if (vInput > treshold)
		{
			return InputDirection.Up;
		}
		else if (vInput < -treshold)
		{
			return InputDirection.Down;
		}

		return InputDirection.None;
	}

}

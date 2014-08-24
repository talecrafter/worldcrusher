using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputController : MonoBehaviour {

	public InputType inputType = InputType.Mouse;

	const float treshold = 0.2f;
	private float minimumTouchMovement = 14.0f;
	private float minimumTime = 0.15f;

	private InputDirection _lastDirection = InputDirection.None;

	private INavigationInput _target = null;

	private NodeDisplay _nodeUnderCursor;
	private bool _isPanning = false;
	public bool isPanning
	{
		get
		{
			return _isPanning;
		}
	}

	private float _timeAtCommandStart = 0;
	private Vector2 _startMousePoint;

	// ================================================================================
	//  unity methods
	// --------------------------------------------------------------------------------

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

		if (!Application.isWebPlayer && Input.GetKeyDown(KeyCode.Escape) )
		{
			Application.Quit();
		}
	}

	public void OnLevelWasLoaded(int levelIndex)
	{
		Reset();
	}

	// ================================================================================
	//  private methods
	// --------------------------------------------------------------------------------

	private void Reset()
	{
		_lastDirection = InputDirection.None;
	}

	private void CheckInput()
	{
		if (HasMouseInput() && inputType == InputType.KeyboardOrGamepad)
		{
			inputType = InputType.Mouse;
		}
		else if (HasKeyboardInput() && inputType == InputType.Mouse)
		{
			inputType = InputType.KeyboardOrGamepad;
			Game.Instance.playerController.MoveCameraBackToNode();
        }

		switch (inputType)
		{
			case InputType.KeyboardOrGamepad:
				ProcessKeyboardInput();
				break;
			case InputType.Mouse:
				ProcessMouseInput();
				break;
			case InputType.Touch:
				break;
			default:
				break;
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			Game.Instance.Restart();
		}

		if (Input.GetButtonDown("EndTurn"))
		{
			Game.Instance.EndTurn();
		}
	}

	private bool HasMouseInput()
	{
		return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
	}

	private void ProcessMouseInput()
	{
		if (_isPanning)
		{
			UpdatePanning();
		}

		if (Game.Instance.interfaceManager.mouseOnInterface)
			return;

		// find node under cursor
		NodeDisplay nodeUnderCursor = null;
        Transform hit = Utilities2D.GetHitFromPointer();

		if (hit != null)
			nodeUnderCursor = hit.GetComponent<NodeDisplay>();

		// simple navigation

		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2)))
			Game.Instance.playerController.MoveCamera(Camera.main.ScreenToWorldPoint(Input.mousePosition));

		//// left mouse button: move camera
		//if (Input.GetMouseButtonDown(0))
		//{
		//	if (nodeUnderCursor == null)
		//		StartPanning(true);
		//	else
		//		StartCommand(nodeUnderCursor);
		//}

		//if (!_isPanning && Input.GetMouseButton(0))
		//{
		//	if (Vector2.Distance(Input.mousePosition, _startMousePoint) > minimumTouchMovement
		//		&& Time.time > _timeAtCommandStart + minimumTime)
		//	{
		//		StartPanning(false);
		//	}
		//      }

		//if (Input.GetMouseButton(0) == false)
		//{
		//	if (_isPanning)
		//	{
		//		EndPanning();
		//	}
		//	else if (_nodeUnderCursor != null)
		//	{
		//		Game.Instance.playerController.Use(_nodeUnderCursor.node);
		//		EndCommand();
		//	}
		//}

		// right mouse button: place or remove marker
		if (Input.GetMouseButtonDown(1) && nodeUnderCursor != null)
		{
			Game.Instance.playerController.Use(nodeUnderCursor.node);
		}
	}

	private void StartCommand(NodeDisplay node)
	{
		_timeAtCommandStart = Time.time;
		_nodeUnderCursor = node;
		_startMousePoint = Input.mousePosition;
	}

	private void EndCommand()
	{
		_nodeUnderCursor = null;
    }

	private void StartPanning(bool savePosition)
	{
		_startMousePoint = Input.mousePosition;
		_isPanning = true;
	}

	private void UpdatePanning()
	{
		Vector2 delta = _startMousePoint - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Game.Instance.playerController.TranslateCamera(delta);
		_startMousePoint = Input.mousePosition;
	}

	private void EndPanning()
	{
		EndCommand();
		_isPanning = false;
    }

	private bool HasKeyboardInput()
	{
		InputDirection inputDirection = GetDirectionFromInput();

		return inputDirection != InputDirection.None || Input.GetButtonDown("Use");
    }

	private void ProcessKeyboardInput()
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

		// activate focused node
		if (Input.GetButtonDown("Use"))
		{
			Game.Instance.playerController.Use();
		}
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

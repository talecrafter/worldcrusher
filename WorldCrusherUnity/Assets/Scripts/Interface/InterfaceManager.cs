using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InterfaceManager : MonoBehaviour {

	public GUISkin skin;

	public Color playerColor;
	public Color enemyColor;

	public Texture2D actionIcon;

	public float margin = 20.0f;

	public bool showHelp = true;

	[System.NonSerialized]
	public float helpTimer = 10.0f;

	private bool _mouseOnInterface = false;

	public bool mouseOnInterface
	{
		get
		{
			return _mouseOnInterface;
		}
	}

	// ================================================================================
	//  unity methods
	// --------------------------------------------------------------------------------

	public void Update()
	{
		_mouseOnInterface = false;

		if (helpTimer > 0)
		{
			helpTimer -= Time.deltaTime;
			if (helpTimer <= 0)
				showHelp = false;
		}
    }

	// ================================================================================
	//  public methods
	// --------------------------------------------------------------------------------

	public void ToggleHelp()
	{
		showHelp = !showHelp;

		if (helpTimer > 0)
			helpTimer = 0;
	}

	public void MarkRect(Rect rect)
	{
		Vector3 screenPosition = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y);

		if (rect.Contains(screenPosition))
			_mouseOnInterface = true;
	}
}
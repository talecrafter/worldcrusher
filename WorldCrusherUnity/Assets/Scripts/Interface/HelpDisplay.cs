using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HelpDisplay : MonoBehaviour {

	public float width = 500.0f;
	public float height = 250.0f;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
			Game.Instance.interfaceManager.ToggleHelp();
	}

	public void OnGUI()
	{
		if (!Game.Instance.IsRunning)
			return;

		GUI.skin = Game.Instance.interfaceManager.skin;

		if (Game.Instance.interfaceManager.showHelp)
		{
			DrawReloadMessage();
			DrawGameDescription();
		}

		DrawToggleHelpMessage();
    }

	private void DrawReloadMessage()
	{
		float margin = Game.Instance.interfaceManager.margin;

		Rect bottomRect = new Rect(margin, Screen.height - margin - 50.0f, 300.0f, 50.0f);

		string message = "'R' Restart with new world";

		GUI.Label(bottomRect, message, GUI.skin.customStyles[3]);
	}

	void DrawGameDescription()
	{
		string message = "Navigate with mouse, keyboard or gamepad" + "\n"
			//+ "\n"
			+ "Place attacks and defenses" + "\n";
		Rect rect = new Rect(Screen.width * 0.5f - width * 0.5f, Screen.height * 0.6f, width, height);
		GUI.Label(rect, message, GUI.skin.customStyles[4]);
	}

	void DrawToggleHelpMessage()
	{
		float margin = Game.Instance.interfaceManager.margin;

		Rect bottomRect = new Rect(Screen.width - margin - 300.0f, Screen.height - margin - 50.0f, 300.0f, 50.0f);

		string message = "'F1' Show Controls";
		if (Game.Instance.interfaceManager.showHelp)
			message = "'F1' Hide Controls";

		GUI.Label(bottomRect, message, GUI.skin.customStyles[2]);
	}
}
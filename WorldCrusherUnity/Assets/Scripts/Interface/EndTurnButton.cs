using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndTurnButton : MonoBehaviour {

	public float width = 300.0f;
	public float height = 150.0f;

	void OnGUI() {
		if (Game.Instance.IsRunning)
			DrawButton();
    }

	private void DrawButton()
	{
		GUI.skin = Game.Instance.interfaceManager.skin;

		float margin = Game.Instance.interfaceManager.margin;
		float offset = 10.0f;

		Rect rect = new Rect(margin, margin, width, height);
		if (GUI.Button(rect, "End Turn"))
		{
			Game.Instance.EndTurn();
		}

		Game.Instance.interfaceManager.MarkRect(rect);

		if (Game.Instance.interfaceManager.showHelp)
		{
			Rect bottomRect = new Rect(margin, rect.y + height + offset, width, height);
			GUI.Label(bottomRect, "E, Return", GUI.skin.customStyles[0]);
		}
	}

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionsDisplay : MonoBehaviour {

	void OnGUI()
	{
		if (Game.Instance.IsRunning)
		{
			DrawActions();
		}
	}

	private void DrawActions()
	{
		GUI.skin = Game.Instance.interfaceManager.skin;

		int actionsMax = Game.Instance.player.actions;
		int actionsLeft = Game.Instance.player.actionsLeft;

		float offset = 10.0f;
		float margin = Game.Instance.interfaceManager.margin;

		Texture2D actionIcon = Game.Instance.interfaceManager.actionIcon;

		Color playerColor = Game.Instance.interfaceManager.playerColor;

		for (int i = 0; i < actionsMax; i++)
		{
			if (i < actionsLeft)
				GUI.color = playerColor;
			else
				GUI.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.2f);

			Rect rect = new Rect(Screen.width - i * (actionIcon.width + offset) - margin - actionIcon.width, margin, actionIcon.width, actionIcon.height);
			GUI.DrawTexture(rect, actionIcon);
		}

		if (Game.Instance.interfaceManager.showHelp)
		{
			GUI.color = Color.white;
			Rect bottomRect = new Rect(Screen.width - 200.0f - margin, margin + actionIcon.height + offset, 200.0f, 50.0f);
			GUI.Label(bottomRect, "Place with 'E', 'Return' or Right Mouse Click", GUI.skin.customStyles[1]);
		}
	}
}
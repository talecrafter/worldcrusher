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
		int actionsMax = Game.Instance.player.actions;
		int actionsLeft = Game.Instance.player.actionsLeft;

		float offset = 10.0f;
		float margin = Game.Instance.interfaceManager.margin;

		Texture2D actionIcon = Game.Instance.interfaceManager.actionIcon;

		for (int i = 0; i < actionsMax; i++)
		{
			if (i < actionsLeft)
				GUI.color = Color.white;
			else
				GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);

			Rect rect = new Rect(Screen.width - i * (actionIcon.width + offset) - margin - actionIcon.width, margin, actionIcon.width, actionIcon.height);
			GUI.DrawTexture(rect, actionIcon);
		}
	}
}
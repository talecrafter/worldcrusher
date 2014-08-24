using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Messenger : MonoBehaviour {

	const float timeUntilNextLine = 2.0f;

	public int maxMessages = 4;

	public GUIStyle style;

	public float width = 300.0f;
	public float height = 30.0f;

	public float offset = 15.0f;

	private List<string> _messages = new List<string>();

	private float timer = 0;

	// ================================================================================
	//  unity methods
	// --------------------------------------------------------------------------------

	public void Update()
	{
		timer += Time.deltaTime;
		if (timer >= timeUntilNextLine)
		{
			RemoveMessage();
		}
	}

	public void OnGUI()
	{
		if (Game.Instance.IsRunning && !Game.Instance.interfaceManager.showHelp)
			DisplayMessages();
	}

	private void DisplayMessages()
	{
		float margin = Game.Instance.interfaceManager.margin;

		for (int i = 0; i < _messages.Count; i++)
		{
			float y = Screen.height - margin - height - i * (height + offset);
			Rect rect = new Rect(margin, y, width, height);
			GUI.Label(rect, _messages[i], style);
		}
	}


	// ================================================================================
	//  public methods
	// --------------------------------------------------------------------------------

	public void Message(string message)
	{
		_messages.Add(message);

		if (_messages.Count > maxMessages)
			_messages.RemoveAt(0);

		if (_messages.Count == 1)
			timer = 0;
	}

	public void Clear()
	{
		_messages.Clear();
	}

	// ================================================================================
	//  private methods
	// --------------------------------------------------------------------------------

	private void RemoveMessage()
	{
		if (_messages.Count > 0)
			_messages.RemoveAt(0);

		timer -= timeUntilNextLine;

		if (timer < 0)
			timer = 0;
	}

}
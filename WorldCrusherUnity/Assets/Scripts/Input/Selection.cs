using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selection : MonoBehaviour {

	private Transform _transform;

	private SpriteRenderer[] _rendererList;

	private bool _isVisible = true;

	public void Awake()
	{
		_transform = transform;

		_rendererList = GetComponentsInChildren<SpriteRenderer>();
	}

	public void Update()
	{
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		if (Game.Instance.IsRunning && Game.Instance.inputController.inputType == InputType.KeyboardOrGamepad)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	public void Select(Node node)
	{
		_transform.position = node.positionBehind;
	}

	public void Show()
	{
		ShowOrHide(true);
	}

	public void Hide()
	{
		ShowOrHide(false);
	}

	private void ShowOrHide(bool doShow)
	{
		if (doShow == _isVisible)
			return;

		for (int i = 0; i < _rendererList.Length; i++)
		{
			_rendererList[i].enabled = doShow;
		}

		_isVisible = doShow;
	}

}
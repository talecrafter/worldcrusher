using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selection : MonoBehaviour {

	private Transform _transform;

	public void Awake()
	{
		_transform = transform;
	}

	public void Select(Node node)
	{
		_transform.position = node.positionBehind;
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

}
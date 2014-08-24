using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PressAnyKeyForLoadingLevel : MonoBehaviour {

	public int levelIndex = 0;

    void Update() {
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			LoadNewLevel();
		}
    }

	private void LoadNewLevel()
	{
		Application.LoadLevel(levelIndex);
	}
}
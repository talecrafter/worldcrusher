using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public static Game Instance = null;

	void Awake()
	{
		if (Instance != null)
		{
			DestroyImmediate(gameObject);
			return;
		}

		Instance = this;

		DontDestroyOnLoad(gameObject);
	}

}
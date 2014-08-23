using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public static Game Instance = null;

	public World world = new World();

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			DestroyImmediate(gameObject);
			return;
		}

		Instance = this;

		DontDestroyOnLoad(gameObject);
	}

}
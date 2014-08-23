using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public static Game Instance = null;

	private GameState _state = GameState.Running;

	public World world = new World();
	public InterfaceManager interfaceManager = null;
	public InputController inputController = null;
	public PlayerController playerController = null;

	public Faction player = new Faction(FactionType.Player);
	public Faction enemy = new Faction(FactionType.Enemy);

	public bool IsRunning
	{
		get
		{
			return _state == GameState.Running;
		}
	}

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			DestroyImmediate(gameObject);
			return;
		}

		Instance = this;

		DontDestroyOnLoad(gameObject);

		interfaceManager = GetComponent<InterfaceManager>();
		inputController = GetComponent<InputController>();
		playerController = GetComponent<PlayerController>();
	}

}
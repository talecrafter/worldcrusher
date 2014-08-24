using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public static Game Instance = null;

	private GameState _state = GameState.Running;

	public InterfaceManager interfaceManager = null;
	public Messenger messenger = null;
	public InputController inputController = null;

	// gameplay
	public World world = new World();
	public EndOfTurnSolver endOfTurnSolver = new EndOfTurnSolver();

	// player and enemy
	public PlayerController playerController = null;
	public EnemyController enemyController = null;
	public Faction player = new Faction(FactionType.Player);
	public Faction enemy = new Faction(FactionType.Enemy);

	public bool IsRunning
	{
		get
		{
			return _state == GameState.Running;
		}
	}

	// ================================================================================
	//  unity methods
	// --------------------------------------------------------------------------------

	void Awake()
	{
		// uncomment if used on multiple scenes

		//if (Instance != null && Instance != this)
		//{
		//	DestroyImmediate(gameObject);
		//	return;
		//}

		//DontDestroyOnLoad(gameObject);

		Instance = this;

		interfaceManager = GetComponent<InterfaceManager>();
		messenger = GetComponent<Messenger>();
		inputController = GetComponent<InputController>();
		playerController = GetComponent<PlayerController>();
		enemyController = GetComponent<EnemyController>();
	}

	// ================================================================================
	//  public methods
	// --------------------------------------------------------------------------------

	public void EndTurn()
	{
		StartCoroutine(HandleEndTurn());
	}

	public void Restart()
	{
		messenger.Clear();

		WorldGenerator generator = Object.FindObjectOfType<WorldGenerator>();
		generator.ResetWorld();

		player.NewRound();
		enemy.NewRound();
	}

	// ================================================================================
	//  private methods
	// --------------------------------------------------------------------------------

	private IEnumerator HandleEndTurn()
	{
		// disable player input
		_state = GameState.EndOfTurn;
		interfaceManager.showHelp = false;
		playerController.ShowSelection(false);

		// calculate enemy actions
		enemyController.PlaceActions();
		yield return new WaitForSeconds(0.5f);

		// show result of all attacks
		endOfTurnSolver.GatherActions();
		foreach (var attack in endOfTurnSolver.attacks)
		{
			playerController.Select(attack.node);
			yield return new WaitForSeconds(0.5f);
			if (attack.isDefended)
			{
				attack.node.Defended();
			}
			else
			{
				attack.node.Conquered();
			}
			yield return new WaitForSeconds(0.5f);
		}

		// calculate islands and give small islands to other faction

		yield return new WaitForSeconds(0.5f);

		// examine winning conditions

		// start new round
		world.NewRound();
		_state = GameState.Running;
		playerController.ShowSelection(true);
	}
}
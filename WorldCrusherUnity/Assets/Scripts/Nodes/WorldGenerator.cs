using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

	[Range(0, 100)]
	public int gridWidth = 10;
	[Range(0, 100)]
	public int gridHeight = 10;

	[Range(0, 10.0f)]
	public float offset = 2.0f;
	[Range(0, 0.5f)]
	public float placingVariation = .2f;

	[Range(0, 1.0f)]
	public float randomDeleteChance = 0.05f;

	public NodeDisplay nodePrefab;
	public NodeConnectionDisplay connectionPrefab;

	private GameObject _worldObject = null;

    void Awake() {
		CreateWorld();
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetWorld();
		}
	}

	private void CreateWorld()
	{
		NodeGrid grid = new NodeGrid(gridWidth, gridHeight);

		// floodfill from the corners to delete some worlds		
		DeleteCorners(grid);
		DeleteRandomNodes(grid);

		// create connections in all four directions

		// cut some of the connections

		// gather nodes in islands

		// destroy small islands

		// offset nodes to get a more organic feeling
		OffsetCenters(grid);

		// determine player start nodes

		// create nodes on the map
		CreateNodesFromGrid(grid);
	}

	private void DeleteCorners(NodeGrid grid)
	{
		const int min = 3;
		const int max = 7;
		const float chance = 0.2f;

		grid.FloodFill(0, 0, min, max, chance, grid.Delete);
		grid.FloodFill(0, gridHeight - 1, min, max, chance, grid.Delete);
		grid.FloodFill(gridWidth - 1, 0, min, max, chance, grid.Delete);
		grid.FloodFill(gridWidth - 1, gridHeight - 1, min, max, chance, grid.Delete);
	}

	private void DeleteRandomNodes(NodeGrid grid)
	{
		foreach (var node in grid)
		{
			if (Random.Range(0,1.0f) < randomDeleteChance)
			{
				grid.Delete(node);
			}
		}
	}

	private void OffsetCenters(NodeGrid grid)
	{
		foreach (var node in grid)
		{
			float xOffset = Random.Range(-placingVariation * offset, placingVariation * offset);
			float yOffset = Random.Range(-placingVariation * offset, placingVariation * offset);

			node.y = node.row * offset + xOffset;
			node.x = node.column * offset + yOffset;
		}
	}

	private void ResetWorld()
	{
		DestroyWorld();
		CreateWorld();
	}

	private void DestroyWorld()
	{
		Game.Instance.world.Clear();
		Destroy(_worldObject);
	}

	private void CreateNodesFromGrid(NodeGrid grid)
	{
		World world = Game.Instance.world;

		_worldObject = new GameObject("World");

		foreach (var node in grid)
		{
			NodeDisplay nodeDisplay = Instantiate(nodePrefab, node.position, Quaternion.identity) as NodeDisplay;
			nodeDisplay.gameObject.name = node.ToString();
            nodeDisplay.node = node;

			nodeDisplay.transform.parent = _worldObject.transform;

			world.nodes.Add(node);
        }

		Debug.Log(world.nodes.Count + " nodes created");
	}
}
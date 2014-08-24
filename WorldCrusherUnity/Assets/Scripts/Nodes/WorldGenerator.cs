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
	[Range(0, 0.5f)]
	public float sizeVariation = .2f;

	[Range(0, 1.0f)]
	public float randomDeleteChance = 0.05f;

	public List<Sprite> planetImages = new List<Sprite>();

	public NodeDisplay nodePrefab;
	public NodeConnection connectionPrefab;

	private GameObject _worldObject = null;

    void Start() {
		ResetWorld();
    }

	private void CreateWorld()
	{
		NodeGrid grid = new NodeGrid(gridWidth, gridHeight);

		// floodfill from the corners to delete some worlds		
		DeleteCorners(grid);
		DeleteRandomNodes(grid);

		// create connections in all four directions
		ConnectNodes(grid);

		// cut some of the connections

		// gather nodes in islands and destroy small islands
		grid.RemoveUnconnectedNodes();

		// positions
		CalculatePositions(grid);
		OffsetCenters(grid); // offset nodes to get a more organic feeling

		// create nodes on the map
		CreateNodesFromGrid(grid);

		// determine player start 
		PlaceStartNodes();
	}

	private static void PlaceStartNodes()
	{
		Game.Instance.world.PickStartNode();
		int maxStartNodes = Random.Range(3, 6);
		NodeList nodes = Game.Instance.world.nodes.GetConnectedNodes(Game.Instance.world.home, null, maxStartNodes);
		foreach (var item in nodes)
		{
			item.SetFaction(FactionType.Player);
		}

		Game.Instance.world.nodes.LooseUnconnectedWorlds();
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
			if (UnityEngine.Random.Range(0,1.0f) < randomDeleteChance)
			{
				grid.Delete(node);
			}
		}
	}

	private void ConnectNodes(NodeGrid grid)
	{
		Direction[] directions = grid.allDirections;

		foreach (var node in grid)
		{
			for (int i = 0; i < directions.Length; i++)
			{
				Direction direction = directions[i];

				Node otherNode = grid.GetNodeInDirection(node, direction);
				node.Connect(otherNode, direction);
			}
		}
	}

	private void CalculatePositions(NodeGrid grid)
	{
		float halfWidth = (grid.width - 1) * offset * 0.5f;
		float halfHeight = (grid.height - 1) * offset * 0.5f;

		foreach (var node in grid)
		{
			node.x = node.column * offset - halfWidth;
			node.y = node.row * offset - halfHeight;
		}
	}

	private void OffsetCenters(NodeGrid grid)
	{
		foreach (var node in grid)
		{
			float xOffset = UnityEngine.Random.Range(-placingVariation * offset, placingVariation * offset);
			float yOffset = UnityEngine.Random.Range(-placingVariation * offset, placingVariation * offset);

			node.y += xOffset;
			node.x += yOffset;
		}
	}

	public void ResetWorld()
	{
		DestroyWorld();
		CreateWorld();
		Game.Instance.playerController.SetSelectionToHome();
		Game.Instance.world.NewRound();
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

		// create nodes itself
		foreach (var node in grid)
		{
			NodeDisplay nodeDisplay = Instantiate(nodePrefab, node.position, Quaternion.identity) as NodeDisplay;
			nodeDisplay.gameObject.name = node.ToString();
			nodeDisplay.SetNode(node);

			// variable size
			float size = Random.Range(1.0f - sizeVariation, 1.0f + sizeVariation);
			nodeDisplay.mainImage.transform.localScale = new Vector3(size, size, size);
			nodeDisplay.SetImage(planetImages.PickRandom());

			nodeDisplay.transform.parent = _worldObject.transform;

			world.nodes.Add(node);
        }

		// create connections between the nodes
		foreach (var node in grid)
		{
			CreateConnections(node);
		}
    }

	private void CreateConnections(Node fromNode)
	{
		foreach (var item in fromNode.connections)
		{
			Node toNode = item.Value;

			if (fromNode.display.HasConnection(toNode))
				continue;

			NodeConnection connection = Instantiate(connectionPrefab, fromNode.positionBehind, Quaternion.identity) as NodeConnection;

			connection.fromNode = fromNode;
			connection.toNode = toNode;

			fromNode.display.AttachConnection(toNode, connection);
			toNode.display.AttachConnection(fromNode, connection);

			connection.gameObject.name = connection.ToString();
			connection.transform.parent = _worldObject.transform;

			// rotate
			float distance = Vector3.Distance(fromNode.positionBehind, toNode.positionBehind);
			connection.transform.localScale = new Vector3(distance, 1.0f, 1.0f);
			connection.transform.rotation = Utilities2D.GetRotationFromVectorToVector(fromNode.positionBehind, toNode.positionBehind);
		}
	}
}
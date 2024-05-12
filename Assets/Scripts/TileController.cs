using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TileController : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array of tile prefabs representing different tile types.
    public int gridSize = 5; // Size of the grid (5x5 in your case).

    // Define the desired quantities of each tile type.
    private Dictionary<string, int> desiredTileCounts = new Dictionary<string, int>
    {
        { "Tile_+", 2 },

        { "Tile_C1", 3 },
        { "Tile_C2", 3 },
        { "Tile_C3", 3 },
        { "Tile_C4", 3 },

        { "Tile_D1", 2 },
        { "Tile_D2", 2 },
        { "Tile_D3", 2 },
        { "Tile_D4", 2 },

        { "Tile_L1", 3 },
        { "Tile_L2", 3 },

        { "Tile_S", 1 },
        { "Tile_E", 1 }
    };

    public GameObject boardCenter; // Reference to the empty game object used as the board's center.
    public float distanceBelowBoard = 3.0f; // Set the desired distance below the board.

    public GameObject[,] boardTiles; // 2D Array to represent the tiles on the board

    public GameObject character; // Assign this in the Inspector with your character GameObject
    private Vector3 initialCharacterPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Create a list with the desired tile names.
        List<string> tileNames = new List<string>();

        foreach (var kvp in desiredTileCounts)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                tileNames.Add(kvp.Key);
            }
        }

        // Shuffle the list.
        for (int i = 0; i < tileNames.Count; i++)
        {
            int randomIndex = Random.Range(i, tileNames.Count);
            string temp = tileNames[i];
            tileNames[i] = tileNames[randomIndex];
            tileNames[randomIndex] = temp;
        }

        // Find the index for "tile S" and "tile E."
        int startIndex = tileNames.IndexOf("Tile_S");
        int endIndex = tileNames.IndexOf("Tile_E");

        // Ensure that "tile S" is at the bottom left.
        if (startIndex != -1)
        {
            // Swap the positions of "tile S" and the first tile in the shuffled list.
            string temp = tileNames[startIndex];
            tileNames[startIndex] = tileNames[0];
            tileNames[0] = temp;
        }

        // Ensure that "tile E" is at the top-right (25th tile).
        if (endIndex != -1)
        {
            // Swap the positions of "tile E" and the 25th tile in the shuffled list.
            if (endIndex != 24)
            {
                string temp = tileNames[endIndex];
                tileNames[endIndex] = tileNames[24];
                tileNames[24] = temp;
            }
        }

        // Calculate the offset to center the grid around the empty game object.
        float offset = (gridSize - 1) / 2.0f;
        Vector3 centerPosition = boardCenter.transform.position;
        Vector3 startingPosition = centerPosition - new Vector3(offset, offset, 0);

        // Existing instantiation logic here...
        boardTiles = new GameObject[gridSize, gridSize]; // Initialize the 2D array

        // Loop through the shuffled list and instantiate tiles based on the names.
        int index = 0;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                string tileName = tileNames[index];
                GameObject selectedTilePrefab = GetTilePrefabByName(tileName);

                // Adjust the position calculation as needed.
                Vector3 position = startingPosition + new Vector3(col, -row, 0);
                GameObject instantiatedTile = Instantiate(selectedTilePrefab, position, Quaternion.identity, transform);

                // If this is the start or end tile, adjust its tag.
                if (tileName == "Tile_S" || tileName == "Tile_E")
                {
                    instantiatedTile.tag = tileName == "Tile_S" ? "StartTile" : "EndTile";
                }

                boardTiles[row, col] = instantiatedTile; // Store the tile in the 2D array

                index++;
            }
        }

        // Instantiate and position the last (outside) tile correctly.
        string lastTileName = tileNames[25];
        GameObject lastTilePrefab = GetTilePrefabByName(lastTileName);

        // Calculate the position for the outside tile to be below the board.
        Vector3 lastTilePosition = centerPosition - new Vector3(0, offset + distanceBelowBoard, 0);

        GameObject outsideTile = Instantiate(lastTilePrefab, lastTilePosition, Quaternion.identity, transform);
        outsideTile.tag = "OutsideTile"; // Assign the unique tag.

        StoreCharacterInitialPosition();
    }

    // Function to get the tile prefab by name.
    private GameObject GetTilePrefabByName(string name)
    {
        foreach (GameObject tilePrefab in tilePrefabs)
        {
            if (tilePrefab.name == name)
            {
                return tilePrefab;
            }
        }
        return null; // Handle the case where the tile name is not found.
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        float offset = (gridSize - 1) / 2.0f;
        Vector3 centerPosition = boardCenter.transform.position;
        Vector3 startingPosition = centerPosition - new Vector3(offset, offset, 0);
        Gizmos.DrawSphere(centerPosition, 0.1f); // Draw a small sphere at the center position
        Gizmos.DrawSphere(startingPosition, 0.1f); // Draw a small sphere at the starting position
    }

    void StoreCharacterInitialPosition()
    {
        if (character != null)
        {
            initialCharacterPosition = character.transform.position;
        }
    }

    public void MoveCharacterToInitialPosition()
    {
        if (character != null)
        {
            character.transform.position = initialCharacterPosition;
        }
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;

    // Reference to the TerrainGeneration component
    public TerrainGeneration terrainGeneration;

    public TileEditManager tileEditManager;

    public InventoryManager inventoryManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Other GameManager functions...
}
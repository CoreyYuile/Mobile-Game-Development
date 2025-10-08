using UnityEngine;

public class FarmGrid : MonoBehaviour
{

    [Header("Grid Settings")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 5f;

    [Header("Prefabs")]
    public GameObject unlockedPlotPrefab;
    public GameObject lockedPlotPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        if (unlockedPlotPrefab == null || lockedPlotPrefab == null)
        {
            Debug.Log("One or more prefabs are not assigned!");
            return;
        }

        int centerX = gridWidth / 2;
        int centerZ = gridHeight / 2;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3((x - gridWidth / 2f) * cellSize, 0, (z - gridHeight / 2f) * cellSize);

                // Check if this is one of the 4 centre plots (for the unlocked plots)
                bool isCenterPlot = (x == centerX || x == centerX - 1) && (z == centerZ || z == centerZ - 1);

                GameObject prefabToUse = isCenterPlot ? unlockedPlotPrefab : lockedPlotPrefab;
                GameObject cell = Instantiate(prefabToUse, position, Quaternion.identity, transform);
                cell.name = $"{(isCenterPlot ? "Unlocked" : "Locked")}_Cell_{x}_{z}";
            }
        }
    }
}

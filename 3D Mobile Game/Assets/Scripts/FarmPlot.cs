using UnityEngine;

public class FarmPlot : MonoBehaviour
{

    [Header("Plot State")]
    public bool isOwned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        if (!isOwned)
        {
            UnlockPlot();
        }
        else
        {
            Debug.Log($"{name} has already been unlocked");
        }
    }

    public void UnlockPlot()
    {
        isOwned = true;
        Debug.Log($"{name} unlocked!");

        // Swap to unlocked prefab
        SwapPrefab();
    }

    private void SwapPrefab()
    {
        // Get reference to the unlocked prefab from manager
        var grid = FindFirstObjectByType<FarmGrid>();
        if (grid == null || grid.unlockedPlotPrefab == null)
        {
            Debug.Log("FarmGrid or unlocked prefab not found / assigned");
            return;
        }

        // Spawn the unlocked plot prefab at the position of the locked one
        GameObject newPlot = Instantiate(
            grid.unlockedPlotPrefab,
            transform.position,
            transform.rotation,
            transform.parent
        );

        // Update state and name
        var newPlotScript = newPlot.GetComponent<FarmPlot>();
        newPlotScript.isOwned = true;
        newPlot.name = gameObject.name;

        // Destroy the old locked plot prefab
        Destroy(gameObject);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class FarmGrid : MonoBehaviour
{

    public List<FarmPlot> allPlots = new List<FarmPlot>();

    public static FarmGrid Instance {  get; private set; }

    [Header("Grid Settings")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 5f;

    [Header("Prefabs")]
    public GameObject unlockedPlotPrefab;
    public GameObject lockedPlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    // Generate all plots in a grid
    void GenerateGrid()
    {
        // Check for if both prefabs are set (NECESSARY for this to generate!!)
        if (unlockedPlotPrefab == null || lockedPlotPrefab == null)
        {
            Debug.Log("One or more prefabs are not assigned!");
            return;
        }

        // Mid point (used later)
        int centerX = gridWidth / 2;
        int centerZ = gridHeight / 2;

        // Loop through all points
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calculate the world pos for the plot
                Vector3 position = new Vector3((x - gridWidth / 2f) * cellSize, 0, (z - gridHeight / 2f) * cellSize);

                // Check if this is one of the 4 centre plots (for the unlocked plots)
                bool isCenterPlot = (x == centerX || x == centerX - 1) && (z == centerZ || z == centerZ - 1);

                // If center plot, use unlockedPlotPrefab
                GameObject prefabToUse = isCenterPlot ? unlockedPlotPrefab : lockedPlotPrefab;

                // Instantiate plot, name it
                GameObject cell = Instantiate(prefabToUse, position, Quaternion.identity, transform);
                cell.name = $"{(isCenterPlot ? "Unlocked" : "Locked")}_Cell_{x}_{z}";

                // Store to the list of all plots
                FarmPlot plot = cell.GetComponent<FarmPlot>();
                if (plot != null)
                {
                    plot.gridX = x;
                    plot.gridZ = z;
                    plot.isOwned = isCenterPlot;
                    allPlots.Add(plot);
                }
            }
        }

        SaveManager.Instance.LoadGame();
    }

    // For loading all of the plot data
    public void RestorePlots(List<PlotSaveData> savedPlots)
    {
        // If there is nothing, don't go any further
        if (savedPlots == null)
        {
            return;
        }

        // Loop through every plot read from the save file
        foreach (var plotSave in savedPlots)
        {
            FarmPlot plot = null;
            // Loop through each plot, if plot grids match set plot to p
            foreach (FarmPlot p in allPlots)
            {
                if (p.gridX == plotSave.xIndex && p.gridZ == plotSave.zIndex)
                {
                    plot = p;
                    break;
                }
            }

            // If nothing is found for a specific plot, log it (ideally this should not happen anymore)
            if (plot == null)
            {
                Debug.Log($"No plot data found for {plotSave.xIndex},{plotSave.zIndex}");
                continue;
            }

            // !! NEED TO WORK ON GETTING OWNED PLOTS TO SHOW UP INSTEAD OF LOCKED PLOTS !!
            // I'm tired :(
            plot.isOwned = plotSave.isOwned;

            // Convert the file's state variable back to an enum that can actually be used
            // This is stupid, I hate it.
            if (Enum.TryParse(plotSave.state, out FarmPlot.PlotState loadedState))
            {
                plot.state = loadedState;
            }
            else
            {
                plot.state = FarmPlot.PlotState.Empty;
            }

            // Check for if the plot was originally growing something before the player left
            if (plotSave.plantedTimeTicks > 0)
            {
                // Convert back to UTC DateTime
                DateTime plantedUtc = new DateTime(plotSave.plantedTimeTicks, DateTimeKind.Utc);
                plot.plantedUTCTime = plantedUtc;

                // Calculate how long its been in seconds since the crop was planted
                double elapsed = (DateTime.UtcNow - plantedUtc).TotalSeconds;

                // If its been growing for longer than growthDuration, then state needs to be changed
                if (elapsed >= plot.growthDuration)
                {
                    plot.state = FarmPlot.PlotState.ReadyToHarvest;
                    // !! THIS FEELS MESSY :(
                    plot.SpawnCrop();
                }
                else
                {
                    // Still growing, set state to reflect this
                    plot.state = FarmPlot.PlotState.Growing;
                    // !! THIS FEELS MESSY :(
                    plot.SpawnSeed();
                }
            }
            else
            {
                // Nothing was growing, plot should be empty
                if (plot.state == FarmPlot.PlotState.Empty)
                {
                    // Get rid of any crops or seeds sat there (as they shouldn't be there!)
                    if (plot.currentCropPrefab != null)
                    {
                        Destroy(plot.currentCropPrefab);
                    }
                }
                // Gotta account for if the player left the game with a crop ready to harvest
                else if (plot.state == FarmPlot.PlotState.ReadyToHarvest)
                {
                    // Spawn a grown crop that the player can harvest
                    plot.SpawnCrop();
                }
            }
        }

        // I'm gonna lose my damn mind I should've just kept to PlayerPrefs for the time being BUT IT WORKS YAY!!
        Debug.Log("Plots restored from save.");
    }
}
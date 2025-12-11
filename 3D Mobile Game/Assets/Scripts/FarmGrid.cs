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
        if (savedPlots == null || savedPlots.Count == 0)
        {
            return;
        }

        // Loop through every plot read from the save file
        foreach (var plotSave in savedPlots)
        {
            // Find the matching plot by grid coordinates
            FarmPlot plot = allPlots.Find(p =>
                p.gridX == plotSave.xIndex &&
                p.gridZ == plotSave.zIndex
            );

            // If nothing is found for a specific plot, log it (ideally this should not happen anymore)
            if (plot == null)
            {
                Debug.Log($"Plot not found for {plotSave.xIndex},{plotSave.zIndex}");
                continue;
            }

            // Swap prefab if unlocked or not
            bool shouldBeOwned = plotSave.isOwned;

            if (plot.isOwned != shouldBeOwned)
            {
                plot = SwapPrefab(plot, shouldBeOwned ? unlockedPlotPrefab : lockedPlotPrefab);
            }

            plot.isOwned = shouldBeOwned;

            // ---------- RESTORE PLOT STATE ----------
            if (!Enum.TryParse(plotSave.state, out FarmPlot.PlotState loadedState))
            {
                loadedState = FarmPlot.PlotState.Empty;
            }

            plot.state = loadedState;
            Enum.TryParse(plotSave.crop, out CropData.CropIDs cropID);
            

            // Check for if the plot was originally growing something before the player left
            if (plotSave.plantedTimeTicks > 0)
            {
                // Convert back to UTC DateTime
                plot.plantedUTCTime = new DateTime(plotSave.plantedTimeTicks, DateTimeKind.Utc);
                // Calculate how long its been in seconds since the crop was planted
                double elapsed = (DateTime.UtcNow - plot.plantedUTCTime).TotalSeconds;

                // !! FIX THIS TO USE CROPDATA GROWTH TIME !!
                // If its been growing for longer than growthDuration, then state needs to be changed
                if (elapsed >= plot.growthDuration)
                {
                    // Crop finished growing while player was away
                    plot.state = FarmPlot.PlotState.ReadyToHarvest;
                    switch (cropID)
                    {
                        case CropData.CropIDs.Tomato:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[3]);
                            break;
                        case CropData.CropIDs.Potato:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[1]);
                            break;
                        case CropData.CropIDs.Wheat:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[4]);
                            break;
                        case CropData.CropIDs.Corn:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[0]);
                            break;
                        case CropData.CropIDs.Pumpkin:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[2]);
                            break;
                    }
                    //plot.SpawnCrop();
                }
                else
                {
                    // Still growing, set state to reflect this
                    plot.state = FarmPlot.PlotState.Growing;
                    switch (cropID)
                    {
                        case CropData.CropIDs.Tomato:
                            plot.SpawnSeed(MenuManager.Instance.availableCrops[3]);
                            break;
                        case CropData.CropIDs.Potato:
                            plot.SpawnSeed(MenuManager.Instance.availableCrops[1]);
                            break;
                        case CropData.CropIDs.Wheat:
                            plot.SpawnSeed(MenuManager.Instance.availableCrops[4]);
                            break;
                        case CropData.CropIDs.Corn:
                            plot.SpawnSeed(MenuManager.Instance.availableCrops[0]);
                            break;
                        case CropData.CropIDs.Pumpkin:
                            plot.SpawnSeed(MenuManager.Instance.availableCrops[2]);
                            break;
                    }
                    //plot.SpawnSeed();
                }
            }
            else
            {
                // Nothing was growing, plot should be empty
                if (plot.state == FarmPlot.PlotState.Empty)
                {
                    if (plot.currentCropPrefab != null)
                    {
                        Destroy(plot.currentCropPrefab);
                    }
                }
                // Gotta account for if the player left the game with a crop ready to harvest
                else if (plot.state == FarmPlot.PlotState.ReadyToHarvest)
                {
                    switch (cropID)
                    {
                        case CropData.CropIDs.Tomato:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[3]);
                            break;
                        case CropData.CropIDs.Potato:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[1]);
                            break;
                        case CropData.CropIDs.Wheat:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[4]);
                            break;
                        case CropData.CropIDs.Corn:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[0]);
                            break;
                        case CropData.CropIDs.Pumpkin:
                            plot.SpawnCrop(MenuManager.Instance.availableCrops[2]);
                            break;
                    }
                    //// Spawn a grown crop that the player can harvest
                    //plot.SpawnCrop();
                }
            }
        }

        Debug.Log("Plots restored from save.");
    }


    public FarmPlot SwapPrefab(FarmPlot oldPlot, GameObject newPrefab)
    {
        GameObject newObj = Instantiate(
            newPrefab,
            oldPlot.transform.position,
            oldPlot.transform.rotation,
            oldPlot.transform.parent
        );

        // Get new script
        FarmPlot newPlot = newObj.GetComponent<FarmPlot>();

        // Copy grid coordinates
        newPlot.gridX = oldPlot.gridX;
        newPlot.gridZ = oldPlot.gridZ;

        // Replace in list
        int index = allPlots.IndexOf(oldPlot);
        allPlots[index] = newPlot;

        Destroy(oldPlot.gameObject);

        return newPlot;
    }
}
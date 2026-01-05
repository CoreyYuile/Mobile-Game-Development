using UnityEngine;
using System.Collections;
using System;

public class FarmPlot : MonoBehaviour
{
    [Header("Plot State")]

    public bool isOwned = false;

    public CropData currentCrop;

    // Different states a plot can be in
    // (!! Could add isOwned as a state instead of bool?? !!)
    public enum PlotState
    {
        Empty,
        Growing,
        ReadyToHarvest
    }
    public PlotState state = PlotState.Empty;

    [Header("Growth Settings")]
    public float growthDuration = 5f;
    public GameObject seedlingPrefab;
    public GameObject grownPrefab;

    public GameObject currentCropPrefab;
    public Transform cropAnchor;

    [Header("Plot Misc Settings")]

    public int unlockCost = 50;
    public int harvestReward = 20;

    public int gridX;
    public int gridZ;

    public DateTime plantedTime;

    //[Header("References")]

    //private MenuManager menuManager;
    //private MoneyManager moneyManager;

    private void Start()
    {
        //menuManager = FindAnyObjectByType<MenuManager>();
        //moneyManager = FindAnyObjectByType<MoneyManager>();
    }

    private void Update()
    {
        if (state == PlotState.Growing && currentCrop != null)
        {
            // Check if elapsed time has gone past growthDuration
            double elapsedSeconds = (DateTime.UtcNow - plantedTime).TotalSeconds;

            float growthMultiplier = 1.0f;

            // Checks for if a weather multiplier should be added to the growth time
            if (WeatherData.Instance.currentWeatherType == WeatherData.WeatherType.rain)
            {
                growthMultiplier = 0.5f;
            }
            else if (WeatherData.Instance.currentWeatherType == WeatherData.WeatherType.cloudy)
            {
                growthMultiplier = 1.25f;
            }

            // Check if enough time has passed for seed to grow into a crop
            if (elapsedSeconds >= (currentCrop.growthDuration * growthMultiplier))
            {
                // Change state and update visuals
                state = PlotState.ReadyToHarvest;
                Debug.Log($"Crop is ready to harvest at {name}!");

                // Play vibration
                PhoneVibration.Instance.MediumVibration();

                // Swap to grown crop prefab
                if (currentCropPrefab)
                {
                    Destroy(currentCropPrefab);
                }
                if (grownPrefab)
                {
                    currentCropPrefab = Instantiate(currentCrop.grownPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
                }

                SaveManager.Instance.SaveGame();
            }
        }
    }

    // Method to handling when the player taps on the plot
    public void HandleTap()
    {
        // Plot unlocking
        if (!isOwned)
        {
            //UnlockPlot();
            unlockCost = MoneyManager.Instance.NextPlotCost();
            MenuManager.Instance.ShowBuyPlotPopup(this, unlockCost);
            return;
        }

        // Check what state the plot is currently in
        switch (state)
        {
            // If empty, plant seed
            case PlotState.Empty:
                PlantSeed(MenuManager.Instance.selectedCrop);
                //MenuManager.Instance.ShowCropSelection(this);
                break;

            // If the seed is still growing, do nothing (for now)
            case PlotState.Growing:
                Debug.Log($"crop is still growing at {name}");
                break;

            // If ready to harvest, handle harvesting
            case PlotState.ReadyToHarvest:
                HarvestCrop();
                break;
        }
    }

    // Handles seed planting
    public void PlantSeed(CropData cropdata)
    {
        currentCrop = cropdata;

        // Get the time that the seed was planted
        plantedTime = DateTime.UtcNow;

        // Change state
        state = PlotState.Growing;

        // Instantiate the seed prefab at the anchor position on the plot
        if (seedlingPrefab)
        {
            if (cropAnchor == null)
            {
                Debug.Log($"{name} has no crop anchor set!");
                cropAnchor = transform;
            }

            // Play vibration
            PhoneVibration.Instance.LightVibration();

            // Spawn the crop seed
            GameObject crop = Instantiate(cropdata.seedlingPrefab, cropAnchor.position, cropAnchor.rotation);
            crop.transform.SetParent(cropAnchor, true);
            crop.transform.localScale = Vector3.one;

            currentCropPrefab = crop;
        }

        Debug.Log($"Planted a seed at {name}");
        SaveManager.Instance.SaveGame();

        // Start growth timer
        //StartCoroutine(GrowCrop());
    }

    //private IEnumerator GrowCrop()
    //{
    //    yield return new WaitForSeconds(growthDuration);

    //    state = PlotState.ReadyToHarvest;
    //    Debug.Log($"crop is ready to harvest at {name}");

    //    // Swap to grown crop prefab
    //    if (currentCropPrefab)
    //    {
    //        Destroy(currentCropPrefab);
    //    }
    //    if (grownPrefab)
    //    {
    //        currentCropPrefab = Instantiate(grownPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
    //    }
    //}

    // Handles the harvest
    public void HarvestCrop()
    {
        // This is the fallback for if I screwed anything up and the script cannot figure out what crop is should be
        if (currentCrop == null)
        {
            Debug.Log("!! NO CROP DATA, DEFAULTING TO BASE SETTINGS !!");
            // Change state back to empty, get rid of crop
            state = PlotState.Empty;
            if (currentCropPrefab)
            {
                Destroy(currentCropPrefab);
            }

            // Give some sort of haptic feedback, reward player with money
            PhoneVibration.Instance.DefaultVibration();
            MoneyManager.Instance.AddMoney(harvestReward);
            Debug.Log($"Harvested at {name} and earned {harvestReward}");

            currentCrop = null;
        }
        else
        {
            // Change state back to empty, get rid of crop
            state = PlotState.Empty;
            if (currentCropPrefab)
            {
                Destroy(currentCropPrefab);
            }

            // Play vibration
            PhoneVibration.Instance.HeavyVibration();

            // Check if a weather multiplier should be applied
            if (WeatherData.Instance.currentWeatherType == WeatherData.WeatherType.rain)
            {
                MoneyManager.Instance.AddMoney(currentCrop.harvestReward * 2);
                Debug.Log($"Harvested at {name} and earned {harvestReward * 2}");
            }
            else
            {
                MoneyManager.Instance.AddMoney(currentCrop.harvestReward);
                Debug.Log($"Harvested at {name} and earned {harvestReward}");
            }

            currentCrop = null;
        }

        SaveManager.Instance.SaveGame();
    }

    // Handle unlocking the selected plot
    public void UnlockPlot()
    {
        isOwned = true;
        Debug.Log($"{name} unlocked!");

        if (FarmGrid.Instance.unlockedPlotPrefab == null)
        {
            Debug.Log("FarmGrid or unlocked prefab not found / assigned");
            return;
        }

        // Spawn the unlocked plot prefab at the position of the locked one
        GameObject newPlot = Instantiate(FarmGrid.Instance.unlockedPlotPrefab, transform.position, transform.rotation, transform.parent);

        // Update state and name for the new script as the old one will be deleted with the game object
        var newPlotScript = newPlot.GetComponent<FarmPlot>();
        newPlotScript.isOwned = true;
        newPlot.name = gameObject.name;

        PhoneVibration.Instance.MediumVibration();

        // Destroy the old locked plot game object and script
        Destroy(gameObject);

        SaveManager.Instance.SaveGame();
    }

    // !! THERE HAS TO BE A BETTER WAY TO DO THIS THERE HAS TO BE A BETTER WAY TO DO THIS THERE HAS TO BE A BETTER WAY TO DO THIS !!
    public void SpawnSeed(CropData cropData)
    {
        // Set current crop to cropdata found in save
        currentCrop = cropData;

        // Exit out if there is nothing in the cropdata's prefab
        if (cropData.seedlingPrefab == null)
        {
            return;
        }
        if (cropAnchor == null)
        {
            cropAnchor = transform;
        }
        // This shouldn't happen, but best just to make sure that a crop hasn't already been accidentally placed in there
        if (currentCropPrefab)
        {
            Destroy(currentCropPrefab);
        }

        // Instantiate the crop on the plot
        GameObject crop = Instantiate(cropData.seedlingPrefab, cropAnchor.position, cropAnchor.rotation);

        // Have to add this otherwise it scales horifically but don't have to for crop spawning???
        // !! FIGURE OUT WHY?!?!? !!
        crop.transform.SetParent(cropAnchor, true);
        crop.transform.localScale = Vector3.one;
        currentCropPrefab = crop;
    }

    public void SpawnCrop(CropData cropData)
    {
        //Debug.Log(cropData.name);

        // Set current crop to cropdata found in save
        currentCrop = cropData;

        // Exit out if there is nothing in the cropdata's prefab
        if (cropData.grownPrefab == null)
        {
            return;
        }
        if (cropAnchor == null)
        {
            cropAnchor = transform;
        }
        // This shouldn't happen, but best just to make sure that a crop hasn't already been accidentally placed in there
        if (currentCropPrefab)
        {
            Destroy(currentCropPrefab);
        }

        // Instantiate the crop on the plot
        GameObject crop = Instantiate(cropData.grownPrefab, cropAnchor.position, cropAnchor.rotation, cropAnchor);
        //crop.transform.localScale = Vector3.one;
        currentCropPrefab = crop;
    }
}

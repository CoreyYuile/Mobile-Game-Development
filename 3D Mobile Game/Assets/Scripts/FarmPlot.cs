using UnityEngine;
using System.Collections;
using System;

public class FarmPlot : MonoBehaviour
{
    [Header("Plot State")]

    public bool isOwned = false;

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

    private GameObject currentCropPrefab;
    public Transform cropAnchor;

    [Header("Plot Misc Settings")]

    public int unlockCost = 50;
    public int harvestReward = 20;

    private DateTime plantedUTCTime;

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
        if (state == PlotState.Growing)
        {
            double elapsedSeconds = (DateTime.UtcNow - plantedUTCTime).TotalSeconds;
            if (elapsedSeconds >= growthDuration)
            {
                state = PlotState.ReadyToHarvest;
                Debug.Log($"{name}: crop is ready to harvest!");

                // Swap to grown crop prefab
                if (currentCropPrefab)
                {
                    Destroy(currentCropPrefab);
                }
                if (grownPrefab)
                {
                    currentCropPrefab = Instantiate(grownPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
                }
            }
        }
    }

    // Method to handling when the player taps on the plot
    public void HandleTap()
    {
        // Plot unlocking
        // !! LATER CHANGE THIS TO REQUIRE CURRENCY TO UNLOCK !!
        if (!isOwned)
        {
            //UnlockPlot();
            MenuManager.Instance.ShowBuyPlotPopup(this, unlockCost);
            return;
        }

        // Check what state the plot is currently in
        switch (state)
        {
            // If empty, plant seed
            case PlotState.Empty:
                PlantSeed();
                break;

            // If the seed is still growing, do nothing (for now)
            case PlotState.Growing:
                Debug.Log($"{name}: crop is still growing...");
                break;

            // If ready to harvest, handle harvesting
            case PlotState.ReadyToHarvest:
                HarvestCrop();
                break;
        }
    }

    private void PlantSeed()
    {
        plantedUTCTime = DateTime.UtcNow;

        // Change state
        state = PlotState.Growing;
        Debug.Log($"{name}: planted a seed!");

        // Instantiate the seed prefab at the anchor position on the plot
        if (seedlingPrefab)
        {
            if (cropAnchor == null)
            {
                Debug.LogWarning($"{name} has no crop anchor set!");
                cropAnchor = transform;
            }

            GameObject crop = Instantiate(seedlingPrefab, cropAnchor.position, cropAnchor.rotation);
            crop.transform.SetParent(cropAnchor, true);
            crop.transform.localScale = Vector3.one;

            currentCropPrefab = crop;
        }

        // Start growth timer
        //StartCoroutine(GrowCrop());
    }

    private IEnumerator GrowCrop()
    {
        yield return new WaitForSeconds(growthDuration);

        state = PlotState.ReadyToHarvest;
        Debug.Log($"{name}: crop is ready to harvest!");

        // Swap to grown crop prefab
        if (currentCropPrefab)
        {
            Destroy(currentCropPrefab);
        }
        if (grownPrefab)
        {
            currentCropPrefab = Instantiate(grownPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
        }
    }

    private void HarvestCrop()
    {
        Debug.Log($"{name}: harvested crop!");

        state = PlotState.Empty;
        if (currentCropPrefab)
        {
            Destroy(currentCropPrefab);
        }

        PhoneVibration.Instance.HeavyVibration();
        MoneyManager.Instance.AddMoney(harvestReward);
        Debug.Log($"Harvested at {name} and earned {harvestReward}");
    }

    public void UnlockPlot()
    {
        isOwned = true;
        Debug.Log($"{name} unlocked!");

        var grid = FindFirstObjectByType<FarmGrid>();
        if (grid == null || grid.unlockedPlotPrefab == null)
        {
            Debug.LogWarning("FarmGrid or unlocked prefab not found / assigned");
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

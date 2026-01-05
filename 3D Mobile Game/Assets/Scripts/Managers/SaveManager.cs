using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{

    // !! LOOK AT ENCRYPTING THE SAVE DATA !!
    // Currently can just edit the .json junk as it is in plaintext, encryption would be a better solution

    public static SaveManager Instance { get; private set; }

    // the path that the json file will be saved to
    // Would just set it here but Unity is being stupid and annoying
    private static string savePath;

    private void Awake()
    {
        Instance = this;
        savePath = Application.persistentDataPath + "/save.json";
    }

    private void Start()
    {
        //LoadGame();

        // !! UNCOMMENT THIS ONCE TO DELETE SAVEDATA !!
        //SaveSystem.DeleteSave();
    }

    // Save the game when the game closes
    // !! THIS DOESN'T WORK ON ANDROID BUT ONFOCUS DOES?!?!?!? !!
    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveGame();
        }
    }

    // Get all the necessary data to write to the savesystem
    public void SaveGame()
    {
        SaveData data = new SaveData();

        // Grab current money from the moneymanager
        data.playerMoney = MoneyManager.Instance.currentMoney;
        data.playerNetMoney = MoneyManager.Instance.netMoney;

        // Collect all plot data
        foreach (var plot in FarmGrid.Instance.allPlots)
        {
            // We need the grid pos of the plot, if it is owned, and state it was left in
            PlotSaveData plotData = new PlotSaveData();
            plotData.xIndex = plot.gridX;
            plotData.zIndex = plot.gridZ;
            plotData.isOwned = plot.isOwned;
            // Gotta convert this to string :/
            plotData.state = plot.state.ToString();

            // If the state was currently growing something, we'll need to convert the UTC time to game ticks to store easier
            if (plot.state == FarmPlot.PlotState.Growing)
            {
                plotData.plantedTimeTicks = plot.plantedTime == DateTime.MinValue ? 0 : plot.plantedTime.Ticks;
            }
            // Otherwise just leave it as 0
            else
            {
                plotData.plantedTimeTicks = 0;
            }
            // Get the name of the crop currently planted (if any)
            if (plot.currentCrop != null)
            {
                plotData.crop = plot.currentCrop.cropName;
            }
            else
            {
                // Keep this space blank if no crops on the plot
                plotData.crop = "";
            }

            // Add this to the SaveData
            data.plots.Add(plotData);
        }

        // Write all of this funky junk to the json file
        WriteSave(data);
    }

    // Load data from the json file
    public void LoadGame()
    {
        // Get SaveSystem to fetch and read the data
        SaveData data = ReadSave();
        if (data == null)
        {
            Debug.Log("No save found, new game");
            return;
        }

        // Overwrite the current money with what was saved
        MoneyManager.Instance.currentMoney = data.playerMoney;
        MenuManager.Instance.UpdateMoneyDisplay(MoneyManager.Instance.currentMoney);

        MoneyManager.Instance.netMoney = data.playerNetMoney;

        // call the FarmGrid to overwrite the plots with the saved data (THIS IS WHERE THE MAIN LOADING IS)
        FarmGrid.Instance.RestorePlots(data.plots);
    }

    // Save to file
    public static void WriteSave(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved to: " + savePath);
    }

    // Load the file
    public static SaveData ReadSave()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found");
            return null;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }

    // !! FOR IF PROGRESS NEEDS TO BE RESET !!
    public static void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Deleted save");
        }
    }
}

// Main save data to be reading and writing to
[Serializable]
public class SaveData
{
    public int playerMoney;
    public int playerNetMoney;
    public List<PlotSaveData> plots = new List<PlotSaveData>();
}

// Save data format for individual plots (contains everything from what plot it is, what state it was left in, etc)
[Serializable]
public class PlotSaveData
{
    public int xIndex;
    public int zIndex;
    public bool isOwned;
    public string state;
    public string crop;
    public long plantedTimeTicks;
}

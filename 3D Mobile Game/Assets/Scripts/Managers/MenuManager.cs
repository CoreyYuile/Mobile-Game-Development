using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; private set; }

    [Header("Crop UI Settings")]

    public CropData[] availableCrops;
    private FarmPlot selectedPlot;
    public CropData selectedCrop;

    [Header("UI Text References")]

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI buyPlotPopupText;
    //public TMP_Text inputText;

    [Header("UI Menu GO References")]

    public GameObject buyPlotPopup;
    public GameObject buyPlotRefused;
    public GameObject CropSelection;

    private FarmPlot pendingPlot;

    //[Header("Other References")]

    //private MoneyManager mm;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //mm = FindAnyObjectByType<MoneyManager>();
        moneyText.text = $": {MoneyManager.Instance.currentMoney}";
    }

    // Update the money text
    public void UpdateMoneyDisplay(int amount)
    {
        moneyText.text = $": {amount}";
    }

    // Set the purchase popup to active
    public void ShowBuyPlotPopup(FarmPlot plot, int cost)
    {
        pendingPlot = plot;
        // Set text
        buyPlotPopupText.text = $"Buy plot for {cost}??";
        buyPlotPopup.SetActive(true);
    }

    public void OnConfirmBuyPlot()
    {
        // Call a check to see if the player can afford the plot, if they can, call to unlock plot
        if ((pendingPlot != null) && (MoneyManager.Instance.RemoveMoney(pendingPlot.unlockCost)))
        {
            pendingPlot.UnlockPlot();
        }
        else
        {
            Debug.Log("Not Enough Money!");
            buyPlotPopup.SetActive(false);
            buyPlotRefused.SetActive(true);
        }

        // Deactivate popup
        buyPlotPopup.SetActive(false);
    }

    public void OnCancelBuyPlot()
    {
        pendingPlot = null;
        if (buyPlotPopup)
        {
            buyPlotPopup.SetActive(false);
        }
    }

    // Display a rewarded ad on the player's screen
    public void OnShowRewardedAd()
    {
        AdsManager.Instance.rewardedAds.ShowRewardedAd();
    }

    public void ShowCropSelection(FarmPlot plot)
    {
        selectedPlot = plot;
        CropSelection.SetActive(true);
    }

    public CropData GetCropID(CropData.CropIDs id)
    {
        foreach (var crop in availableCrops)
        {
            if (crop.cropID == id)
            {
                return crop;
            }
        }

        return null;
    }


    public void SelectCropFromMenu(CropData cropData)
    {
        selectedPlot.PlantSeed(cropData);
        selectedPlot = null;
        CropSelection.SetActive(false);
    }

    public void OnCropSelected(CropData cropData)
    {
        //SelectCropFromMenu(availableCrops[cropIndex]);
        selectedCrop = cropData;
    }

    public void ChangeAutoType(Toggle toggle)
    {
        TapManager.Instance.isHarvesting = toggle.isOn;
    }

    public void ActivateKeyboard()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    //public void OnGUI()
    //{
    //    string stringToEdit = inputText.text;
    //    stringToEdit = GUI.TextField(new Rect(10, 10, 200, 30), stringToEdit, 30);

    //    if (GUI.Button(new Rect(10, 50, 200, 100), "Default"))
    //    {
    //        TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    //    }
    //}

    public void PlayGame()
    {
        SceneManager.LoadScene("Farm");
    }
}
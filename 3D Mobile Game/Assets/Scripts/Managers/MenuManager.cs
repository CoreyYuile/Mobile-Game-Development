using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; private set; }

    [Header("Crop UI Settings")]

    public CropData[] availableCrops;
    private FarmPlot selectedPlot;

    [Header("UI Text References")]

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI buyPlotPopupText;

    [Header("UI Menu GO References")]

    public GameObject buyPlotPopup;
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

        moneyText.text = $"MONEY: {MoneyManager.Instance.currentMoney}";

        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.onMoneyChanged += UpdateMoneyDisplay;
        }
    }

    // Update the money text
    public void UpdateMoneyDisplay(int amount)
    {
        if (moneyText)
        {
            moneyText.text = $"MONEY: {amount}";
        }
    }

    // Set the purchase popup to active
    public void ShowBuyPlotPopup(FarmPlot plot, int cost)
    {
        pendingPlot = plot;
        if (buyPlotPopup && buyPlotPopupText)
        {
            // Set text
            buyPlotPopupText.text = $"Buy plot for {cost}??";
            buyPlotPopup.SetActive(true);
        }
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
        AdsManager.instance.rewardedAds.ShowRewardedAd();
    }

    public void ShowCropSelection(FarmPlot plot)
    {
        selectedPlot = plot;
        CropSelection.SetActive(true);
    }

    public void SelectCropFromMenu(CropData cropData)
    {
        selectedPlot.PlantSeed(cropData);
        selectedPlot = null;
        CropSelection.SetActive(false);
    }
}

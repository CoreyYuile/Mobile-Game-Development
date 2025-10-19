using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; private set; }

    [Header("UI Text References")]

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI buyPlotPopupText;

    [Header("UI Menu GO References")]

    public GameObject buyPlotPopup;

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
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.onMoneyChanged += UpdateMoneyDisplay;
        }
    }

    public void UpdateMoneyDisplay(int amount)
    {
        if (moneyText)
        {
            moneyText.text = $"MONEY: {amount}";
        }
    }

    public void ShowBuyPlotPopup(FarmPlot plot, int cost)
    {
        pendingPlot = plot;
        if (buyPlotPopup && buyPlotPopupText)
        {
            buyPlotPopupText.text = $"Buy plot for {cost}??";
            buyPlotPopup.SetActive(true);
        }
    }

    public void OnConfirmBuyPlot()
    {
        if ((pendingPlot != null) && (MoneyManager.Instance.RemoveMoney(pendingPlot.unlockCost)))
        {
            pendingPlot.UnlockPlot();
        }
        else
        {
            Debug.Log("Not Enough Money!");
        }

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

    public void OnShowRewardedAd()
    {
        AdsManager.instance.rewardedAds.ShowRewardedAd();
    }
}

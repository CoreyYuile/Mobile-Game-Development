using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class MoneyManager : MonoBehaviour
{

    public static MoneyManager Instance { get; private set; }

    public event Action<int> onMoneyChanged;
    public event Action<int> onNetMoneyChanged;

    [Header("Money Settings")]

    public int startMoney = 100;
    public int currentMoney;
    public int netMoney;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Set starting money for now
        if (currentMoney == 0)
        {
            currentMoney = startMoney;
        }
        onMoneyChanged?.Invoke(currentMoney);
        onNetMoneyChanged?.Invoke(netMoney);
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public bool HasEnough(int amount)
    {
        return currentMoney >= amount;
    }

    // Add money, self-explanatory
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        netMoney += amount;

        onMoneyChanged?.Invoke(currentMoney);
        onNetMoneyChanged?.Invoke(netMoney);

        Debug.Log($"+{amount} money added, total: {currentMoney}, net worth: {netMoney}");
    }

    // Remove money from player
    public bool RemoveMoney(int amount)
    {
        // Check if the player has enough, otherwise they'll be in debt or something
        if (!HasEnough(amount))
        {
            return false;
        }

        currentMoney -= amount;
        onMoneyChanged?.Invoke(currentMoney);
        onNetMoneyChanged?.Invoke(netMoney);

        Debug.Log($"Money spent: -{amount}, Total: {currentMoney}, NetWorth: {netMoney}");
        return true;
    }
}

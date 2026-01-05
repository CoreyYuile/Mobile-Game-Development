using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Dan.Main;
using System.Net;
using Dan.Demo;
using Dan.Models;
using System;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    [Header("Leaderboard Essentials:")]
    [SerializeField] private Transform _entryDisplayParent;
    [SerializeField] private EntryDisplay _entryDisplayPrefab;
    [SerializeField] private CanvasGroup _leaderboardLoadingPanel;

    [Header("Search Query Essentials:")]
    [SerializeField] private TMP_Dropdown _timePeriodDropdown;
    [SerializeField] private TMP_InputField _pageInput, _entriesToTakeInput;
    [SerializeField] private int _defaultPageNumber = 1, _defaultEntriesToTake = 100;

    [Header("Personal Entry:")]
    [SerializeField] private RectTransform _personalEntryPanel;
    [SerializeField] private TextMeshProUGUI _personalEntryText;
    [SerializeField] private TextMeshProUGUI PlayerNameInputPlaceholder, playerNameInputText;

    private string publicKey = "e523bc06ccdeea6c772ad9ca7033cadb45c91a7352a61808730afeefa5507b90";
    private string privateKey = "10001580531a2eb3d2ad690b8eec8a96b091b4004b5fca6a2e0e0d7a9767d6ef818e1deef630b0c34943e9d45f5a7663086d211d197e1faf06db125a5a12c53d229cfbc68f42c07944d349fa24846f065667f1199c6eaf5b0d17813c81ae053ab23da14b35da8a83af55ab16229bd43ebdf62e18d323eea1fd5a0b2a460a47ae";

    private Coroutine _personalEntryMoveCoroutine;

    // !! THIS SCRIPT WAS TAKEN FROM A PREVIOUS PROJECT !!
    // !! If I remember correctly, a lot of this was derived from official documentation and the demo setup. A lot has been modified to change it however. !!

    // Send a database query based off of values the player can change
    public void Load()
    {

        // Get correct time period the player has asked for through the dropdown
        // !! REMOVED THIS TO FREE UP SPACE, COULD DELETE THIS IN CLEANUP??? !!
        var timePeriod =
            _timePeriodDropdown.value == 1 ? Dan.Enums.TimePeriodType.Today :
            _timePeriodDropdown.value == 2 ? Dan.Enums.TimePeriodType.ThisWeek :
            _timePeriodDropdown.value == 3 ? Dan.Enums.TimePeriodType.ThisMonth :
            _timePeriodDropdown.value == 4 ? Dan.Enums.TimePeriodType.ThisYear : Dan.Enums.TimePeriodType.AllTime;

        // Get the specified page number
        var pageNumber = int.TryParse(_pageInput.text, out var pageValue) ? pageValue : _defaultPageNumber;
        pageNumber = Mathf.Max(1, pageNumber);
        _pageInput.text = pageNumber.ToString();

        // Get the requested amount of entries to display per page
        var take = int.TryParse(_entriesToTakeInput.text, out var takeValue) ? takeValue : _defaultEntriesToTake;
        take = Mathf.Clamp(take, 1, 100);
        _entriesToTakeInput.text = take.ToString();

        // Set up the search query that will be sent to database
        var searchQuery = new LeaderboardSearchQuery
        {
            Skip = (pageNumber - 1) * take,
            Take = take,
            TimePeriod = timePeriod
        };

        // Send off request, call OnLeaderboardLoaded if all goes well
        Leaderboards.Mobile.GetEntries(searchQuery, OnLeaderboardLoaded, ErrorCallback);
        ToggleLoadingPanel(true);
    }

    // Increment / decrement page number
    public void ChangePageBy(int amount)
    {
        // Get the pagenumber from the text UI
        var pageNumber = int.TryParse(_pageInput.text, out var pageValue) ? pageValue : _defaultPageNumber;
        // Increment by specified amount
        pageNumber += amount;
        // If the player tries anything funny like setting it below 1, exit out
        if (pageNumber < 1)
        {
            return;
        }
        // Set UI text to reflect the change
        _pageInput.text = pageNumber.ToString();

        // Call to load the entries for the new page
        Load();
    }

    // Called if the search query returns with data
    private void OnLeaderboardLoaded(Entry[] entries)
    {
        // Clear all the old entries if changing page
        foreach (Transform t in _entryDisplayParent)
        {
            Destroy(t.gameObject);
        }

        // Add in all the new entries
        foreach (var t in entries)
        {
            var entryDisplay = Instantiate(_entryDisplayPrefab.gameObject, _entryDisplayParent);
            entryDisplay.GetComponent<EntryDisplay>().SetEntry(t);
        }

        // Finished loading
        ToggleLoadingPanel(false);
    }

    private void ToggleLoadingPanel(bool isOn)
    {
        _leaderboardLoadingPanel.alpha = isOn ? 1f : 0f;
        _leaderboardLoadingPanel.interactable = isOn;
        _leaderboardLoadingPanel.blocksRaycasts = isOn;
    }

    // Used for the player position button, slide out a submenu with details
    public void MovePersonalEntryMenu(float xPos)
    {
        if (_personalEntryMoveCoroutine != null)
        {
            StopCoroutine(_personalEntryMoveCoroutine);
        }
        _personalEntryMoveCoroutine = StartCoroutine(MoveMenuCoroutine(_personalEntryPanel, new Vector2(_personalEntryPanel.anchoredPosition.x, xPos)));
    }

    // Animation for the submenu moving out
    private IEnumerator MoveMenuCoroutine(RectTransform rectTransform, Vector2 anchoredPosition)
    {
        const float duration = 0.25f;
        var time = 0f;
        var startPosition = rectTransform.anchoredPosition;
        while (time < duration)
        {
            time += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, anchoredPosition, time / duration);
            yield return null;
        }

        rectTransform.anchoredPosition = anchoredPosition;
        _personalEntryMoveCoroutine = null;
    }

    // Small animation to signify that the leaderboard is still loading
    private IEnumerator LoadingTextCoroutine(TMP_Text text)
    {
        var loadingText = "Loading";
        for (int i = 0; i < 3; i++)
        {
            loadingText += ".";
            text.text = loadingText;
            yield return new WaitForSeconds(0.25f);
        }

        StartCoroutine(LoadingTextCoroutine(text));
    }

    // Initialise UI listeners and show the loading screen while everything loads in the load function
    private void InitializeComponents()
    {
        StartCoroutine(LoadingTextCoroutine(_leaderboardLoadingPanel.GetComponentInChildren<TextMeshProUGUI>()));

        _pageInput.onValueChanged.AddListener(_ => _pageInput.image.color = Color.yellow);
        _entriesToTakeInput.onValueChanged.AddListener(_ => _entriesToTakeInput.image.color = Color.yellow);

        _pageInput.placeholder.GetComponent<TextMeshProUGUI>().text = _defaultPageNumber.ToString();
        _entriesToTakeInput.placeholder.GetComponent<TextMeshProUGUI>().text = _defaultEntriesToTake.ToString();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private IEnumerator Start()
    {
        // Setup everything, load entries
        InitializeComponents();
        Load();
        
        Debug.Log(PlayerPrefs.GetString("PlayerName"));
        //PlayerPrefs.DeleteKey("PlayerName");
        // Check that there is a username for the player
        if (PlayerPrefs.GetString("PlayerName") == null || PlayerPrefs.GetString("PlayerName") == "")
        {
            // If there is no username, generate a random one
            yield return (StartCoroutine(GenerateRandomPlayerName()));
        }
        Debug.Log(PlayerPrefs.GetString("PlayerName"));
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }

    // Upload a new score
    public void Submit(string playerUsername, int score)
    {
        Leaderboards.Mobile.UploadNewEntry(playerUsername, score, Callback, ErrorCallback);
    }

    // Delete the player's entry
    public void DeleteEntry()
    {
        Leaderboards.Mobile.DeleteEntry(Callback, ErrorCallback);
    }


    // Completely remove the player's entry from the leaderboard and reset name
    public void ResetPlayer()
    {
        LeaderboardCreator.ResetPlayer();
        GenerateRandomPlayerName();
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }

    // Set new name to whatever the player chooses, save it
    public void ChangePlayerName()
    {
        string playerName = playerNameInputText.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }

    // Request the personal entry of the player, callback to OnPersonalEntryLoaded if successful
    public void GetPersonalEntry()
    {
        Leaderboards.Mobile.GetPersonalEntry(OnPersonalEntryLoaded, ErrorCallback);
    }

    // Set the text to the data provided, begin the submenu movement anim
    private void OnPersonalEntryLoaded(Entry entry)
    {
        _personalEntryText.text = $"{entry.RankSuffix()}. {entry.Username} : {entry.Score}";
        MovePersonalEntryMenu(0f);
    }

    // Default callback
    private void Callback(bool success)
    {
        if (success)
        {
            Load();
        }
    }

    // Callback for if there is an error with anything
    private void ErrorCallback(string error)
    {
        Debug.LogError(error);
    }

    // Make a new name for the player
    private IEnumerator GenerateRandomPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", ("Player#" + UnityEngine.Random.Range(1000, 9999)));
        yield return null;
    }

    // Get the player's name from the playerprefs
    public void LoadPlayername()
    {
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }
}

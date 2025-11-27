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

    public void Load()
    {
        var timePeriod =
            _timePeriodDropdown.value == 1 ? Dan.Enums.TimePeriodType.Today :
            _timePeriodDropdown.value == 2 ? Dan.Enums.TimePeriodType.ThisWeek :
            _timePeriodDropdown.value == 3 ? Dan.Enums.TimePeriodType.ThisMonth :
            _timePeriodDropdown.value == 4 ? Dan.Enums.TimePeriodType.ThisYear : Dan.Enums.TimePeriodType.AllTime;

        var pageNumber = int.TryParse(_pageInput.text, out var pageValue) ? pageValue : _defaultPageNumber;
        pageNumber = Mathf.Max(1, pageNumber);
        _pageInput.text = pageNumber.ToString();

        var take = int.TryParse(_entriesToTakeInput.text, out var takeValue) ? takeValue : _defaultEntriesToTake;
        take = Mathf.Clamp(take, 1, 100);
        _entriesToTakeInput.text = take.ToString();

        var searchQuery = new LeaderboardSearchQuery
        {
            Skip = (pageNumber - 1) * take,
            Take = take,
            TimePeriod = timePeriod
        };

        _pageInput.image.color = Color.white;
        _entriesToTakeInput.image.color = Color.white;

        Leaderboards.Mobile.GetEntries(searchQuery, OnLeaderboardLoaded, ErrorCallback);
        ToggleLoadingPanel(true);
    }

    public void ChangePageBy(int amount)
    {
        var pageNumber = int.TryParse(_pageInput.text, out var pageValue) ? pageValue : _defaultPageNumber;
        pageNumber += amount;
        if (pageNumber < 1) return;
        _pageInput.text = pageNumber.ToString();

        Load();
    }

    private void OnLeaderboardLoaded(Entry[] entries)
    {
        foreach (Transform t in _entryDisplayParent)
            Destroy(t.gameObject);

        foreach (var t in entries)
            CreateEntryDisplay(t);

        ToggleLoadingPanel(false);
    }

    private void ToggleLoadingPanel(bool isOn)
    {
        _leaderboardLoadingPanel.alpha = isOn ? 1f : 0f;
        _leaderboardLoadingPanel.interactable = isOn;
        _leaderboardLoadingPanel.blocksRaycasts = isOn;
    }

    public void MovePersonalEntryMenu(float xPos)
    {
        if (_personalEntryMoveCoroutine != null)
            StopCoroutine(_personalEntryMoveCoroutine);
        _personalEntryMoveCoroutine = StartCoroutine(MoveMenuCoroutine(_personalEntryPanel,
            new Vector2(xPos, _personalEntryPanel.anchoredPosition.y)));
    }

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

    private void CreateEntryDisplay(Entry entry)
    {
        var entryDisplay = Instantiate(_entryDisplayPrefab.gameObject, _entryDisplayParent);
        entryDisplay.GetComponent<EntryDisplay>().SetEntry(entry);
    }

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
        InitializeComponents();
        Load();
        
        Debug.Log(PlayerPrefs.GetString("PlayerName"));
        //PlayerPrefs.DeleteKey("PlayerName");
        if (PlayerPrefs.GetString("PlayerName") == null)
        {
            yield return (StartCoroutine(GenerateRandomPlayerName()));
        }
        Debug.Log(PlayerPrefs.GetString("PlayerName"));
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }

    public void Submit(string playerUsername, int score)
    {
        Leaderboards.Mobile.UploadNewEntry(playerUsername, score, null, ErrorCallback);
    }

    public void DeleteEntry()
    {
        Leaderboards.Mobile.DeleteEntry(Callback, ErrorCallback);
    }

    public void ResetPlayer()
    {
        LeaderboardCreator.ResetPlayer();
        GenerateRandomPlayerName();
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }

    public void ChangePlayerName()
    {
        string playerName = playerNameInputText.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }

    public void GetPersonalEntry()
    {
        Leaderboards.Mobile.GetPersonalEntry(OnPersonalEntryLoaded, ErrorCallback);
    }

    private void OnPersonalEntryLoaded(Entry entry)
    {
        _personalEntryText.text = $"{entry.RankSuffix()}. {entry.Username} : {entry.Score}";
        MovePersonalEntryMenu(0f);
    }

    private void Callback(bool success)
    {
        if (success)
        {
            Load();
        }
    }

    private void ErrorCallback(string error)
    {
        Debug.LogError(error);
    }

    private IEnumerator GenerateRandomPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", ("Player#" + UnityEngine.Random.Range(1000, 9999)));
        return null;
    }

    public void LoadPlayername()
    {
        PlayerNameInputPlaceholder.text = PlayerPrefs.GetString("PlayerName");
    }
}

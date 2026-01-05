using System;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherData : MonoBehaviour
{
    public static WeatherData Instance { get; private set; }

    [Header("Settings")]

    public float updateCallDelay = 600.0f;
    public string OWAPIKey;

    [Header("References")]

    public TextMeshProUGUI currentWeatherText;

    [Header("Info")]

    public float latitude;
    public float longitude;
    public string cityName;
    public string currentWeather;
    public WeatherType currentWeatherType = WeatherType.clear;

    private string IPAddress;
    public float callTimer;
    private bool isLocationInitialized = false;

    public enum WeatherType
    {
        clear,
        cloudy,
        rain
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        StartCoroutine(GetIP());
    }

    // Update is called once per frame
    void Update()
    {
        // Wait until APIs fetch the required data needed for openweather to get the right data
        if (!isLocationInitialized)
        {
            return;
        }

        // Update the weather every hour (only have something like 1000 requests free per day)
        // !! CHANGE THIS TO DATETIME STUFF !!
        // Doesn't really work the intended way currently lmao.
        if (callTimer <= 0)
        {
            StartCoroutine(GetWeatherInfo());
            callTimer = updateCallDelay;
        }
        else
        {
            callTimer -= Time.deltaTime;
        }
    }

    // Get the player's IP in order to request lat / lon values for openweather
    private IEnumerator GetIP()
    {
        // Send request to website
        var www = new UnityWebRequest("https://api.ipify.org")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        // Check for if there was an error requesting the web address
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("IP request failed: " + www.error);
            yield break;
        }

        // If reached this point, IP is obtained and can move to accessing lat / lon for city location
        IPAddress = www.downloadHandler.text;
        StartCoroutine(GetLatAndLon());
    }

    // Get latitude and longitude coordinates
    // !! REALISED THAT IPAPI CAN GET THE IP ADDRESS, MAYBE REWORK THIS AND REMOVE IPIFY??? !!
    private IEnumerator GetLatAndLon()
    {
        // Send request to website
        var www = new UnityWebRequest("https://ipapi.co/" + IPAddress + "/json/")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        // I don't know why this is needed or why the request decided to stop randomly without it but it took me too long to find how to fix it and I'm not gonna question it
        www.SetRequestHeader("User-Agent", "Unity3D");

        yield return www.SendWebRequest();

        // Check for if there was an error requesting the web address
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Coordinate request failed: " + www.error);
            yield break;
        }

        // Now have everything needed for openweather, set a bunch of variables
        var locationData = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
        latitude = locationData.latitude;
        longitude = locationData.longitude;
        cityName = locationData.city;

        // Variables set to allow openweather to run
        isLocationInitialized = true;
        callTimer = 0;
        Debug.Log($"Location found {cityName}, {latitude}, {longitude}");
    }

    // Get the weather from openweather
    private IEnumerator GetWeatherInfo()
    {
        // Loooong url string
        UnityWebRequest www = UnityWebRequest.Get("https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=" + OWAPIKey + "&units=metric");
        yield return www.SendWebRequest();

        // Check for if there is an error
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Weather request failed: " + www.error);
            yield break;
        }

        // FINALLY have the weather
        var weather = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);

        // Get current weather from JSON
        currentWeather = weather.weather[0].main;

        // Split the main weather description into one of 3 different enum types
        if (currentWeather == "Rain" || currentWeather == "Drizzle" || currentWeather == "Thunderstorm")
        {
            currentWeatherType = WeatherType.rain;
        }
        else if (currentWeather == "Clouds")
        {
            currentWeatherType = WeatherType.cloudy;
        }
        else
        {
            currentWeatherType = WeatherType.clear;
        }

        // Update the text!!
        string display = $"{cityName}: {currentWeather}";
        Debug.Log(display);
        if (currentWeatherText != null)
        {
            currentWeatherText.text = display;
        }
    }
}

// Buncha junk for the API info
[Serializable]
public class LocationInfo
{
    public string city;
    public float latitude;
    public float longitude;
}

[Serializable]
public class WeatherInfo
{
    public WeatherCondition[] weather;
}

[Serializable]
public class WeatherCondition
{
    public string main;
}
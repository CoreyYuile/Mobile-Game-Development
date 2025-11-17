using System;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherData : MonoBehaviour
{

    [Header("Settings")]

    public float updateMinuteDelay = 10f;
    public string OWAPIKey;

    [Header("References")]

    public TextMeshProUGUI currentWeatherText;

    [Header("Info")]

    public float latitude;
    public float longitude;
    public string cityName;
    public string currentWeather;

    private string IPAddress;
    public float timer;
    private bool isLocationInitialized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetIP());
    }

    // Update is called once per frame
    void Update()
    {
        // Wait until APIs fetch the required data for openweather to operate
        if (!isLocationInitialized) return;

        // Update the weather every hour (only have something like 1000 requests free per day)
        // !! CHANGE THIS TO DATETIME STUFF !!
        // Doesn't really work the intended way currently lmao.
        if (timer <= 0)
        {
            StartCoroutine(GetWeatherInfo());
            timer = updateMinuteDelay * 60;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // Get the player's IP in order to request lat / lon values for openweather
    private IEnumerator GetIP()
    {
        // Send request to website
        var www = new UnityWebRequest("https://api.ipify.org?format=text")
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
        StartCoroutine(GetCoordinates());
    }

    // Get latitude and longitude coordinates
    private IEnumerator GetCoordinates()
    {
        // Send request to website
        var www = new UnityWebRequest("https://ipapi.co/" + IPAddress + "/json/")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
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
        isLocationInitialized = true;
        timer = 0;
        Debug.Log($"Location found: {cityName} ({latitude}, {longitude})");
    }

    // Get the weather from openweather
    private IEnumerator GetWeatherInfo()
    {
        // Loooong url string
        UnityWebRequest www = UnityWebRequest.Get("https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=" + OWAPIKey + "&units=metric");
        yield return www.SendWebRequest();

        // Check for error
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Weather request failed: " + www.error);
            yield break;
        }

        // FINALLY have the weather
        var weather = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);

        // Get current weather from JSON
        currentWeather = weather.weather[0].description;

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
    public string description;
}
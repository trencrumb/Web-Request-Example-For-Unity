using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherRequest : MonoBehaviour
{
    #region Singleton Pattern

    // Singleton instance of the WeatherRequest class
    public static WeatherRequest Instance { get; set; }

    // Awake method to enforce singleton pattern
    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy this instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        // Otherwise, set this as the instance
        Instance = this;
    }

    #endregion
    
    // Serialized fields for latitude, longitude, and time interval
    [SerializeField] private float latitude = 51.5114f;
    [SerializeField] private float longitude = 0.2376f;
    [Range(9, 120f)]
    [Tooltip("Time interval between requests sent to api in seconds")]
    [SerializeField] private float timeInterval = 10f;

    // Base URL for the weather API
    private string baseUrl = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";
    // Query parameters for the API
    private string unitGroup = "metric";
    private string apiKey = "{PUT_YOUR_API_KEY_HERE}";
    private string contentType = "json";

    // Start method called on the object's initialization
    void Start()
    {
        // Start the coroutine to get weather data periodically
        StartCoroutine(GetWeatherData());
    }

    // Coroutine to get weather data from the API
    private IEnumerator GetWeatherData()
    {
        while (true)
        {
            // Construct the URL with the coordinates and query parameters
            string coordinates = $"{latitude},{longitude}";
            string url = $"{baseUrl}{coordinates}?unitGroup={unitGroup}&key={apiKey}&contentType={contentType}";

            // Create the web request
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check for errors in the web request
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Successfully received a response
                string jsonResponse = webRequest.downloadHandler.text;
                try
                {
                    // Deserialize the JSON response to a WeatherResponse object
                    WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);
                    // Update the WeatherManager with the new response
                    WeatherManager.UpdateWeatherResponse(weatherResponse);
                    // Log some information from the response
                    Debug.Log($"lat: {weatherResponse.latitude} long: {weatherResponse.longitude}");
                    Debug.Log($"Wind Speed: {weatherResponse.currentConditions.windspeed}");
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON Error: " + jsonEx.Message);
                }
            }

            // Wait for the specified time interval before making the next request
            yield return new WaitForSeconds(timeInterval);
        }
    }
}

// Static class to manage the current weather response
public static class WeatherManager
{
    // Property to hold the current weather response
    public static WeatherResponse CurrentResponse { get; private set; }

    // Method to update the current weather response
    public static void UpdateWeatherResponse(WeatherResponse newWeatherResponse)
    {
        CurrentResponse = newWeatherResponse;
    }
}

#region WeatherResponseClasses

// Class to represent the weather response from the API
public class WeatherResponse
{
    public double? queryCost { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public string resolvedAddress { get; set; }
    public string address { get; set; }
    public string timezone { get; set; }
    public double? tzoffset { get; set; }
    public string description { get; set; }
    public List<Day> days { get; set; }
    public List<object> alerts { get; set; }
    public Stations stations { get; set; }
    public CurrentConditions currentConditions { get; set; }
}

// Various classes representing different parts of the weather response

public class AR120
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class CurrentConditions
{
    public string datetime { get; set; }
    public double? datetimeEpoch { get; set; }
    public double? temp { get; set; }
    public double? feelslike { get; set; }
    public double? humidity { get; set; }
    public double? dew { get; set; }
    public double? precip { get; set; }
    public double? precipprob { get; set; }
    public double? snow { get; set; }
    public double? snowdepth { get; set; }
    public string preciptype { get; set; }
    public double? windgust { get; set; }
    public double? windspeed { get; set; }
    public double? winddir { get; set; }
    public double? pressure { get; set; }
    public double? visibility { get; set; }
    public double? cloudcover { get; set; }
    public double? solarradiation { get; set; }
    public double? solarenergy { get; set; }
    public double? uvindex { get; set; }
    public string conditions { get; set; }
    public string icon { get; set; }
    public List<string> stations { get; set; }
    public string source { get; set; }
    public string sunrise { get; set; }
    public double? sunriseEpoch { get; set; }
    public string sunset { get; set; }
    public double? sunsetEpoch { get; set; }
    public double? moonphase { get; set; }
}

public class D5621
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class Day
{
    public string datetime { get; set; }
    public double? datetimeEpoch { get; set; }
    public double? tempmax { get; set; }
    public double? tempmin { get; set; }
    public double? temp { get; set; }
    public double? feelslikemax { get; set; }
    public double? feelslikemin { get; set; }
    public double? feelslike { get; set; }
    public double? dew { get; set; }
    public double? humidity { get; set; }
    public double? precip { get; set; }
    public double? precipprob { get; set; }
    public double? precipcover { get; set; }
    public List<string> preciptype { get; set; }
    public double? snow { get; set; }
    public double? snowdepth { get; set; }
    public double? windgust { get; set; }
    public double? windspeed { get; set; }
    public double? winddir { get; set; }
    public double? pressure { get; set; }
    public double? cloudcover { get; set; }
    public double? visibility { get; set; }
    public double? solarradiation { get; set; }
    public double? solarenergy { get; set; }
    public double? uvindex { get; set; }
    public double? severerisk { get; set; }
    public string sunrise { get; set; }
    public double? sunriseEpoch { get; set; }
    public string sunset { get; set; }
    public double? sunsetEpoch { get; set; }
    public double? moonphase { get; set; }
    public string conditions { get; set; }
    public string description { get; set; }
    public string icon { get; set; }
    public List<string> stations { get; set; }
    public string source { get; set; }
    public List<Hour> hours { get; set; }
}

public class EGKB
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class EGKK
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class EGLC
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class EGLL
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class EGMC
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class EGSS
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class EGWU
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class F6665
{
    public double? distance { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public double? useCount { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public double? quality { get; set; }
    public double? contribution { get; set; }
}

public class Hour
{
    public string datetime { get; set; }
    public double? datetimeEpoch { get; set; }
    public double? temp { get; set; }
    public double? feelslike { get; set; }
    public double? humidity { get; set; }
    public double? dew { get; set; }
    public double? precip { get; set; }
    public double? precipprob { get; set; }
    public double? snow { get; set; }
    public double? snowdepth { get; set; }
    public List<string> preciptype { get; set; }
    public double? windgust { get; set; }
    public double? windspeed { get; set; }
    public double? winddir { get; set; }
    public double? pressure { get; set; }
    public double? visibility { get; set; }
    public double? cloudcover { get; set; }
    public double? solarradiation { get; set; }
    public double? solarenergy { get; set; }
    public double? uvindex { get; set; }
    public double? severerisk { get; set; }
    public string conditions { get; set; }
    public string icon { get; set; }
    public List<string> stations { get; set; }
    public string source { get; set; }
}

public class Stations
{
    public EGWU EGWU { get; set; }
    public EGSS EGSS { get; set; }
    public EGLL EGLL { get; set; }
    public EGKK EGKK { get; set; }
    public D5621 D5621 { get; set; }
    public F6665 F6665 { get; set; }
    public AR120 AR120 { get; set; }
    public EGMC EGMC { get; set; }
    public EGLC EGLC { get; set; }
    public EGKB EGKB { get; set; }
}

#endregion

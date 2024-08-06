using System;
using UnityEngine;

public class test : MonoBehaviour
{
    
    void Update()
    {
        // Get the weather response and store in a variable
        WeatherResponse weather = WeatherManager.CurrentResponse;
        // first check to see if we have filled the value from the WeatherRequestScript.
        if (weather != null)
        {
            var cloudCover = weather.currentConditions.cloudcover;
            var windSpeed = weather.currentConditions.windspeed;
            var rain = weather.currentConditions.precip;
            var rainProbability = weather.currentConditions.precipprob;
            // issues with this one
            var rainType = weather.currentConditions.preciptype;
            var moonPhase = weather.currentConditions.moonphase;
            
            // Example epoch timestamp in seconds for time until the sunset
            double epochSeconds = (double)weather.currentConditions.sunsetEpoch;
            // Convert epoch to human-readable date and time
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)epochSeconds);
            DateTime dateTime = dateTimeOffset.LocalDateTime; // Converts to local time
            var timeSpan = DateTime.Now.Subtract(dateTime);
            
            print($"Human-readable date and time (local): {timeSpan}");

            print($@"CloudCover: {cloudCover}
                WindSpeed: {windSpeed}
                Rain: {rain}
                RainProbability: {rainProbability}
                RainType: {rainType}
                MoonPhase: {moonPhase}");
            
        }
    }   
}
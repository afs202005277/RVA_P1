using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class WeatherService : MonoBehaviour
{
    public enum WeatherCondition
    {
        Rain,
        Clear
    }
    public enum DayState
    {
        Day,
        Night
    }

    struct GeolocationData
    {
        [JsonProperty("geoplugin_request")] public string Request { get; set; }
        [JsonProperty("geoplugin_status")] public int Status { get; set; }
        [JsonProperty("geoplugin_delay")] public string Delay { get; set; }
        [JsonProperty("geoplugin_credit")] public string Credit { get; set; }
        [JsonProperty("geoplugin_city")] public string City { get; set; }
        [JsonProperty("geoplugin_region")] public string Region { get; set; }
        [JsonProperty("geoplugin_regionCode")] public string RegionCode { get; set; }
        [JsonProperty("geoplugin_regionName")] public string RegionName { get; set; }
        [JsonProperty("geoplugin_areaCode")] public string AreaCode { get; set; }
        [JsonProperty("geoplugin_dmaCode")] public string DMACode { get; set; }
        [JsonProperty("geoplugin_countryCode")] public string CountryCode { get; set; }
        [JsonProperty("geoplugin_countryName")] public string CountryName { get; set; }
        [JsonProperty("geoplugin_inEU")] public int InEU { get; set; }
        [JsonProperty("geoplugin_euVATrate")] public bool EUVATRate { get; set; }
        [JsonProperty("geoplugin_continentCode")] public string ContinentCode { get; set; }
        [JsonProperty("geoplugin_continentName")] public string ContinentName { get; set; }
        [JsonProperty("geoplugin_latitude")] public string Latitude { get; set; }
        [JsonProperty("geoplugin_longitude")] public string Longitude { get; set; }
        [JsonProperty("geoplugin_locationAccuracyRadius")] public string LocationAccuracyRadius { get; set; }
        [JsonProperty("geoplugin_timezone")] public string TimeZone { get; set; }
        [JsonProperty("geoplugin_currencyCode")] public string CurrencyCode { get; set; }
        [JsonProperty("geoplugin_currencySymbol")] public string CurrencySymbol { get; set; }
        [JsonProperty("geoplugin_currencySymbol_UTF8")] public string CurrencySymbolUTF8 { get; set; }
        [JsonProperty("geoplugin_currencyConverter")] public double CurrencyConverter { get; set; }
    }
    struct WeatherData
    {
        [JsonProperty("coord")] public OpenWeather_Coordinates Location { get; set; }
        [JsonProperty("weather")] public List<OpenWeather_Condition> WeatherConditions { get; set; }
        [JsonProperty("base")] public string Internal_Base { get; set; }
        [JsonProperty("main")] public OpenWeather_KeyInfo KeyInfo { get; set; }
        [JsonProperty("visibility")] public int Visibility { get; set; }
        [JsonProperty("wind")] public OpenWeather_Wind Wind { get; set; }
        [JsonProperty("clouds")] public OpenWeather_Clouds Clouds { get; set; }
        [JsonProperty("rain")] public OpenWeather_Rain Rain { get; set; }
        [JsonProperty("snow")] public OpenWeather_Snow Snow { get; set; }
        [JsonProperty("dt")] public int TimeOfCalculation { get; set; }
        [JsonProperty("sys")] public OpenWeather_Internal Internal_Sys { get; set; }
        [JsonProperty("timezone")] public int Timezone { get; set; }
        [JsonProperty("id")] public int CityID { get; set; }
        [JsonProperty("name")] public string CityName { get; set; }
        [JsonProperty("cod")] public int Internal_COD { get; set; }
    }
    struct OpenWeather_Coordinates
    {
        [JsonProperty("lon")] public double Longitude { get; set; }
        [JsonProperty("lat")] public double Latitude { get; set; }
    }
    // Condition Info: https://openweathermap.org/weather-conditions
    struct OpenWeather_Condition
    {
        [JsonProperty("id")] public int ConditionID { get; set; }
        [JsonProperty("main")] public string Group { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
    }
    struct OpenWeather_KeyInfo
    {
        [JsonProperty("temp")] public double Temperature { get; set; }
        [JsonProperty("feels_like")] public double Temperature_FeelsLike { get; set; }
        [JsonProperty("temp_min")] public double Temperature_Minimum { get; set; }
        [JsonProperty("temp_max")] public double Temperature_Maximum { get; set; }
        [JsonProperty("pressure")] public int Pressure { get; set; }
        [JsonProperty("sea_level")] public int PressureAtSeaLevel { get; set; }
        [JsonProperty("grnd_level")] public int PressureAtGroundLevel { get; set; }
        [JsonProperty("humidity")] public int Humidity { get; set; }
    }
    struct OpenWeather_Wind
    {
        [JsonProperty("speed")] public double Speed { get; set; }
        [JsonProperty("deg")] public int Direction { get; set; }
        [JsonProperty("gust")] public double Gust { get; set; }
    }
    struct OpenWeather_Clouds
    {
        [JsonProperty("all")] public int Cloudiness { get; set; }
    }
    struct OpenWeather_Rain
    {
        [JsonProperty("1h")] public float VolumeInLastHour { get; set; }
        [JsonProperty("3h")] public float VolumeInLast3Hours { get; set; }
    }
    struct OpenWeather_Snow
    {
        [JsonProperty("1h")] public int VolumeInLastHour { get; set; }
        [JsonProperty("3h")] public int VolumeInLast3Hours { get; set; }
    }
    struct OpenWeather_Internal
    {
        [JsonProperty("type")] public int Internal_Type { get; set; }
        [JsonProperty("id")] public int Internal_ID { get; set; }
        [JsonProperty("message")] public double Internal_Message { get; set; }
        [JsonProperty("country")] public string CountryCode { get; set; }
        [JsonProperty("sunrise")] public int SunriseTime { get; set; }
        [JsonProperty("sunset")] public int SunsetTime { get; set; }
    }

    public int requestTimeoutSeconds = 1;
    public double HotTemperatureThreshold = 30.0;

    const string OpenWeatherAPIKey = "12e41b48270751f2ac2ea183c7051b0d";
    const string URL_GetPublicIpAddress = "https://api.ipify.org";
    const string URL_GetGeolocationData = "http://www.geoplugin.net/json.gp?ip=";
    const string URL_GetWeatherData = "https://api.openweathermap.org/data/2.5/weather?";
    
    string PlayerIpAddress;
    GeolocationData PlayerGeolocationData;
    WeatherData PlayerWeatherData;

    public WeatherCondition GetWeatherCondition()
    {
        return PlayerWeatherData.WeatherConditions[0].Group switch
        {
            "Rain" or "Drizzle" or "Thunderstorm" => WeatherCondition.Rain,
            _ => WeatherCondition.Clear,
        };
    }

    public DayState GetDayState()
    {
        DateTimeOffset sunriseTime = DateTimeOffset.FromUnixTimeSeconds(PlayerWeatherData.Internal_Sys.SunriseTime);
        DateTimeOffset sunsetTime = DateTimeOffset.FromUnixTimeSeconds(PlayerWeatherData.Internal_Sys.SunsetTime);
        DateTime currentTime = System.DateTime.UtcNow;
        
        if (currentTime > sunriseTime && currentTime < sunsetTime)
        {
            return DayState.Day;
        }
        else
        {
            return DayState.Night;
        }
    }

    public double GetCurrentTemperature()
    {
        return PlayerWeatherData.KeyInfo.Temperature;
    }

    public bool IsRaining()
    {
        return GetWeatherCondition() == WeatherCondition.Rain;
    }

    public bool IsNight()
    {
        return GetDayState() == DayState.Night;
    }

    public bool IsHot()
    {
        return GetCurrentTemperature() > HotTemperatureThreshold;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator Init()
    {
        if (string.IsNullOrEmpty(OpenWeatherAPIKey))
        {
            Debug.LogError("No API key set for https://openweathermap.org/");  
        } else
        {
            yield return StartCoroutine(FetchPlayerIpAddress());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FetchPlayerIpAddress()
    {
        // Attempt to retrieve player's IP address
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetPublicIpAddress + PlayerIpAddress))
        {
            request.timeout = requestTimeoutSeconds;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                PlayerIpAddress = request.downloadHandler.text.Trim();
                yield return StartCoroutine(FetchPlayerGeolocationData());
            }
            else
            {
                Debug.LogError($"Failed to get player's IP: {request.downloadHandler.text}");
            }
        }
    }

    IEnumerator FetchPlayerGeolocationData()
    {
        // Attempt to retrieve player's geolocation data
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetGeolocationData))
        {
            request.timeout = requestTimeoutSeconds;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                PlayerGeolocationData = JsonConvert.DeserializeObject<GeolocationData>(request.downloadHandler.text);
                yield return StartCoroutine(FetchWeatherData());
            }
            else
            {
                Debug.LogError($"Failed to get player's geolocation data: {request.downloadHandler.text}");
            }
        }
    }

    IEnumerator FetchWeatherData()
    {
        string weatherURL = URL_GetWeatherData;
        weatherURL += $"lat={PlayerGeolocationData.Latitude}";
        weatherURL += $"&lon={PlayerGeolocationData.Longitude}";
        weatherURL += $"&appid={OpenWeatherAPIKey}";
        weatherURL += "&units=metric"; // To return temperature in Celsius

        // Attempt to retrieve weather data
        using (UnityWebRequest request = UnityWebRequest.Get(weatherURL))
        {
            request.timeout = requestTimeoutSeconds;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                PlayerWeatherData = JsonConvert.DeserializeObject<WeatherData>(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"Failed to get player's weather data: {request.downloadHandler.text}");
            }
        }
    }
}

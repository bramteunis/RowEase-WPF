using Newtonsoft.Json;
using Models;

namespace Forecast
{
    public class GetForecast
    {
        public async Task<ApiResponse> LoadWeather()
        {
            // Connects to API
            string apiURL = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Zwolle/last2days?unitGroup=metric&key=S8EY27U9AHWSRV6LUB7PHJAWL&contentType=json";
            
            var client = new HttpClient();

            var requestToAPI = new HttpRequestMessage(HttpMethod.Get, apiURL);

            var responseFromAPI = await client.SendAsync(requestToAPI);
            responseFromAPI.EnsureSuccessStatusCode(); // Throw an exception if error
            
            string apiBody = await responseFromAPI.Content.ReadAsStringAsync(); // From the URL query code above 
            
            ApiResponse weatherData = JsonConvert.DeserializeObject<ApiResponse>(apiBody);
            
            return weatherData;
        }
    }
}
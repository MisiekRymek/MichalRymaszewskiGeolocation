using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MichalRymaszewskiGeolocation.Models;

namespace MichalRymaszewskiGeolocation.Services
{
    public class IpStackResponse
    {
        public string ip { get; set; }
        public string country_name { get; set; }
        public string city { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class IpStackGeoService : IGeoService
    {
        private readonly string _apiKey;

        public IpStackGeoService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<Result<GeoLocation>> GetGeoLocationAsync(string ipOrUrl)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync(
                    $"http://api.ipstack.com/{ipOrUrl}?access_key={_apiKey}"
                );

                var json = JsonConvert.DeserializeObject<IpStackResponse>(response);

                if (json == null)
                    return Result<GeoLocation>.Fail("Empty response from API.");

                var geo = new GeoLocation
                {
                    IpOrUrl = ipOrUrl,
                    Country = json.country_name,
                    City = json.city,
                    Latitude = json.latitude,
                    Longitude = json.longitude,
                    RetrievedAt = DateTime.UtcNow
                };

                return Result<GeoLocation>.Ok(geo);
            }
            catch (Exception ex)
            {
                return Result<GeoLocation>.Fail(ex.Message);
            }
        }
    }
}

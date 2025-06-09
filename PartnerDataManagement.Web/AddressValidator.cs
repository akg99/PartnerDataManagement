using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Engine;
using GoogleMapsApi;

namespace PartnerDataManagement.Web
{
    
    public class AddressValidator
    {
        private readonly string _apiKey = "YOUR_GOOGLE_API_KEY";

        public async Task<GeocodingResponse> ValidateAddress(string address)
        {
            var request = new GeocodingRequest
            {
                Address = address,
                ApiKey = _apiKey
            };

            var response = await GoogleMaps.Geocode.QueryAsync(request);

            var coordinates = response.Results.FirstOrDefault()?.Geometry.Location;
            Console.WriteLine($"Latitude: {coordinates.Latitude}, Longitude: {coordinates.Longitude}");
            var formattedAddress = response.Results.FirstOrDefault()?.FormattedAddress;
            Console.WriteLine($"Formatted Address: {formattedAddress}");

            return response;
        }
    }
}

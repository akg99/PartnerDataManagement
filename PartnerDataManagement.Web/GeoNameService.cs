using NGeoNames;
using NGeoNames.Entities;
using System;
using System.Linq;

namespace PartnerDataManagement.Web
{

    public class GeoNameService
    {
        public static void GetNearbyLocations(string countryCode)
        {
            var geoData = GeoFileReader.ReadExtendedGeoNames($"./{countryCode}.txt").ToArray();
            var referencePoint = geoData.FirstOrDefault(n => n.Name == "Amsterdam");

            var reverseGeocoder = new ReverseGeoCode<ExtendedGeoName>(geoData);
            var results = reverseGeocoder.RadialSearch(referencePoint, 250);

            foreach (var location in results)
            {
                Console.WriteLine($"{location.Name}, {location.Latitude}, {location.Longitude}");
            }
        }
    }
}

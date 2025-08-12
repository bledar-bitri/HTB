using AddressAndDistanceLookup;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private const string ApiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6ImU5NjRhZDQ3MWQ2NTRjYzg5MTY1ZGFkMTc0MTgxOTYyIiwiaCI6Im11cm11cjY0In0=";
    private const string DrivingUrl = "https://api.openrouteservice.org/v2/directions/driving-car";
    private const string GeoCodeUrl = "https://nominatim.openstreetmap.org/search";

    private static readonly List<string> Addresses = new List<string>()
    {
        "Greith 15, 4893, Zell am Moos, Austria",
        "Am Teichfeld 2, 4152 Sarleinsbach, Austria",
        "Waidhofnerstr. 44, 3333 Böhlerwerk, Austria",
        "Gleinker Hauptstraße 2A, 4407 Steyr Gleink, Austria",
        "Viktor Adler Weg 12, 4030 Linz, Austria",
        "Arbeiterheimstraße 30, 4662 LAAKIRCHEN, Austria",
        "Neuhauserweg 14, 4061 Pasching, Austria"
    };

    static async Task Main(string[] args)
    {
        await PrintCoordinatesAsync();
    }

    static async Task Mainx(string[] args)
    {
        Console.Write("Enter first address: ");
        string address1 = Console.ReadLine();

        Console.Write("Enter second address: ");
        string address2 = Console.ReadLine();

        var coord1 = await GetCoordinatesAsync(address1);
        var coord2 = await GetCoordinatesAsync(address2);

        if (coord1 == null || coord2 == null)
        {
            Console.WriteLine("Failed to geocode one or both addresses.");
            return;
        }

        Console.WriteLine($"Address 1: {coord1.Lat}, {coord1.Lon}");
        Console.WriteLine($"Address 2: {coord2.Lat}, {coord2.Lon}");

        double? distanceKm = await GetDrivingDistanceKmAsync(coord1, coord2);
        if (distanceKm == null)
        {
            Console.WriteLine("Failed to get route distance.");
        }
        else
        {
            Console.WriteLine($"Driving distance: {distanceKm:F2} km");
        }
    }

    public static async Task<GeoResult> GetCoordinatesAsync(string address)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0");

            var url = $"{GeoCodeUrl}?q={Uri.EscapeDataString(address)}&format=json&limit=1";
            Console.WriteLine(url);
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<GeoResult[]>(json);

            return results != null && results.Length > 0 ? results[0] : null;
        }
    }

    public static async Task<double?> GetDrivingDistanceKmAsync(GeoResult a, GeoResult b)
    {
        using (var client = new HttpClient())
        {
            string url =$"{DrivingUrl}?api_key={ApiKey}&start={a.Lon},{a.Lat}&end={b.Lon},{b.Lat}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Root>(json);
            return result?.features?[0]?.properties?.segments?[0]?.distance / 1000.0;
        }
    }


    public static async Task PrintCoordinatesAsync()
    {
        foreach (var address in Addresses)
        {
            Console.Write($"{address}: ");
            var coors = await GetCoordinatesAsync(address);
            if (coors != null)
                Console.WriteLine($"{coors.Lat}, {coors.Lon}");
            else
                Console.WriteLine("NF");
        }
    }
}

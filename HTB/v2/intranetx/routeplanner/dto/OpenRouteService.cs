using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AddressAndDistanceLookup
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Engine
    {
        public string version { get; set; }
        public DateTime build_date { get; set; }
        public DateTime graph_date { get; set; }
        public DateTime osm_date { get; set; }
    }

    public class Extras
    {
        public Roadaccessrestrictions roadaccessrestrictions { get; set; }
    }

    public class Feature
    {
        public List<double> bbox { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Geometry
    {
        public List<List<double>> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Metadata
    {
        public string attribution { get; set; }
        public string service { get; set; }
        public long timestamp { get; set; }
        public Query query { get; set; }
        public Engine engine { get; set; }
    }

    public class Properties
    {
        public List<Segment> segments { get; set; }
        public Extras extras { get; set; }
        public List<Warning> warnings { get; set; }
        public List<int> way_points { get; set; }
        public Summary summary { get; set; }
    }

    public class Query
    {
        public List<List<double>> coordinates { get; set; }
        public string profile { get; set; }
        public string profileName { get; set; }
        public string format { get; set; }
    }

    public class Roadaccessrestrictions
    {
        public List<List<int>> values { get; set; }
        public List<Summary> summary { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<double> bbox { get; set; }
        public List<Feature> features { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Segment
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public List<Step> steps { get; set; }
    }

    public class Step
    {
        public double distance { get; set; }
        public double duration { get; set; }
        public int type { get; set; }
        public string instruction { get; set; }
        public string name { get; set; }
        public List<int> way_points { get; set; }
    }

    public class Summary
    {
        public double value { get; set; }
        public double distance { get; set; }
        public double amount { get; set; }
        public double duration { get; set; }
    }

    public class Warning
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class GeoResult
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("lon")]
        public string Lon { get; set; }

        [JsonPropertyName("place_id")]
        public int PlaceId { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public int osm_id { get; set; }
        public string @class { get; set; }
        public string type { get; set; }
        public int place_rank { get; set; }
        public double importance { get; set; }
        public string addresstype { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public List<string> boundingbox { get; set; }
    }

}

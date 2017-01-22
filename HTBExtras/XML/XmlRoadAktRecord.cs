using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlRoadAktRecord : Record
    {
        public string Index { get; set; }
        public string Address { get; set; }
        public string Akt { get; set; }
        public string AktAZ { get; set; }
        public string Klient { get; set; }
        public string Gegner { get; set; }
        public string Distance { get; set; }
        public string Time { get; set; }
        public string TotalDistance { get; set; }
        public string TotalTime { get; set; }
        public string ExampleTime { get; set; }
        public string AktStatus { get; set; }
        public string AktCarInfo { get; set; }
        public string AktCarLicensePlate { get; set; }
        public string FieldmanName { get; set; }
        public int UserId{ get; set; }
        public bool AktHasActions { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}

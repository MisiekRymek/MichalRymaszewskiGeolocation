namespace MichalRymaszewskiGeolocation.Models
{
    public class GeoLocation
    {
        public int Id { get; set; }
        public string IpOrUrl { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime RetrievedAt { get; set; }
    }
}

namespace CallWebApi.Model
{
    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string Suite { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public Geo Geo { get; set; } = default!;

    }
}
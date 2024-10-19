using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CallWebApi.Model
{
    public class Address
    {
        [JsonPropertyName("street")]
        public string Street { get; set; } = default!;
        [JsonPropertyName("suite")]
        public string Suite { get; set; } = default!;
        [JsonPropertyName("city")]
        public string City { get; set; } = default!;
        [JsonPropertyName("zipcode")]
        public string ZipCode { get; set; }  = default!;
        [JsonPropertyName("geo")]
        public Geo Geo { get; set; } = default!;

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CallWebApi.Model
{
    public class Geo
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; } = default!;
        [JsonPropertyName("lng")]
        public string Lng { get; set; } = default!;
    }
}
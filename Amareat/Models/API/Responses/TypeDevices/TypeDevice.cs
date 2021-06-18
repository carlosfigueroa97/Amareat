using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.TypeDevices
{
    public class TypeDevice
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("__v")]
        public int __V { get; set; }
    }
}

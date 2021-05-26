using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Devices
{
    public class Device
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("idRoom")]
        public string IdRoom { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public bool Value { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}

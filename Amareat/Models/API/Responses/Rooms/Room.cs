using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Rooms
{
    public class Room
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}

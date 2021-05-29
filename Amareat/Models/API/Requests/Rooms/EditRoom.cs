using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Rooms
{
    public class EditRoom
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

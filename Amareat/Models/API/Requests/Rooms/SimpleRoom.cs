using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Rooms
{
    public class SimpleRoom
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

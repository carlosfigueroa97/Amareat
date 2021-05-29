using System;
using System.Collections.Generic;
using Amareat.Models.API.Requests.Rooms;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Buildings
{
    public class SaveBuilding
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rooms")]
        public List<SimpleRoom> Rooms { get; set; }
    }
}

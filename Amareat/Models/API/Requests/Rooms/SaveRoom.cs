using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Rooms
{
    public class SaveRoom
    {
        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

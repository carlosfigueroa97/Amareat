using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Buildings
{
    public class BuildingList
    {
        [JsonProperty("data")]
        public List<Building> Data { get; set; }
    }
}
using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Buildings
{
    public class BuildingData
    {
        [JsonProperty("data")]
        public Building Data { get; set; }
    }
}

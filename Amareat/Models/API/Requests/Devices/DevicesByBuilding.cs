using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Devices
{
    public class DevicesByBuilding
    {
        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

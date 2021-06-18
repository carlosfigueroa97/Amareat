using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Devices
{
    public class SaveDevice
    {
        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("idRoom")]
        public string IdRoom { get; set; }

        [JsonProperty("idTypeDevice")]
        public string IdTypeDevice { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.TypeDevices
{
    public class SaveTypeDevice
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

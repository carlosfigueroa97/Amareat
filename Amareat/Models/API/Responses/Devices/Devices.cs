using System;
using Amareat.Models.API.Responses.TypeDevices;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Devices
{
    public class Devices : Device
    {
        [JsonProperty("idTypeDevice")]
        public TypeDevice IdTypeDevices { get; set; }
    }
}

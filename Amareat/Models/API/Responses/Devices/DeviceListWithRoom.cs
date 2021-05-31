using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Devices
{
    public class DeviceListWithRoom
    {
        [JsonProperty("data")]
        public List<DeviceWithRoom> Data { get; set; }
    }
}

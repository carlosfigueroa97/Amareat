using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Devices
{
    public class DeviceList
    {
        [JsonProperty("data")]
        public List<Device> Data { get; set; }
    }
}

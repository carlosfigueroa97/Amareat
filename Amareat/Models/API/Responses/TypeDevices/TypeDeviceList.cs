using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.TypeDevices
{
    public class TypeDeviceList
    {
        [JsonProperty("data")]
        public List<TypeDevice> Data { get; set; }
    }
}

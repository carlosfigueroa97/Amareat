using System.Collections.Generic;
using Amareat.Models.API.Responses.Rooms;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Devices
{
    public class DeviceWithRoom : Device
    {
        [JsonProperty("room")]
        public Room Room { get; set; }

        [JsonProperty("devices")]
        public List<Devices> Devices { get; set; }
    }
}

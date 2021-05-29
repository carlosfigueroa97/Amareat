using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Rooms
{
    public class RoomList
    {
        [JsonProperty("data")]
        public List<Room> Data { get; set; }
    }
}

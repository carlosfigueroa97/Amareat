using System;
using Newtonsoft.Json;
using Amareat.Models.API.Responses.Users;
using Amareat.Models.API.Responses.Buildings;
using Amareat.Models.API.Responses.Rooms;
using Amareat.Models.API.Responses.Devices;

namespace Amareat.Models.API.Responses.History
{
    public class History
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("idUser")]
        public User IdUser { get; set; }

        [JsonProperty("idBuilding")]
        public Building IdBuilding { get; set; }

        [JsonProperty("idRoom")]
        public Room IdRoom { get; set; }

        [JsonProperty("idDevice")]
        public Device IdDevice { get; set; }

        [JsonProperty("change")]
        public bool Change { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}

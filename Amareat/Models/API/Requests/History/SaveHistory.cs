using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.History
{
    public class SaveHistory
    {
        [JsonProperty("idUser")]
        public string IdUser { get; set; }

        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("idRoom")]
        public string IdRoom { get; set; }

        [JsonProperty("idDevice")]
        public string IdDevice { get; set; }

        [JsonProperty("change")]
        public bool Change { get; set; }
    }
}

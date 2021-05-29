using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.History
{
    public class History
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

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

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}

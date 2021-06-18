using System;
using Amareat.Components.Base;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Devices
{
    public class Device : BaseVm
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("idBuilding")]
        public string IdBuilding { get; set; }

        [JsonProperty("idRoom")]
        public string IdRoom { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        private bool _value;
        [JsonProperty("value")]
        public bool Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}

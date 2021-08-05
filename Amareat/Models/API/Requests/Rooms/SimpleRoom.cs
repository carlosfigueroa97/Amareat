using System;
using Amareat.Components.Base;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Rooms
{
    public class SimpleRoom : BaseVm
    {
        private string _name;

        [JsonProperty("name")]
        public string Name 
        { 
            get => _name; 
            set => SetProperty(ref _name, value); 
        }
    }
}

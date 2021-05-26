using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Base
{
    public class BaseApiResponse
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}

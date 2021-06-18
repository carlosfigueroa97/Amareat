using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses
{
    public class GenericSuccessfulResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}

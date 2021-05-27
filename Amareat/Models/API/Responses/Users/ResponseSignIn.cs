using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Users
{
    public class ResponseSignIn
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}

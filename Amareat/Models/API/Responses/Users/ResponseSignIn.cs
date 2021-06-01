using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Responses.Users
{
    public class ResponseSignIn
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }
}

using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Users
{
    public class SignIn
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

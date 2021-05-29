using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Users
{
    public class SaveUser
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

    }
}

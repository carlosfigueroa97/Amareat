using System;
using Newtonsoft.Json;

namespace Amareat.Models.API.Requests.Users
{
    public class EditUser
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
